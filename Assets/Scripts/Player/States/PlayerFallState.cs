using System;
using UnityEngine;

public class PlayerFallState : State<Player>
{
    public PlayerFallState(Player owner, Func<bool> enterCondition, Func<bool> exitCondition)
    : base(owner, enterCondition, exitCondition) { }

    public override void OnEnter()
    {
        Debug.Log("Fall OnEnter");
        PlayerData data = owner.Data;
        if (data.wasDrifting)
        {
            data.keepDrifting = true;
        }
    }

    public override void OnExit()
    {
        Debug.Log("Fall OnExit");
        PlayerData data = owner.Data;
        data.wasDrifting = false;


    }

    public override void OnFixedUpdate()
    {
        PlayerData data = owner.Data;
        data.body.AddTorque(data.steer * data.body.transform.up * data.playerData.airRotVelocity, ForceMode.Acceleration);
        data.body.AddTorque(data.flip * data.body.transform.right * data.playerData.airRotVelocity, ForceMode.Acceleration);
    }
}
