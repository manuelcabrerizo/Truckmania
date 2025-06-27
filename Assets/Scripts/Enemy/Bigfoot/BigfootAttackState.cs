using UnityEngine;

public class BigfootAttackState : State<Bigfoot>
{
    private ExplosiveBarrilProjectile holdingBarril = null;

    public BigfootAttackState(Bigfoot bigfoot)
        : base(bigfoot) { }

    public override void OnEnter()
    {
        owner.Animator.SetBool("IsAttaking", true);
    }

    public override void OnExit()
    {
        if (holdingBarril != null)
        {
            holdingBarril.SendReleaseEvent();
            holdingBarril = null;
        }
    }

    public override void OnUpdate()
    {
        FaceToTarget();
        SetHoldingCratePosition();

        float distance = (owner.Target.position - owner.transform.position).magnitude;
        if (distance > owner.AttackRadio)
        {
            owner.StateMachine.ChangeState(owner.IdleState);
        }
    }

    private void FaceToTarget()
    {
        Vector3 forward = owner.transform.forward;
        forward.y = 0.0f;
        forward.Normalize();
        Quaternion currentRotation = Quaternion.LookRotation(forward, Vector3.up);

        forward = owner.Target.position - owner.transform.position;
        forward.y = 0.0f;
        forward.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(forward, Vector3.up);

        Quaternion newRotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * 2.0f);
        owner.transform.rotation = newRotation;
    }

    private void SetHoldingCratePosition()
    {
        if (holdingBarril != null)
        {
            holdingBarril.transform.position = owner.Hand.position;
            holdingBarril.transform.rotation = owner.Hand.rotation;
        }
    }

    public void SpawnCrate()
    {
        holdingBarril = ProjectileSpawner.Instance.Spawn<ExplosiveBarrilProjectile>();
        holdingBarril.transform.position = owner.Hand.position;
        holdingBarril.transform.rotation = owner.Hand.rotation;
    }

    public void LunchCrate()
    {
        if (holdingBarril != null)
        {
            float distance = (owner.Target.position - owner.transform.position).magnitude;
            float attackRadioRatio = Mathf.Min(distance / owner.AttackRadio, 1.0f);
            float timeToTarget = 2.0f - (2.0f * (1.0f - attackRadioRatio));
            Vector3 targetPosition = owner.Target.position + owner.TargetBody.velocity * (timeToTarget * Random.Range(0.5f, 1.0f));
            holdingBarril.Lunch(holdingBarril.transform.position, targetPosition, timeToTarget);
            holdingBarril = null;
        }
    }
}
 