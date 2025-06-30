using UnityEngine;

public class BigfootIdleState : State<Bigfoot>
{

    private float timer;
    private float timeToRoar;
    public BigfootIdleState(Bigfoot bigfoot)
    : base(bigfoot) { }

    public override void OnEnter()
    {
        timer = 0.0f;
        timeToRoar = Random.Range(4.0f, 10.0f);
        owner.Animator.SetBool("IsAttaking", false);
    }

    public override void OnUpdate()
    {
        float distance = (owner.Target.position - owner.transform.position).magnitude;
        if (distance <= owner.AttackRadio)
        {
            owner.StateMachine.ChangeState(owner.AttackState);
        }

        timer += Time.deltaTime;
        if (timer >= timeToRoar)
        {
            AudioManager.onPlayClip3D?.Invoke(owner.Clips.mounsterIdle, owner.transform.position, 100, 200);
            timer = 0.0f;
            timeToRoar = Random.Range(4.0f, 10.0f);
        }
    }
}
