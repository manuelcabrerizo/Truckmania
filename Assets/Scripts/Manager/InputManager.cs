using UnityEngine.InputSystem;

public class InputManager : MonoBehaviourSingleton<InputManager>
{
    public bool JoystickOrKeyboardUse { get; private set; }
    private Player player = null;

    protected override void OnAwaken()
    {
        JoystickOrKeyboardUse = true;
        GameEventManager.Instance.AddListener<PlayerCreatedEvent>(OnPlayerCreated);
    }

    protected override void OnDestroyed()
    {
        GameEventManager.Instance.RemoveListener<PlayerCreatedEvent>(OnPlayerCreated);
    }

    private void OnPlayerCreated(GameEvent gameEvent)
    {
        PlayerCreatedEvent playerCreatedEvent = (PlayerCreatedEvent)gameEvent;
        this.player = playerCreatedEvent.player;
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
            GameEventManager.Instance.TriggerEvent(GodModeCheatEvent.GetEvent());
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
            GameEventManager.Instance.TriggerEvent(PauseEvent.GetEvent());
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
            GameEventManager.Instance.TriggerEvent(LockCameraEvent.GetEvent());
        }
    }

    public void OnWinCheat(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameEventManager.Instance.TriggerEvent(WinCheatEvent.GetEvent());
        }
    }

    public void OnLoseCheat(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameEventManager.Instance.TriggerEvent(LoseCheatEvent.GetEvent());
        }
    }

    public void OnJoystickUse(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (JoystickOrKeyboardUse == false)
            {
                GameEventManager.Instance.TriggerEvent(JoystickOrKeyboardUseEvent.GetEvent());
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
                GameEventManager.Instance.TriggerEvent(JoystickOrKeyboardUseEvent.GetEvent());
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
