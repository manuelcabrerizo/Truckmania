using System;
using UnityEngine;

public class PlayerRestartState : State<Player>
{
    public static event Action<bool> onOnShowResetText;

    private CameraMovement cameraMovement;

    public PlayerRestartState(Player owner, Func<bool> enterCondition, Func<bool> exitCondition) 
        : base(owner, enterCondition, exitCondition) 
    {
        CameraMovement.onCameraCreated += OnCameraCreated;
    }

    // TODO: create a destroy function and call it from the player script
    // or move this to player OnDestroy
    ~PlayerRestartState()
    {
        CameraMovement.onCameraCreated -= OnCameraCreated;
    }

    public override void OnEnter()
    {
        Debug.Log("Restart OnEnter");
        InputManager.onResetCar += OnResetCar;
        onOnShowResetText?.Invoke(true);
    }

    public override void OnExit()
    {
        Debug.Log("Restart OnExit");
        InputManager.onResetCar -= OnResetCar;
        onOnShowResetText?.Invoke(false);
    }

    private void OnResetCar()
    {
        owner.transform.position += Vector3.up * 2.0f;
        Vector3 forward = cameraMovement.transform.forward;
        forward.y = 0f;
        forward.Normalize();
        owner.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
    }

    private void OnCameraCreated(CameraMovement cameraMovement)
    {
        this.cameraMovement = cameraMovement;
    }

}
