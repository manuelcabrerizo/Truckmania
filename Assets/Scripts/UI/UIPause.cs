using System;
using UnityEngine;
using UnityEngine.UI;

public class UIPause : MonoBehaviour
{
    [SerializeField] private Button pauseResumeButton;
    [SerializeField] private Button pauseResetButton;
    [SerializeField] private Button pauseSettingsButton;
    [SerializeField] private Button pauseMenuButton;
    [SerializeField] private Button pauseExitButton;

    private void Awake()
    {
        pauseResumeButton.onClick.AddListener(OnResumeButtonClick);
        pauseResetButton.onClick.AddListener(OnResetButtonClick);
        pauseSettingsButton.onClick.AddListener(OnSettingsButtonClick);
        pauseMenuButton.onClick.AddListener(OnMenuButtonClick);
        pauseExitButton.onClick.AddListener(OnExitButtonClick);
    }

    private void OnDestroy()
    {
        pauseResumeButton.onClick.RemoveListener(OnResumeButtonClick);
        pauseResetButton.onClick.RemoveListener(OnResetButtonClick);
        pauseSettingsButton.onClick.RemoveListener(OnSettingsButtonClick);
        pauseMenuButton.onClick.RemoveListener(OnMenuButtonClick);
        pauseExitButton.onClick.RemoveListener(OnExitButtonClick);
    }

    private void OnSettingsButtonClick()
    {
        GameEventManager.Instance.TriggerEvent(new SettingButtonClickEvent());
    }

    private void OnResetButtonClick()
    {
        GameEventManager.Instance.TriggerEvent(new ResetButtonClickEvent());
    }

    private void OnResumeButtonClick()
    {
        GameEventManager.Instance.TriggerEvent(new ResumeButtonClickEvent());
    }

    private void OnExitButtonClick()
    {
        GameEventManager.Instance.TriggerEvent(new ExitButtonClickEvent());
    }

    private void OnMenuButtonClick()
    {
        GameEventManager.Instance.TriggerEvent(new MenuButtonClickEvent());
    }
}
