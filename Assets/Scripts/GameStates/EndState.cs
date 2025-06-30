using System;
using System.Collections;
using UnityEngine;

public class EndState : State<GameManager>
{
    public static event Action onEnter;
    public static event Action<bool> onShowFinishUI;
    public static event Action<bool> onShowTimeoutUI;

    public EndState(GameManager owner)
        : base(owner) { }

    public override void OnEnter()
    {
        onEnter?.Invoke();
        owner.StartCoroutine(WaitSeconds(5.0f));

        if (owner.seconds > 0)
        {
            onShowFinishUI?.Invoke(true);
        }
        else
        {
            onShowTimeoutUI?.Invoke(true);
        }
    }

    public override void OnExit()
    {
        onShowFinishUI?.Invoke(false);
        onShowTimeoutUI?.Invoke(false);
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
