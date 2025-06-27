using UnityEngine;

public class BigfootIdleState : State<Bigfoot>
{
    public BigfootIdleState(Bigfoot bigfoot)
    : base(bigfoot) { }

    public override void OnEnter()
    {
        owner.Animator.SetBool("IsAttaking", false);
    }

    public override void OnUpdate()
    {
        float distance = (owner.Target.position - owner.transform.position).magnitude;
        if (distance <= owner.AttackRadio)
        {
            owner.StateMachine.ChangeState(owner.AttackState);
        }   
    }
}
