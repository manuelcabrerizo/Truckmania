using System;
using UnityEngine;

public class PlayerRestartState : State<Player>
{
    public PlayerRestartState(Player owner, Func<bool> enterCondition, Func<bool> exitCondition) 
        : base(owner, enterCondition, exitCondition) 
    {
    }

    public override void OnEnter()
    {
        GameEventManager.Instance.AddListener<PlayerRestartEvent>(OnResetCar);
        GameEventManager.Instance.TriggerEvent(new ShowResetTextEvent(true));
    }

    public override void OnExit()
    {
        GameEventManager.Instance.RemoveListener<PlayerRestartEvent>(OnResetCar);
        GameEventManager.Instance.TriggerEvent(new ShowResetTextEvent(false));
        PlayerData data = owner.Data;
        data.wasDrifting = false;
    }

    private void OnResetCar(GameEvent gameEvent)
    {
        PlayerRestartEvent playerResetEvent = (PlayerRestartEvent)gameEvent;

        if (playerResetEvent.player != owner)
        {
            return;
        }

        PlayerData data = owner.Data;
        owner.transform.position += Vector3.up * 2.0f;
        Vector3 forward = data.cameraMovement.transform.forward;
        forward.y = 0f;
        forward.Normalize();
        owner.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
    }
}
