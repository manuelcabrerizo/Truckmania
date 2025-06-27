using System;
using System.Collections.Generic;
using UnityEngine;

class PlayingState : State<GameManager>
{
    public static event Action<bool> onShowPlayingUI;
    public static event Action<int, int> onUpdateCoinPickText;
    public static event Action<int> onUpdateTimeText;

    private int roundTime;
    private int seconds;
    private float timer;

    private List<Coin> coins;
    private List<Enemy> enemies;
    private int coinsCollectedCount = 0;
    private int enemiesKillCount = 0;

    public PlayingState(GameManager gameManager, int roundTime, List<Coin> coins, List<Enemy> enemies)
    : base(gameManager)
    {
        this.roundTime = roundTime;
        this.coins = coins;
        this.enemies = enemies;
    }
    
    public override void OnEnter()
    {        
        Coin.onCoinPick += OnCoinPick;
        Enemy.onEnemyKill += OnEnemyKill;
        Player.onPlayerHit += OnPlayerHit;

        seconds = roundTime;
        coinsCollectedCount = 0;
        enemiesKillCount = 0;

        onShowPlayingUI?.Invoke(true);
        onUpdateTimeText?.Invoke(seconds);
        onUpdateCoinPickText?.Invoke(coinsCollectedCount, coins.Count);

        foreach (Coin coin in coins)
        {
            coin.Restart();
        }

        foreach (Enemy enemy in enemies)
        {
            enemy.Restart();
        }
    }

    public override void OnExit()
    {
        onShowPlayingUI?.Invoke(false);
        Coin.onCoinPick -= OnCoinPick;
        Enemy.onEnemyKill -= OnEnemyKill;
        Player.onPlayerHit -= OnPlayerHit;
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
                owner.SetWinState();
            }
            else
            {
                owner.SetGameOverState();
            }
        }
    }

    private void OnPlayerHit()
    {
        seconds = Math.Max(seconds - 20, 0);
        timer = 0;
        onUpdateTimeText?.Invoke(seconds);
    }

    private void OnCoinPick(Coin coin)
    {
        coinsCollectedCount++;
        onUpdateCoinPickText?.Invoke(coinsCollectedCount, coins.Count);
    }

    private void OnEnemyKill(Enemy enemy)
    {
        enemiesKillCount++;
    }
}