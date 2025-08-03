using System;
using TMPro;
using UnityEngine;

public class UIPlaying : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinCountText;
    [SerializeField] private TextMeshProUGUI enemyCountText;
    [SerializeField] private TextMeshProUGUI pressRToRestartText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject rKeyImage;
    [SerializeField] private GameObject bButtonImage;

    private void Awake()
    {
        GameEventManager.Instance.AddListener<UpdateCoinPickTextEvent>(OnUpdateCoinPickText);
        GameEventManager.Instance.AddListener<UpdateEnemyKillTextEvent>(OnUpdateEnemyKillText);
        GameEventManager.Instance.AddListener<UpdateTimeTextEvent>(OnUpdateTimeText);
        GameEventManager.Instance.AddListener<ShowResetTextEvent>(OnShowResetText);
        GameEventManager.Instance.AddListener<KeyboardUseEvent>(OnKeyboardUse);
        GameEventManager.Instance.AddListener<JoystickUseEvent>(OnJoystickUse);
        GameEventManager.Instance.TriggerEvent(ResetInputEvent.GetEvent());
    }

    private void OnDestroy()
    {
        GameEventManager.Instance.RemoveListener<UpdateCoinPickTextEvent>(OnUpdateCoinPickText);
        GameEventManager.Instance.RemoveListener<UpdateEnemyKillTextEvent>(OnUpdateEnemyKillText);
        GameEventManager.Instance.RemoveListener<UpdateTimeTextEvent>(OnUpdateTimeText);
        GameEventManager.Instance.RemoveListener<ShowResetTextEvent>(OnShowResetText);
        GameEventManager.Instance.RemoveListener<KeyboardUseEvent>(OnKeyboardUse);
        GameEventManager.Instance.RemoveListener<JoystickUseEvent>(OnJoystickUse);
    }

    private void Start()
    {
        pressRToRestartText.gameObject.SetActive(false);
    }

    public void OnUpdateCoinPickText(GameEvent gameEvent)
    {
        UpdateCoinPickTextEvent e = (UpdateCoinPickTextEvent)gameEvent;
        coinCountText.text = "You grabbed " + e.coinCount + " coins of " + e.coinSpawn;
    }

    public void OnUpdateEnemyKillText(GameEvent gameEvent)
    {
        UpdateEnemyKillTextEvent e = (UpdateEnemyKillTextEvent)gameEvent;
        enemyCountText.text = "You Killed " + e.enemyCount + " enemies of " + e.enemySpawn;
    }

    private void OnShowResetText(GameEvent gameEvent)
    {
        ShowResetTextEvent e = (ShowResetTextEvent)gameEvent;
        pressRToRestartText.gameObject.SetActive(e.show);
    }

    private void OnUpdateTimeText(GameEvent gameEvent)
    {
        UpdateTimeTextEvent e = (UpdateTimeTextEvent)gameEvent;
        TimeSpan timeSpan = TimeSpan.FromSeconds(e.seconds);
        timeText.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
    }

    private void OnKeyboardUse(GameEvent gameEvent)
    { 
        rKeyImage.gameObject.SetActive(true);
        bButtonImage.gameObject.SetActive(false);
    }

    private void OnJoystickUse(GameEvent gameEvent)
    {
        rKeyImage.gameObject.SetActive(false);
        bButtonImage.gameObject.SetActive(true);
    }
}
