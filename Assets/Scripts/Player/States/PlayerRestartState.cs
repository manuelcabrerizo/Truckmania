﻿using System;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerRestartState : State<Player>
{
    public static event Action<bool> onOnShowResetText;

    public PlayerRestartState(Player owner, Func<bool> enterCondition, Func<bool> exitCondition) 
        : base(owner, enterCondition, exitCondition) 
    {
    }

    public override void OnEnter()
    {
        InputManager.onResetCar += OnResetCar;
        onOnShowResetText?.Invoke(true);
    }

    public override void OnExit()
    {
        InputManager.onResetCar -= OnResetCar;
        onOnShowResetText?.Invoke(false);

        PlayerData data = owner.Data;
        data.wasDrifting = false;
    }

    private void OnResetCar()
    {
        PlayerData data = owner.Data;
        owner.transform.position += Vector3.up * 2.0f;
        Vector3 forward = data.cameraMovement.transform.forward;
        forward.y = 0f;
        forward.Normalize();
        owner.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
    }
}
