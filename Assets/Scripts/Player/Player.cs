using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    public static event Action<Player> onPlayerCreated;
    public static event Action onPlayerHit;

    public static Vector3 startPosition;
    public static Quaternion startRotation;

    [field:SerializeField] public PlayerData Data { get; private set; }
    private StateMachine stateMachine = null;
    private StateMachine additiveStateMachine = null;
    private StateGraph<Player> stateGraph = null;
    private List<State<Player>> basicStates = null;
    private List<State<Player>> additiveStates = null;

    private void Awake()
    {
        Data.Initialize();

        startPosition = transform.position;
        startRotation = transform.rotation;

        Data.transform = transform;
        Data.body = GetComponent<Rigidbody>();
        Data.collision = GetComponent<Collider>();
        Data.aimBar = GetComponent<PlayerAimBar>();
        Data.engineSound = GetComponent<AudioSource>();

        stateMachine = new StateMachine();
        additiveStateMachine = new StateMachine();

        InitializeStates();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        foreach (Material mat in Data.materials)
        {
            mat.SetColor("_Tint", Color.black);
        }

        stateMachine.Clear();
        additiveStateMachine.Clear();
        
        Data.Destroy();
    }

    private void Start()
    {
        onPlayerCreated?.Invoke(this);
    }

    private void Update()
    {
        Data.Update(this);

        stateMachine.Update();
        additiveStateMachine.Update();
        ProcessBasicStates();
        ProcessAdditiveStates();
    }

    private void FixedUpdate()
    {
        Data.FixedUpdate(this);

        stateMachine.FixedUpdate();
        additiveStateMachine.FixedUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        IPickable pickable = null;
        if (other.gameObject.TryGetComponent<IPickable>(out pickable))
        {
            AudioManager.onPlayClip?.Invoke(Data.clips.barrilPickup);
            StartCoroutine(StartFeedbackAnimation(0.5f, Color.yellow));
            pickable.PickUp();
        }
    }

    private void InitializeStates()
    {
        State<Player> idleState = new PlayerIdleState(this,
            () => { return Data.isCoundown; }, () => { return !Data.isCoundown; });
        State<Player> driveState = new PlayerDriveState(this,
            () => { return Data.breaking <= 0.01f &&
                           !Data.keepDrifting &&
                           (Data.sliptAngle <= Data.playerData.driftAngle) && 
                           !Data.isCoundown; });
        State<Player> driftState = new PlayerDriftState(this,
            () => { return Data.breaking > 0.01f || Data.keepDrifting; },
            () => { return (Data.sliptAngle <= Data.playerData.driftAngle) ||
                            Data.isGrounded == false || Data.isCoundown || Data.isNoclipCheatActive; });
        State<Player> fallState = new PlayerFallState(this,
            () => { return !Data.isGrounded; },
            () => { return Data.isGrounded || Data.body.velocity.magnitude <= 0.01f || Data.isCoundown || Data.isNoclipCheatActive; });
        State<Player> restartState = new PlayerRestartState(this,
            () => { return Data.upsideDownRatio < 0.25f && Data.body.velocity.magnitude <= 0.01f; },
            () => { return Data.upsideDownRatio >= 0.25f || Data.isCoundown || Data.isNoclipCheatActive; });
        State<Player> barrilState = new PlayerBarrilState(this, () => { return Data.barril != null; });
        State<Player> noclip = new PlayerNoclipState(this,
            () => { return Data.isNoclipCheatActive; },
            () => { return !Data.isNoclipCheatActive || Data.isCoundown; });

        StateGraph<Player> stateGraph = new StateGraph<Player>();
        stateGraph.AddStateTransitions(idleState, new List<State<Player>> { driveState });
        stateGraph.AddStateTransitions(driveState, new List<State<Player>> { driftState, fallState, barrilState, restartState, idleState, noclip });
        stateGraph.AddStateTransitions(driftState, new List<State<Player>> { driveState, fallState, barrilState, restartState, idleState, noclip });
        stateGraph.AddStateTransitions(fallState, new List<State<Player>> { driveState, driftState, barrilState, restartState, idleState, noclip });
        stateGraph.AddStateTransitions(restartState, new List<State<Player>> { driveState, fallState, barrilState, idleState, noclip });
        stateGraph.AddStateTransitions(barrilState, new List<State<Player>> { driveState, driftState, fallState, restartState, idleState, noclip });
        stateGraph.AddStateTransitions(noclip, new List<State<Player>> { driveState, driftState, fallState, restartState, idleState, barrilState });

        List<State<Player>> basicStates = new List<State<Player>> { driveState, driftState, fallState, restartState, idleState, noclip };
        List<State<Player>> additiveStates = new List<State<Player>> { barrilState };

        this.basicStates = basicStates;
        this.additiveStates = additiveStates;
        this.stateGraph = stateGraph;
        this.stateMachine.PushState(driveState);
    }

    private void ProcessBasicStates()
    {
        foreach (State<Player> state in basicStates)
        {
            State<Player> currentState = stateMachine.PeekState() as State<Player>;
            if (currentState.ExitCondition())
            {
                if (state.EnterCondition() && currentState != state)
                {
                    if (stateGraph.IsValid(currentState, state))
                    {
                        stateMachine.ChangeState(state);
                    }
                }
            }
        }
    }

    private void ProcessAdditiveStates()
    {
        foreach (State<Player> state in additiveStates)
        {
            if (state.EnterCondition())
            {
                if (additiveStateMachine.PeekState() != state)
                {
                    if (stateGraph.IsValid(stateMachine.PeekState() as State<Player>, state))
                    {
                        additiveStateMachine.PushState(state);
                    }
                }
            }
            else if (additiveStateMachine.PeekState() != null && additiveStateMachine.PeekState() == state)
            {
                if (stateGraph.IsValid(additiveStateMachine.PeekState() as State<Player>, stateMachine.PeekState() as State<Player>))
                {
                    additiveStateMachine.PopState();
                }
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if (!Data.isGodModeCheatActive)
        {
            StartCoroutine(StartFeedbackAnimation(2.0f, Color.red));
            onPlayerHit?.Invoke();
        }
    }

    IEnumerator StartFeedbackAnimation(float duration, Color color)
    {
        float time = 0.0f;
        while (time <= duration)
        {
            time += Time.deltaTime;
            foreach (Material mat in Data.materials)
            {
                mat.SetColor("_Tint", Color.Lerp(Color.black, color, Mathf.Sin(time * 40)));
            }
            yield return new WaitForEndOfFrame(); 
        }
        foreach (Material mat in Data.materials)
        {
            mat.SetColor("_Tint", Color.black);
        }
    }
}
