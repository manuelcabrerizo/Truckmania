using System;
using System.Threading;
using UnityEditor;
using UnityEngine;
public class PlayerIdleState : State<Player>
{
    public PlayerIdleState(Player owner, Func<bool> enterCondition, Func<bool> exitCondition)
        : base(owner, enterCondition, exitCondition) { }

    public override void OnEnter()
    {
        PlayerData data = owner.Data;
        data.body.isKinematic = true;
        data.Restart();
    }

    public override void OnExit()
    {
        PlayerData data = owner.Data;
        data.body.isKinematic = false;
    }
}
