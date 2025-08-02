using UnityEngine;

public class UIMainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject achivementsPanel;

    private void Awake()
    {
        GameEventManager.Instance.AddListener<SettingButtonClickEvent>(OnSettingsButtonClick);
        GameEventManager.Instance.AddListener<SettingBackButtonClickEvent>(OnSettingsBackButtonClick);

        GameEventManager.Instance.AddListener<AchivementButtonClickEvent>(OnAchivementButtonClick);
        GameEventManager.Instance.AddListener<AchivementBackButtonClickEvent>(OnAchivementBackButtonClick);
    }

    private void OnDestroy()
    {
        GameEventManager.Instance.RemoveListener<SettingButtonClickEvent>(OnSettingsButtonClick);
        GameEventManager.Instance.RemoveListener<SettingBackButtonClickEvent>(OnSettingsBackButtonClick);

        GameEventManager.Instance.RemoveListener<AchivementButtonClickEvent>(OnAchivementButtonClick);
        GameEventManager.Instance.RemoveListener<AchivementBackButtonClickEvent>(OnAchivementBackButtonClick);
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

    private void OnAchivementButtonClick(GameEvent gameEvent)
    {
        achivementsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    private void OnAchivementBackButtonClick(GameEvent gameEvent)
    {
        mainMenuPanel.SetActive(true);
        achivementsPanel.SetActive(false);
    }
}
