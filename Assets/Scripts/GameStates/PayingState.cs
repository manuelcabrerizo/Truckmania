using System;
using System.Collections.Generic;
using UnityEngine;

class PlayingState : State<GameManager>
{
    public static event Action<bool> onShowPlayingUI;
    public static event Action<int, int> onUpdateCoinPickText;
    public static event Action<int, int> onUpdateEnemyKillText;
    public static event Action<int> onUpdateTimeText;

    private int roundTime;
    private float timer;
    private float timerScale = 1.0f;

    public PlayingState(GameManager gameManager, int roundTime)
    : base(gameManager)
    {
        this.roundTime = roundTime;
    }
    
    public override void OnEnter()
    {        
        EndTrigger.onEndTriggerHit += OnEndTriggerHit;
        Coin.onCoinPick += OnCoinPick;
        Enemy.onEnemyKill += OnEnemyKill;
        Player.onPlayerHit += OnPlayerHit;
        InputManager.onWinCheat += OnWinCheat;
        InputManager.onLoseCheat += OnLoseCheat;
        InputManager.onGodModeCheat += OnGodModeCheat;

        owner.seconds = roundTime;
        owner.coinsCollectedCount = 0;
        owner.enemiesKillCount = 0;

        onShowPlayingUI?.Invoke(true);
        onUpdateTimeText?.Invoke(owner.seconds);
        onUpdateCoinPickText?.Invoke(owner.coinsCollectedCount, owner.Coins.Count);
        onUpdateEnemyKillText?.Invoke(owner.enemiesKillCount, owner.Enemies.Count);
    }

    public override void OnExit()
    {
        onShowPlayingUI?.Invoke(false);
        EndTrigger.onEndTriggerHit -= OnEndTriggerHit;
        Coin.onCoinPick -= OnCoinPick;
        Enemy.onEnemyKill -= OnEnemyKill;
        Player.onPlayerHit -= OnPlayerHit;
        InputManager.onWinCheat -= OnWinCheat;
        InputManager.onLoseCheat -= OnLoseCheat;
        InputManager.onGodModeCheat -= OnGodModeCheat;
    }

    public override void OnUpdate()
    {
        timer += Time.deltaTime * timerScale;
        if (timer >= 1.0f)
        {
            timer = 0;
            owner.seconds--;
            onUpdateTimeText?.Invoke(owner.seconds);
        }

        if (owner.seconds == 0)
        {
            owner.SetEndState();
        }
    }

    private void OnPlayerHit()
    {
        owner.seconds = Math.Max(owner.seconds - 20, 0);
        timer = 0;
        onUpdateTimeText?.Invoke(owner.seconds);
    }

    private void OnEndTriggerHit()
    {
        owner.SetEndState();
    }

    private void OnCoinPick(Coin coin)
    {
        owner.coinsCollectedCount++;
        onUpdateCoinPickText?.Invoke(owner.coinsCollectedCount, owner.Coins.Count);
    }

    private void OnEnemyKill(Enemy enemy)
    {
        owner.enemiesKillCount++;
        onUpdateEnemyKillText?.Invoke(owner.enemiesKillCount, owner.Enemies.Count);
    }

    private void OnWinCheat()
    {
        owner.coinsCollectedCount = owner.Coins.Count;
        owner.enemiesKillCount = owner.Enemies.Count;
        owner.SetEndState();
    }

    private void OnLoseCheat()
    {
        owner.coinsCollectedCount = 0;
        owner.enemiesKillCount = 0;
        owner.seconds = 0;
    }

    private void OnGodModeCheat()
    {
        if (timerScale > 0.5f)
        {
            timerScale = 0.0f;
        }
        else
        {
            timerScale = 1.0f;
        }
    }
}