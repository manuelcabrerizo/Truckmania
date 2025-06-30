using System;
public class PlayerIdleState : State<Player>
{
    public PlayerIdleState(Player owner, Func<bool> enterCondition, Func<bool> exitCondition)
        : base(owner, enterCondition, exitCondition) { }

    public override void OnEnter()
    {
        PlayerData data = owner.Data;
        data.body.isKinematic = true;
        data.Restart();
        data.engineSound.Stop();
    }

    public override void OnExit()
    {
        PlayerData data = owner.Data;
        data.body.isKinematic = false;
        if (data.engineSound.enabled)
        {
            data.engineSound.Play();
        }
    }
}
