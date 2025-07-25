using System.Collections;
using UnityEngine;

public class EndState : State<GameManager>
{
    public EndState(GameManager owner)
        : base(owner) { }

    public override void OnEnter()
    {
        GameEventManager.Instance.TriggerEvent(new EndStateEnterEvent());
        owner.StartCoroutine(WaitSeconds(5.0f));

        if (owner.seconds > 0)
        {
            GameEventManager.Instance.TriggerEvent(new EndStateShowFinishUIEvent(true));
        }
        else
        {
            GameEventManager.Instance.TriggerEvent(new EndStateShowTimeoutUIEvent(true));
        }
    }

    public override void OnExit()
    {
        GameEventManager.Instance.TriggerEvent(new EndStateShowFinishUIEvent(false));
        GameEventManager.Instance.TriggerEvent(new EndStateShowTimeoutUIEvent(false));
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
