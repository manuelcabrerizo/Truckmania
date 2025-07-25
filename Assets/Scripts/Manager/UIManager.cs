using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject playingUI;
    [SerializeField] private GameObject countDownUI;
    [SerializeField] private GameObject finishUI;
    [SerializeField] private GameObject timeoutUI;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject pausePanel;
    private Selectable currentFirstButton = null;

    private void Awake()
    {
        InputManager.onJoystickOrKeyboardUse += OnJoystickAndKeyboardUse;
        GameEventManager.Instance.AddListener<PlayingShowUIEvent>(OnShowPlayingUI);
        GameEventManager.Instance.AddListener<CountDownShowUIEvent>(OnShowCountDownUI);
        GameEventManager.Instance.AddListener<EndStateShowFinishUIEvent>(OnShowFinishUI);
        GameEventManager.Instance.AddListener<EndStateShowTimeoutUIEvent>(OnShowTimeoutUI);
        GameEventManager.Instance.AddListener<WinStateEnterEvent>(OnWinStateEnter);
        GameEventManager.Instance.AddListener<WinStateExitEvent>(OnWinStateExit);
        GameEventManager.Instance.AddListener<GameOverStateEnterEvent>(OnGameOverStateEnter);
        GameEventManager.Instance.AddListener<GameOverStateExitEvent>(OnGameOverStateExit);
        GameEventManager.Instance.AddListener<PauseStateEnterEvent>(OnPauseStateEnter);
        GameEventManager.Instance.AddListener<PauseStateExitEvent>(OnPauseStateExit);
        GameEventManager.Instance.AddListener<SettingButtonClickEvent>(OnSettingsButtonClick);
        GameEventManager.Instance.AddListener<MenuButtonClickEvent>(OnMenuButtonClick);
        GameEventManager.Instance.AddListener<ExitButtonClickEvent>(OnExitButtonClick);
    }

    private void OnDestroy()
    {
        InputManager.onJoystickOrKeyboardUse -= OnJoystickAndKeyboardUse;
        GameEventManager.Instance.RemoveListener<PlayingShowUIEvent>(OnShowPlayingUI);
        GameEventManager.Instance.RemoveListener<CountDownShowUIEvent>(OnShowCountDownUI);
        GameEventManager.Instance.RemoveListener<EndStateShowFinishUIEvent>(OnShowFinishUI);
        GameEventManager.Instance.RemoveListener<EndStateShowTimeoutUIEvent>(OnShowTimeoutUI);
        GameEventManager.Instance.RemoveListener<WinStateEnterEvent>(OnWinStateEnter);
        GameEventManager.Instance.RemoveListener<WinStateExitEvent>(OnWinStateExit);
        GameEventManager.Instance.RemoveListener<GameOverStateEnterEvent>(OnGameOverStateEnter);
        GameEventManager.Instance.RemoveListener<GameOverStateExitEvent>(OnGameOverStateExit);
        GameEventManager.Instance.RemoveListener<PauseStateEnterEvent>(OnPauseStateEnter);
        GameEventManager.Instance.RemoveListener<PauseStateExitEvent>(OnPauseStateExit);
        GameEventManager.Instance.RemoveListener<SettingButtonClickEvent>(OnSettingsButtonClick);
        GameEventManager.Instance.RemoveListener<MenuButtonClickEvent>(OnMenuButtonClick);
        GameEventManager.Instance.RemoveListener<ExitButtonClickEvent>(OnExitButtonClick);
    }

    private void OnShowPlayingUI(GameEvent gameEvent)
    {
        PlayingShowUIEvent showUIEvent = gameEvent as PlayingShowUIEvent;

        playingUI.SetActive(showUIEvent.show);
    }

    private void OnShowCountDownUI(GameEvent gameEvent)
    {
        CountDownShowUIEvent showUIEvent = gameEvent as CountDownShowUIEvent;
        countDownUI.SetActive(showUIEvent.show);
    }

    private void OnShowFinishUI(GameEvent gameEvent)
    {
        EndStateShowFinishUIEvent showUIEvent = gameEvent as EndStateShowFinishUIEvent;
        finishUI.SetActive(showUIEvent.show);
    }

    private void OnShowTimeoutUI(GameEvent gameEvent)
    {
        EndStateShowTimeoutUIEvent showUIEvent = gameEvent as EndStateShowTimeoutUIEvent;
        timeoutUI.SetActive(showUIEvent.show);
    }

    private void OnPauseStateEnter(GameEvent gameEvent)
    {
        pausePanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    private void OnPauseStateExit(GameEvent gameEvent)
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    private void OnWinStateEnter(GameEvent gameEvent)
    {
        winPanel.SetActive(true);
    }

    private void OnWinStateExit(GameEvent gameEvent)
    {
        winPanel.SetActive(false);
    }

    private void OnGameOverStateEnter(GameEvent gameEvent)
    {
        gameOverPanel.SetActive(true);
    }

    private void OnGameOverStateExit(GameEvent gameEvent)
    {
        gameOverPanel.SetActive(false);
    }

    private void OnSettingsButtonClick(GameEvent gameEvent)
    {
        settingsPanel.SetActive(true);
    }

    private void OnMenuButtonClick(GameEvent gameEvent)
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnExitButtonClick(GameEvent gameEvent)
    {
#if UNITY_WEBGL
        return;
#endif
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void OnJoystickAndKeyboardUse()
    {
        if (currentFirstButton != null)
        {
            EventSystem.current.firstSelectedGameObject = currentFirstButton.gameObject;
            currentFirstButton.Select();
        }
    }
}
