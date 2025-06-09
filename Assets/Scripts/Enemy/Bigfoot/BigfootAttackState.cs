using UnityEngine;

public class BigfootAttackState : BigfootState
{
    private BarrilProjectile holdingBarril = null;

    public BigfootAttackState(Bigfoot bigfoot)
        : base(bigfoot) { }

    public override void OnEnter()
    {
        bigfoot.Animator.SetBool("IsAttaking", true);
    }

    public override void OnUpdate()
    {
        FaceToTarget();
        SetHoldingCratePosition();

        float distance = (bigfoot.Target.position - bigfoot.transform.position).magnitude;
        if (distance > bigfoot.AttackRadio)
        {
            bigfoot.StateMachine.ChangeState(bigfoot.IdleState);
        }
    }

    private void FaceToTarget()
    {
        Vector3 forward = bigfoot.transform.forward;
        forward.y = 0.0f;
        forward.Normalize();
        Quaternion currentRotation = Quaternion.LookRotation(forward, Vector3.up);

        forward = bigfoot.Target.position - bigfoot.transform.position;
        forward.y = 0.0f;
        forward.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(forward, Vector3.up);

        Quaternion newRotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * 2.0f);
        bigfoot.transform.rotation = newRotation;
    }

    private void SetHoldingCratePosition()
    {
        if (holdingBarril != null)
        {
            holdingBarril.transform.position = bigfoot.Hand.position;
            holdingBarril.transform.rotation = bigfoot.Hand.rotation;
        }
    }

    public void SpawnCrate()
    {
        holdingBarril = ProjectileSpawner.Instance.Spawn<BarrilProjectile>();
        holdingBarril.transform.position = bigfoot.Hand.position;
        holdingBarril.transform.rotation = bigfoot.Hand.rotation;
    }

    public void LunchCrate()
    {
        float distance = (bigfoot.Target.position - bigfoot.transform.position).magnitude;
        float attackRadioRatio = Mathf.Min(distance / bigfoot.AttackRadio, 1.0f);
        float timeToTarget = 2.0f - (2.0f * (1.0f - attackRadioRatio));
        holdingBarril.Lunch(holdingBarril.transform.position, bigfoot.Target.position, timeToTarget);
        holdingBarril = null;
    }
}
 