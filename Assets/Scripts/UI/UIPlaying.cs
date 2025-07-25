using System;
using TMPro;
using UnityEngine;

public class UIPlaying : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinCountText;
    [SerializeField] private TextMeshProUGUI enemyCountText;
    [SerializeField] private TextMeshProUGUI pressRToRestartText;
    [SerializeField] private TextMeshProUGUI timeText;

    private void Awake()
    {
        GameEventManager.Instance.AddListener<UpdateCoinPickTextEvent>(OnUpdateCoinPickText);
        GameEventManager.Instance.AddListener<UpdateEnemyKillTextEvent>(OnUpdateEnemyKillText);
        GameEventManager.Instance.AddListener<UpdateTimeTextEvent>(OnUpdateTimeText);
        PlayerRestartState.onOnShowResetText += OnShowResetText;
    }

    private void OnDestroy()
    {
        GameEventManager.Instance.RemoveListener<UpdateCoinPickTextEvent>(OnUpdateCoinPickText);
        GameEventManager.Instance.RemoveListener<UpdateEnemyKillTextEvent>(OnUpdateEnemyKillText);
        GameEventManager.Instance.RemoveListener<UpdateTimeTextEvent>(OnUpdateTimeText);
        PlayerRestartState.onOnShowResetText -= OnShowResetText;
    }

    private void Start()
    {
        pressRToRestartText.gameObject.SetActive(false);
    }

    public void OnUpdateCoinPickText(GameEvent gameEvent)
    {
        UpdateCoinPickTextEvent e = gameEvent as UpdateCoinPickTextEvent;
        coinCountText.text = "You grabbed " + e.coinCount + " coins of " + e.coinSpawn;
    }

    public void OnUpdateEnemyKillText(GameEvent gameEvent)
    {
        UpdateEnemyKillTextEvent e = gameEvent as UpdateEnemyKillTextEvent;
        enemyCountText.text = "You Kill " + e.enemyCount + " enemies of " + e.enemySpawn;
    }

    private void OnShowResetText(bool value)
    {
        pressRToRestartText.gameObject.SetActive(value);
    }

    private void OnUpdateTimeText(GameEvent gameEvent)
    {
        UpdateTimeTextEvent e = gameEvent as UpdateTimeTextEvent;
        TimeSpan timeSpan = TimeSpan.FromSeconds(e.seconds);
        timeText.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
    }

}
