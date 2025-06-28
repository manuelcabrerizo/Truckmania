using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Bigfoot : Enemy
{
    [SerializeField] private Transform hand = null;
    [SerializeField] private Transform target = null;
    [SerializeField] private float attackRadio = 4.0f;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer = null;

    private StateMachine stateMachine = null;
    private BigfootIdleState idleState = null;
    private BigfootAttackState attackState = null;
    private BigfootDeadState deadState = null;
    private BigfootHitState hitState = null;
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
    public BigfootHitState HitState => hitState;
    public Animator Animator => animator;
    public SkinnedMeshRenderer SkinnedMeshRenderer => skinnedMeshRenderer;

    protected override void OnAwaken()
    {
        collision = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        TargetBody = target.GetComponent<Rigidbody>();
        stateMachine = new StateMachine();
        idleState = new BigfootIdleState(this);
        attackState = new BigfootAttackState(this);
        deadState = new BigfootDeadState(this);
        hitState = new BigfootHitState(this);
    }

    protected override void OnStart()
    {
        stateMachine.PushState(idleState);
    }

    protected override void OnDestroyed()
    {
        SkinnedMeshRenderer.material.SetColor("_Tint", Color.black);
        stateMachine.Clear();
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        if (life == 0)
        {
            stateMachine.ChangeState(deadState);
        }
        else
        {
            stateMachine.ChangeState(hitState);
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
        deadState.SetDead();
    }

    public void HitAnimationEnd()
    {
        stateMachine.ChangeState(idleState);
    }
}
