using System;
using UnityEngine;

public class PlayerNoclipState : State<Player>
{
    private float originalDrag;

    public PlayerNoclipState(Player owner, Func<bool> enterCondition, Func<bool> exitCondition) 
        : base(owner, enterCondition, exitCondition) { }

    public override void OnEnter()
    {
        PlayerData data = owner.Data;
        data.body.useGravity = false;
        data.body.velocity = Vector3.zero;
        originalDrag = data.body.drag;
        data.body.drag = 1.0f;
        data.collision.enabled = false;

        Vector3 forward = data.transform.forward;
        forward.y = 0;
        forward.Normalize();
        data.transform.rotation = Quaternion.LookRotation(forward);
        data.isGrounded = true;
    }

    public override void OnExit()
    {
        PlayerData data = owner.Data;
        data.body.useGravity = true;
        data.body.drag = originalDrag;
        data.collision.enabled = true;
        data.isGrounded = false;

    }

    public override void OnUpdate()
    {
        PlayerData data = owner.Data;
        data.body.AddTorque(data.steer * data.body.transform.up * 8.0f, ForceMode.Acceleration);

        data.body.AddForce(data.transform.forward * data.accel * 50.0f, ForceMode.Acceleration);
        data.body.AddForce(Vector3.up * data.flip * 50.0f, ForceMode.Acceleration);
    }
}
