using System;
using UnityEngine;

public class PlayerFallState : State<Player>
{
    public PlayerFallState(Player owner, Func<bool> enterCondition, Func<bool> exitCondition)
    : base(owner, enterCondition, exitCondition) { }

    public override void OnEnter()
    {
        Debug.Log("Fall State OnEnter");
    }

    public override void OnExit()
    {
        Debug.Log("Fall State OnExit");
    }

    public override void OnFixedUpdate()
    {
        ProcessAirMovement();
    }

    private void ProcessAirMovement()
    {
        PlayerData data = owner.Data;
        if (data.sideFlip > 0.0f)
        {
            data.body.AddTorque(-data.steer * data.body.transform.forward * data.playerData.airRotVelocity * 1.5f, ForceMode.Acceleration);
        }
        else
        {
            data.body.AddTorque(data.steer * data.body.transform.up * data.playerData.airRotVelocity, ForceMode.Acceleration);
        }
        data.body.AddTorque(data.flip * data.body.transform.right * data.playerData.airRotVelocity, ForceMode.Acceleration);
    }
}
