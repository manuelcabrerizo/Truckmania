using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWin : MonoBehaviour
{
    [SerializeField] private Button nextButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private TextMeshProUGUI currentTimeText;
    [SerializeField] private TextMeshProUGUI bestTimeText;

    private void Awake()
    {
        GameEventManager.Instance.AddListener<CurrentTimeSetEvent>(OnCurrentTimeSet);
        GameEventManager.Instance.AddListener<BestTimeSetEvent>(OnBestTimeSet);
        nextButton.onClick.AddListener(OnNextButtonClick);
        resetButton.onClick.AddListener(OnResetButtonClick);
        menuButton.onClick.AddListener(OnMenuButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);
    }

    private void OnDestroy()
    {
        GameEventManager.Instance.RemoveListener<CurrentTimeSetEvent>(OnCurrentTimeSet);
        GameEventManager.Instance.RemoveListener<BestTimeSetEvent>(OnBestTimeSet);
        nextButton.onClick.RemoveListener(OnNextButtonClick);
        resetButton.onClick.RemoveListener(OnResetButtonClick);
        menuButton.onClick.RemoveListener(OnMenuButtonClick);
        exitButton.onClick.RemoveListener(OnExitButtonClick);
    }

    private void OnCurrentTimeSet(GameEvent gameEvent)
    {
        CurrentTimeSetEvent e = gameEvent as CurrentTimeSetEvent;
        TimeSpan timeSpan = TimeSpan.FromSeconds(e.seconds);
        currentTimeText.text = e.text + $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
    }

    private void OnBestTimeSet(GameEvent gameEvent)
    {
        BestTimeSetEvent e = gameEvent as BestTimeSetEvent;
        TimeSpan timeSpan = TimeSpan.FromSeconds(e.seconds);
        bestTimeText.text = e.text + $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
    }

    private void OnExitButtonClick()
    {
        GameEventManager.Instance.TriggerEvent(new ExitButtonClickEvent());
    }

    private void OnMenuButtonClick()
    {
        GameEventManager.Instance.TriggerEvent(new MenuButtonClickEvent());
    }

    private void OnResetButtonClick()
    {
        GameEventManager.Instance.TriggerEvent(new ResetButtonClickEvent());
    }

    private void OnNextButtonClick()
    {
        GameEventManager.Instance.TriggerEvent(new NextButtonClickEvent());
    }
}
