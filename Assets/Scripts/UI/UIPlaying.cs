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
        PlayingState.onUpdateCoinPickText += OnUpdateCoinPickText;
        PlayingState.onUpdateEnemyKillText += OnUpdateEnemyKillText;
        PlayingState.onUpdateTimeText += OnUpdateTimeText;
        PlayerRestartState.onOnShowResetText += OnShowResetText;
    }

    private void OnDestroy()
    {
        PlayingState.onUpdateCoinPickText -= OnUpdateCoinPickText;
        PlayingState.onUpdateEnemyKillText -= OnUpdateEnemyKillText;
        PlayingState.onUpdateTimeText -= OnUpdateTimeText;
        PlayerRestartState.onOnShowResetText -= OnShowResetText;
    }

    private void Start()
    {
        pressRToRestartText.gameObject.SetActive(false);
    }

    public void OnUpdateCoinPickText(int coinCount, int coinSpawn)
    {
        coinCountText.text = "You grabbed " + coinCount + " coins of " + coinSpawn;
    }

    public void OnUpdateEnemyKillText(int enemyCount, int enemySpawn)
    {
        enemyCountText.text = "You Kill " + enemyCount + " enemies of " + enemySpawn;
    }

    private void OnShowResetText(bool value)
    {
        pressRToRestartText.gameObject.SetActive(value);
    }

    private void OnUpdateTimeText(int seconds)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        timeText.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
    }

}
