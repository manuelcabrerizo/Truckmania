using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviourSingleton<InputManager>
{
    public static event Action onPause;
    public static event Action onLockCamera;
    public static event Action onWinCheat;
    public static event Action onLoseCheat;
    public static event Action onGodModeCheat;
    public static event Action onJoystickOrKeyboardUse;

    public bool JoystickOrKeyboardUse { get; private set; }
    private Player player = null;

    protected override void OnAwaken()
    {
        JoystickOrKeyboardUse = true;
        Player.onPlayerCreated += OnPlayerCreated;
    }

    protected override void OnDestroyed()
    {
        Player.onPlayerCreated -= OnPlayerCreated;
    }

    private void OnPlayerCreated(Player player)
    {
        this.player = player;
    }

    public void OnAccelerate(InputAction.CallbackContext context)
    {
        player.Data.accel = context.ReadValue<float>();
    }

    public void OnBreak(InputAction.CallbackContext context)
    {
        player.Data.breaking = context.ReadValue<float>();
    }

    public void OnSteer(InputAction.CallbackContext context)
    {
        player.Data.steer = context.ReadValue<float>();
    }

    public void OnFlip(InputAction.CallbackContext context)
    {
        player.Data.flip = context.ReadValue<float>();
    }

    public void OnSideFlip(InputAction.CallbackContext context)
    {
        player.Data.sideFlip = context.ReadValue<float>();
    }

    public void OnGodModeCheat(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            player.SetGodMode();
            onGodModeCheat?.Invoke();
        }
    }

    public void OnNoclipCheat(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            player.SetNoclipMode();
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            player.Shoot();
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
            player.Restart();
        }
    }

    public void OnLockCamera(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            onLockCamera?.Invoke();
        }
    }

    public void OnWinCheat(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            onWinCheat?.Invoke();
        }
    }

    public void OnLoseCheat(InputAction.CallbackContext context)
    {
        if (context.started)
        { 
            onLoseCheat?.Invoke();
        }
    }

    public void OnJoystickUse(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (JoystickOrKeyboardUse == false)
            {
                onJoystickOrKeyboardUse?.Invoke();
                JoystickOrKeyboardUse = true;
            }
        }
    }

    public void OnKeyboardUse(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (JoystickOrKeyboardUse == false)
            {
                onJoystickOrKeyboardUse?.Invoke();
                JoystickOrKeyboardUse = true;
            }
        }
    }

    public void OnMouseUse(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JoystickOrKeyboardUse = false;
        }
    }

}
