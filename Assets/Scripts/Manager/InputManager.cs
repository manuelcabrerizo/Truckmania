using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static event Action<float> onAccelerate;
    public static event Action<float> onBreak;
    public static event Action<float> onSteer;
    public static event Action<float> onFlip;
    public static event Action onJump;
    public static event Action onShoot;
    public static event Action onPause;
    public static event Action onResetCar;
    public static event Action onLockCamera;

    public void OnAccelerate(InputAction.CallbackContext context)
    {
        onAccelerate?.Invoke(context.ReadValue<float>());
    }

    public void OnBreak(InputAction.CallbackContext context)
    {
        onBreak?.Invoke(context.ReadValue<float>());
    }

    public void OnSteer(InputAction.CallbackContext context)
    {
        onSteer?.Invoke(context.ReadValue<float>());
    }

    public void OnFlip(InputAction.CallbackContext context)
    {
        onFlip?.Invoke(context.ReadValue<float>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            onJump?.Invoke();
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        { 
            onShoot?.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
#if UNITY_WEBGL
        if(context.control.name == "escape")
        {
            return;
        }
#endif
        if (context.started)
        {
            onPause?.Invoke();
        }
    }

    public void OnResetCar(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            onResetCar?.Invoke();
        }
    }

    public void OnLockCamera(InputAction.CallbackContext context)
    {
        if (context.started)
        { 
            onLockCamera?.Invoke();
        }
    }
}
