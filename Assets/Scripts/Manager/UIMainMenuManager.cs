using UnityEngine;

public class UIMainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;

    private void Awake()
    {
        GameEventManager.Instance.AddListener<SettingButtonClickEvent>(OnSettingsButtonClick);
        GameEventManager.Instance.AddListener<SettingBackButtonClickEvent>(OnSettingsBackButtonClick);
    }

    private void OnDestroy()
    {
        GameEventManager.Instance.RemoveListener<SettingButtonClickEvent>(OnSettingsButtonClick);
        GameEventManager.Instance.RemoveListener<SettingBackButtonClickEvent>(OnSettingsBackButtonClick);
    }

    private void OnSettingsButtonClick(GameEvent gameEvent)
    {
        settingsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    private void OnSettingsBackButtonClick(GameEvent gameEvent)
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }
}
