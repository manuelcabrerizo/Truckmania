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
        InputManager.onJump += OnJump;

        Data.Initialize();

        startPosition = transform.position;
        startRotation = transform.rotation;

        Data.transform = transform;
        Data.body = GetComponent<Rigidbody>();
        Data.aimBar = GetComponent<PlayerAimBar>();

        stateMachine = new StateMachine();
        additiveStateMachine = new StateMachine();

        InitializeStates();
    }

    private void OnDestroy()
    {
        InputManager.onJump -= OnJump;

        StopAllCoroutines();
        foreach (Material mat in Data.materials)
        {
            mat.SetColor("_Tint", Color.black);
        }

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
                            Data.isGrounded == false || Data.isCoundown; });
        State<Player> fallState = new PlayerFallState(this,
            () => { return !Data.isGrounded; },
            () => { return Data.isGrounded || Data.body.velocity.magnitude <= 0.01f || Data.isCoundown; });
        State<Player> restartState = new PlayerRestartState(this,
            () => { return Data.upsideDownRatio < 0.25f && Data.body.velocity.magnitude <= 0.01f; },
            () => { return Data.upsideDownRatio >= 0.25f || Data.isCoundown; });

        State<Player> barrilState = new PlayerBarrilState(this, () => { return Data.barril != null; });

        StateGraph<Player> stateGraph = new StateGraph<Player>();
        stateGraph.AddStateTransitions(idleState, new List<State<Player>> { driveState });
        stateGraph.AddStateTransitions(driveState, new List<State<Player>> { driftState, fallState, barrilState, restartState, idleState });
        stateGraph.AddStateTransitions(driftState, new List<State<Player>> { driveState, fallState, barrilState, restartState, idleState });
        stateGraph.AddStateTransitions(fallState, new List<State<Player>> { driveState, driftState, barrilState, restartState, idleState });
        stateGraph.AddStateTransitions(restartState, new List<State<Player>> { driveState, fallState, barrilState, idleState });
        stateGraph.AddStateTransitions(barrilState, new List<State<Player>> { driveState, driftState, fallState, restartState, idleState });

        List<State<Player>> basicStates = new List<State<Player>> { driveState, driftState, fallState, restartState, idleState };
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
        StartCoroutine(StartFeedbackAnimation(2.0f, Color.red));
        onPlayerHit?.Invoke();
    }

    public void OnJump()
    {
        if (Data.isGrounded)
        {
            // zeron velocity in the up direction
            Vector3 downVelocity = Vector3.Dot(-transform.up, Data.body.velocity) * transform.up;
            Data.body.velocity -= downVelocity;
            Data.body.AddForce(transform.up * 200.0f, ForceMode.Impulse);
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
