using System;
using System.Collections.Generic;
using UnityEngine;

class PlayingState : State
{
    public static event Action<bool> onShowPlayingUI;
    public static event Action<int, int> onUpdateCoinPickText;
    public static event Action<int> onUpdateTimeText;

    private int roundTime;
    private int seconds;
    private float timer;

    private List<Coin> coins;
    private int coinsCollectedCount = 0;

    public PlayingState(GameManager gameManager, int roundTime, List<Coin> coins)
    : base(gameManager)
    {
        this.roundTime = roundTime;
        this.coins = coins;
    }
    
    public override void OnEnter()
    {        
        Coin.onCoinPick += OnCoinPick;

        seconds = roundTime;
        coinsCollectedCount = 0;

        onShowPlayingUI?.Invoke(true);
        onUpdateTimeText?.Invoke(seconds);
        onUpdateCoinPickText?.Invoke(coinsCollectedCount, coins.Count);

        foreach (Coin coin in coins)
        {
            coin.Restart();
        }
    }

    public override void OnExit()
    {
        onShowPlayingUI?.Invoke(false);
        Coin.onCoinPick -= OnCoinPick;
    }

    public override void OnUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= 1.0f)
        {
            timer = 0;
            seconds--;
            onUpdateTimeText?.Invoke(seconds);
        }

        if (seconds == 0)
        {
            if (coinsCollectedCount == coins.Count)
            {
                gameManager.SetWinState();
            }
            else
            {
                gameManager.SetGameOverState();
            }
        }
    }

    private void OnCoinPick(Coin coin)
    {
        coinsCollectedCount++;
        onUpdateCoinPickText?.Invoke(coinsCollectedCount, coins.Count);
    }
}