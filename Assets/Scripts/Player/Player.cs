using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    public static event Action<Player> onPlayerCreated;
    private static Vector3 startPosition;
    private static Quaternion startRotation;

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

        Data.aimBar = GetComponent<PlayerAimBar>();
        Data.body = GetComponent<Rigidbody>();

        stateMachine = new StateMachine();
        additiveStateMachine = new StateMachine();

        InitializeStates();
    }

    private void OnDestroy()
    {
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
            pickable.PickUp();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Utils.CheckCollisionLayer(collision.gameObject, Data.drivableLayer))
        {
            Data.trickDone = true;
        }
    }

    private void InitializeStates()
    {
        State<Player> driveState = new PlayerDriveState(this,
            () => { return Data.breaking <= 0.01f; });
        State<Player> driftState = new PlayerDriftState(this,
            () => { return Data.breaking > 0.01f; },
            () => { return (Data.sliptAngle * Data.playerData.turnCurve.Evaluate(Mathf.Abs(Data.velocityRatio)) <= Data.playerData.driftAngle) ||
                            Data.isGrounded == false; });
        State<Player> fallState = new PlayerFallState(this,
            () => { return !Data.isGrounded; },
            () => { return Data.isGrounded || Data.body.velocity.magnitude <= 0.01f || Mathf.Abs(Data.flip) > 0.01f; });
        State<Player> restartState = new PlayerRestartState(this,
            () => { return Data.upsideDownRatio < 0.25f && Data.body.velocity.magnitude <= 0.01f; },
            () => { return Data.upsideDownRatio >= 0.25f; });
        State<Player> barrilState = new PlayerBarrilState(this, () => { return Data.barril != null; });
        State<Player> flipState = new PlayerFlipState(this,
            () => { return Mathf.Abs(Data.flip) > 0.01f && !Data.trickDone; },
            () => { return Data.isGrounded || Data.trickDone; });

        StateGraph<Player> stateGraph = new StateGraph<Player>();
        stateGraph.AddStateTransitions(driveState, new List<State<Player>> { driftState, fallState, barrilState, restartState });
        stateGraph.AddStateTransitions(driftState, new List<State<Player>> { driveState, fallState, barrilState, restartState });
        stateGraph.AddStateTransitions(fallState, new List<State<Player>> { driveState, driftState, barrilState, restartState, flipState });
        stateGraph.AddStateTransitions(barrilState, new List<State<Player>> { driveState, driftState, fallState, restartState });
        stateGraph.AddStateTransitions(restartState, new List<State<Player>> { driveState, fallState, barrilState });
        stateGraph.AddStateTransitions(flipState, new List<State<Player>> { fallState, driveState, driftState, restartState, barrilState });
        
        List<State<Player>> basicStates = new List<State<Player>> { driveState, driftState, fallState, restartState, flipState };
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
    }
}
