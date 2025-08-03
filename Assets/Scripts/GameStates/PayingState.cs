using System;
using UnityEngine;

class PlayingState : State<GameManager>
{
    private int roundTime;
    private float timer;
    private float timerScale = 1.0f;
    private int monsterKillCount = 0;

    public PlayingState(GameManager gameManager, int roundTime)
    : base(gameManager)
    {
        this.roundTime = roundTime;
    }
    
    public override void OnEnter()
    {
        monsterKillCount = 0;
        if (PlayerPrefs.HasKey("MonsterKill"))
        {
            monsterKillCount = PlayerPrefs.GetInt("MonsterKill");
        }
        PlayerPrefs.SetInt("MonsterKill", monsterKillCount);

        GameEventManager.Instance.AddListener<EndTriggerHitEvent>(OnEndTriggerHit);
        GameEventManager.Instance.AddListener<CoinPickEvent>(OnCoinPick);
        GameEventManager.Instance.AddListener<EnemyKillEvent>(OnEnemyKill);
        GameEventManager.Instance.AddListener<PlayerHitEvent>(OnPlayerHit);
        GameEventManager.Instance.AddListener<WinCheatEvent>(OnWinCheat);
        GameEventManager.Instance.AddListener<LoseCheatEvent>(OnLoseCheat);
        GameEventManager.Instance.AddListener<GodModeCheatEvent>(OnGodModeCheat);

        owner.seconds = roundTime;
        owner.coinsCollectedCount = 0;
        owner.enemiesKillCount = 0;

        GameEventManager.Instance.TriggerEvent(UpdateTimeTextEvent.GetEvent(owner.seconds));
        GameEventManager.Instance.TriggerEvent(PlayingShowUIEvent.GetEvent(true));
        GameEventManager.Instance.TriggerEvent(UpdateCoinPickTextEvent.GetEvent(owner.coinsCollectedCount, owner.Coins.Count));
        GameEventManager.Instance.TriggerEvent(UpdateEnemyKillTextEvent.GetEvent(owner.enemiesKillCount, owner.Enemies.Count));
    }

    public override void OnExit()
    {
        PlayerPrefs.SetInt("MonsterKill", monsterKillCount);

        GameEventManager.Instance.TriggerEvent(PlayingShowUIEvent.GetEvent(false));

        GameEventManager.Instance.RemoveListener<EndTriggerHitEvent>(OnEndTriggerHit);
        GameEventManager.Instance.RemoveListener<CoinPickEvent>(OnCoinPick);
        GameEventManager.Instance.RemoveListener<EnemyKillEvent>(OnEnemyKill);
        GameEventManager.Instance.RemoveListener<PlayerHitEvent>(OnPlayerHit);
        GameEventManager.Instance.RemoveListener<WinCheatEvent>(OnWinCheat);
        GameEventManager.Instance.RemoveListener<LoseCheatEvent>(OnLoseCheat);
        GameEventManager.Instance.RemoveListener<GodModeCheatEvent>(OnGodModeCheat);
    }

    public override void OnUpdate()
    {
        timer += Time.deltaTime * timerScale;
        if (timer >= 1.0f)
        {
            timer = 0;
            owner.seconds--;
            GameEventManager.Instance.TriggerEvent(UpdateTimeTextEvent.GetEvent(owner.seconds));
        }

        if (owner.seconds == 0)
        {
            owner.SetEndState();
        }
    }

    private void OnPlayerHit(GameEvent gameEvent)
    {
        owner.seconds = Math.Max(owner.seconds - 20, 0);
        timer = 0;
        GameEventManager.Instance.TriggerEvent(UpdateTimeTextEvent.GetEvent(owner.seconds));
    }

    private void OnEndTriggerHit(GameEvent gameEvent)
    {
        owner.SetEndState();
    }

    private void OnCoinPick(GameEvent gameEvent)
    {
        owner.coinsCollectedCount++;
        GameEventManager.Instance.TriggerEvent(UpdateCoinPickTextEvent.GetEvent(owner.coinsCollectedCount, owner.Coins.Count));
    }

    private void OnEnemyKill(GameEvent gameEvent)
    {
        monsterKillCount++;
        if (monsterKillCount == 5)
        {
            GameEventManager.Instance.TriggerEvent(UnlockAchivementEvent.GetEvent(AchivementType.MONSTER_HUNTER));
        }
        if (monsterKillCount == 10)
        {
            GameEventManager.Instance.TriggerEvent(UnlockAchivementEvent.GetEvent(AchivementType.MONSTER_SLAYER));
        }

        owner.enemiesKillCount++;
        GameEventManager.Instance.TriggerEvent(UpdateEnemyKillTextEvent.GetEvent(owner.enemiesKillCount, owner.Enemies.Count));
    }

    private void OnWinCheat(GameEvent gameEvent)
    {
        owner.coinsCollectedCount = owner.Coins.Count;
        owner.enemiesKillCount = owner.Enemies.Count;
        owner.SetEndState();
    }

    private void OnLoseCheat(GameEvent gameEvent)
    {
        owner.coinsCollectedCount = 0;
        owner.enemiesKillCount = 0;
        owner.seconds = 0;
    }

    private void OnGodModeCheat(GameEvent gameEvent)
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