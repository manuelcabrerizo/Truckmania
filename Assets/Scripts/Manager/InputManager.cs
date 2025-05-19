using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static event Action<float> onAccelerate;
    public static event Action<float> onBreak;
    public static event Action<float> onSteer;
    public static event Action<float> onFlip;
    public static event Action onPause;
    public static event Action onResetCar;

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

    public void OnPause(InputAction.CallbackContext context)
    {
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
}
