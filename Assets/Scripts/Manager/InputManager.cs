using System;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviourSingleton<InputManager>
{
    private bool keyboardUse;
    private bool joystickUse;
    private bool joystickOrKeyboardUse;
    private Player player = null;

    protected override void OnAwaken()
    {
        keyboardUse = false;
        joystickUse = false;
        joystickOrKeyboardUse = true;
        GameEventManager.Instance.AddListener<PlayerCreatedEvent>(OnPlayerCreated);
        GameEventManager.Instance.AddListener<ResetInputEvent>(OnReset);
    }

    protected override void OnDestroyed()
    {
        GameEventManager.Instance.RemoveListener<PlayerCreatedEvent>(OnPlayerCreated);
        GameEventManager.Instance.RemoveListener<ResetInputEvent>(OnReset);
    }

    private void OnPlayerCreated(GameEvent gameEvent)
    {
        PlayerCreatedEvent playerCreatedEvent = (PlayerCreatedEvent)gameEvent;
        this.player = playerCreatedEvent.player;
    }

    private void OnReset(GameEvent gameEvent)
    {
        keyboardUse = false;
        joystickUse = false;
    }

    public void OnAccelerate(InputAction.CallbackContext context)
    {
        if (player)
            player.Data.accel = context.ReadValue<float>();
    }

    public void OnBreak(InputAction.CallbackContext context)
    {
        if(player)
            player.Data.breaking = context.ReadValue<float>();
    }

    public void OnSteer(InputAction.CallbackContext context)
    {
        if (player)
            player.Data.steer = context.ReadValue<float>();
    }

    public void OnFlip(InputAction.CallbackContext context)
    {
        if (player)
            player.Data.flip = context.ReadValue<float>();
    }

    public void OnSideFlip(InputAction.CallbackContext context)
    {
        if (player)
            player.Data.sideFlip = context.ReadValue<float>();
    }

    public void OnGodModeCheat(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (player)
                player.SetGodMode();
            GameEventManager.Instance.TriggerEvent(GodModeCheatEvent.GetEvent());
        }
    }

    public void OnNoclipCheat(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (player)
                player.SetNoclipMode();
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (player)
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
            if (player)
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
            if (joystickOrKeyboardUse == false)
            {
                GameEventManager.Instance.TriggerEvent(JoystickOrKeyboardUseEvent.GetEvent());
                joystickOrKeyboardUse = true;
            }

            if (joystickUse == false)
            {
                GameEventManager.Instance.TriggerEvent(JoystickUseEvent.GetEvent());
                joystickUse = true;
                keyboardUse = false;
            }
        }
    }

    public void OnKeyboardUse(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (joystickOrKeyboardUse == false)
            {
                GameEventManager.Instance.TriggerEvent(JoystickOrKeyboardUseEvent.GetEvent());
                joystickOrKeyboardUse = true;
            }

            if (keyboardUse == false)
            {
                GameEventManager.Instance.TriggerEvent(KeyboardUseEvent.GetEvent());
                keyboardUse = true;
                joystickUse = false;
            }
        }
    }

    public void OnMouseUse(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            joystickOrKeyboardUse = false;
        }
    }

}
