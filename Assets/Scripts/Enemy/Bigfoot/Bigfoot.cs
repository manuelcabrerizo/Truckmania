using UnityEngine;

public class Bigfoot : Enemy
{
    [SerializeField] private Transform hand = null;
    [SerializeField] private Transform target = null;
    [SerializeField] private float attackRadio = 4.0f;
    private StateMachine stateMachine = null;
    private BigfootIdleState idleState = null;
    private BigfootAttackState attackState = null;
    private BigfootDeadState deadState = null;
    private Animator animator;
    private Collider collision;

    public Transform Hand => hand;
    public Transform Target => target;
    public Rigidbody TargetBody { get; private set; }
    public float AttackRadio => attackRadio;
    public StateMachine StateMachine => stateMachine;
    public BigfootIdleState IdleState => idleState;
    public BigfootAttackState AttackState => attackState;
    public BigfootDeadState DeadState => deadState;
    public Animator Animator => animator;

    protected override void OnAwaken()
    {
        collision = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        TargetBody = target.GetComponent<Rigidbody>();
        stateMachine = new StateMachine();
        idleState = new BigfootIdleState(this);
        attackState = new BigfootAttackState(this);
        deadState = new BigfootDeadState(this);
    }

    protected override void OnStart()
    {
        stateMachine.PushState(idleState);
    }

    protected override void OnDestroyed()
    {
        stateMachine.Clear();
    }

    private void Update()
    {
        stateMachine.Update();
    }

    protected override void OnTakeDamage(GameObject attacker)
    {
        if (Utils.CheckCollisionLayer(attacker, damagableLayer))
        {
            life--;
            if (life <= 0)
            {
                stateMachine.ChangeState(deadState);
            }
        }
    }

    public override void Restart()
    {
        base.Restart();
        collision.enabled = true;
        animator.enabled = true;
        stateMachine.ChangeState(idleState);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRadio);
    }

    // Methods call from the animator
    public void SpawnCrate()
    {
        attackState.SpawnCrate();
    }

    public void LunchCrate()
    {
        attackState.LunchCrate();
    }

    public void Kill()
    {
        collision.enabled = false;
        animator.enabled = false;
        SendEnemyKillEvent();
    }
}
