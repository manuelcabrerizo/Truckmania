using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button controlsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(OnPlayButtonClick);
        controlsButton.onClick.AddListener(OnControlsButtonClick);
        creditsButton.onClick.AddListener(OnCreditsButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);
        settingsButton.onClick.AddListener(OnSettingsButtonClick);
    }

    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(OnPlayButtonClick);
        controlsButton.onClick.RemoveListener(OnControlsButtonClick);
        creditsButton.onClick.RemoveListener(OnCreditsButtonClick);
        exitButton.onClick.RemoveListener(OnExitButtonClick);
        settingsButton.onClick.RemoveListener(OnSettingsButtonClick);
    }

    private void Start()
    {
        GameEventManager.Instance.TriggerEvent(DiscordUpdateStateEvent.GetEvent("Main Menu"));
    }

    private void OnEnable()
    {
        GameEventManager.Instance.AddListener<JoystickOrKeyboardUseEvent>(OnJoystickAndKeyboardUse);
        OnJoystickAndKeyboardUse(null);
    }

    private void OnDisable()
    {
        GameEventManager.Instance.RemoveListener<JoystickOrKeyboardUseEvent>(OnJoystickAndKeyboardUse);
    }

    private void OnPlayButtonClick()
    {
        LevelManager.Instance.LoadFirstLevel();
    }

    private void OnControlsButtonClick()
    {
        SceneManager.LoadScene("Controls");
    }

    private void OnCreditsButtonClick()
    {
        SceneManager.LoadScene("Credits");
    }

    private void OnSettingsButtonClick()
    {
        GameEventManager.Instance.TriggerEvent(SettingButtonClickEvent.GetEvent());
    }

    private void OnExitButtonClick()
    {
#if UNITY_WEBGL
        return;
#endif
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void OnJoystickAndKeyboardUse(GameEvent gameEvent)
    {
        EventSystem.current.firstSelectedGameObject = playButton.gameObject;
        playButton.Select();
    }
}
