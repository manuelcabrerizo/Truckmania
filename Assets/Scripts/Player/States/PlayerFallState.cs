using System;
using UnityEngine;

public class PlayerFallState : State<Player>
{
    public PlayerFallState(Player owner, Func<bool> enterCondition, Func<bool> exitCondition)
    : base(owner, enterCondition, exitCondition) { }

    public override void OnEnter()
    {
        PlayerData data = owner.Data;
        if (data.wasDrifting)
        {
            data.keepDrifting = true;
        }
    }

    public override void OnExit()
    {
        PlayerData data = owner.Data;
        data.wasDrifting = false;
    }

    public override void OnUpdate()
    {
        PlayerData data = owner.Data;
        float currentPitch = Mathf.Lerp(0.75f, 1.5f, Mathf.Abs(data.accel));
        data.engineSound.pitch = currentPitch;
    }

    public override void OnFixedUpdate()
    {
        PlayerData data = owner.Data;
        data.body.AddTorque(data.steer * data.body.transform.up * data.playerData.airRotVelocity, ForceMode.Acceleration);
        data.body.AddTorque(data.flip * data.body.transform.right * data.playerData.airRotVelocity, ForceMode.Acceleration);
    }
}
