using UnityEngine;

public abstract class GameEvent
{
    protected float timeStamp;
    public GameEvent()
    {
        this.timeStamp = Time.time;
    }
}

public class ResumeButtonClickEvent : GameEvent { }
public class NextButtonClickEvent : GameEvent { }
public class ResetButtonClickEvent : GameEvent { }
public class ExitButtonClickEvent : GameEvent { }
public class MenuButtonClickEvent : GameEvent { }
public class SettingButtonClickEvent : GameEvent { }