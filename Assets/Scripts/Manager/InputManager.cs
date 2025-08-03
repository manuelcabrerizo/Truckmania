using System;
using UnityEngine.InputSystem;

public enum ControllerType
{ 
    KEYBOARD,
    JOYSTICK
}

public class InputManager : MonoBehaviourSingleton<InputManager>
{
    ControllerType currentControlerType = ControllerType.KEYBOARD;
    ControllerType lastControllerType = ControllerType.KEYBOARD;
    bool reset = false;

    private bool joystickOrKeyboardUse;
    private Player player = null;

    protected override void OnAwaken()
    {
        joystickOrKeyboardUse = true;
        GameEventManager.Instance.AddListener<PlayerCreatedEvent>(OnPlayerCreated);
        GameEventManager.Instance.AddListener<ResetInputEvent>(OnReset);
    }

    protected override void OnDestroyed()
    {
        GameEventManager.Instance.RemoveListener<PlayerCreatedEvent>(OnPlayerCreated);
        GameEventManager.Instance.RemoveListener<ResetInputEvent>(OnReset);
    }

    private void Update()
    {
        if (reset || (currentControlerType != lastControllerType))
        {
            if (currentControlerType == ControllerType.KEYBOARD)
            {
                GameEventManager.Instance.TriggerEvent(KeyboardUseEvent.GetEvent());
            }
            else if (currentControlerType == ControllerType.JOYSTICK)
            {
                GameEventManager.Instance.TriggerEvent(JoystickUseEvent.GetEvent());
            }
        }
        lastControllerType = currentControlerType;
    }

    private void OnPlayerCreated(GameEvent gameEvent)
    {
        PlayerCreatedEvent playerCreatedEvent = (PlayerCreatedEvent)gameEvent;
        this.player = playerCreatedEvent.player;
    }

    private void OnReset(GameEvent gameEvent)
    {
        reset = true;
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

            currentControlerType = ControllerType.JOYSTICK;
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

            currentControlerType = ControllerType.KEYBOARD;
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
