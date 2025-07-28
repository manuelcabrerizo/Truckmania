using System.Collections;
using UnityEngine;

public class EndState : State<GameManager>
{
    public EndState(GameManager owner)
        : base(owner) { }

    public override void OnEnter()
    {
        GameEventManager.Instance.TriggerEvent(EndStateEnterEvent.GetEvent());
        owner.StartCoroutine(WaitSeconds(5.0f));

        if (owner.seconds > 0)
        {
            GameEventManager.Instance.TriggerEvent(EndStateShowFinishUIEvent.GetEvent(true));
        }
        else
        {
            GameEventManager.Instance.TriggerEvent(EndStateShowTimeoutUIEvent.GetEvent(true));
        }
    }

    public override void OnExit()
    {
        GameEventManager.Instance.TriggerEvent(EndStateShowFinishUIEvent.GetEvent(false));
        GameEventManager.Instance.TriggerEvent(EndStateShowTimeoutUIEvent.GetEvent(false));
    }

    IEnumerator WaitSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (owner.coinsCollectedCount == owner.Coins.Count && owner.enemiesKillCount == owner.Enemies.Count && owner.seconds > 0)
        {
            owner.SetWinState();
        }
        else
        {
            owner.SetGameOverState();
        }
    }
}
