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
        PlayingState.onShowPlayingUI += OnShowPlayingUI;
        CountDownState.onShowCountDownUI += OnShowCountDownUI;
        EndState.onShowFinishUI += OnShowFinishUI;
        EndState.onShowTimeoutUI += OnShowTimeoutUI;
        WinState.onWinStateEnter += OnWinStateEnter;
        WinState.onWinSateExit += OnWinStateExit;
        GameOverState.onGameOverStateEnter += OnGameOverStateEnter;
        GameOverState.onGameOverSateExit += OnGameOverStateExit;
        PauseState.onPauseStateEnter += OnPauseStateEnter;
        PauseState.onPauseSateExit += OnPauseStateExit;

        GameEventManager.Instance.AddListener<SettingButtonClickEvent>(OnSettingsButtonClick);
        GameEventManager.Instance.AddListener<MenuButtonClickEvent>(OnMenuButtonClick);
        GameEventManager.Instance.AddListener<ExitButtonClickEvent>(OnExitButtonClick);
    }

    private void OnDestroy()
    {
        InputManager.onJoystickOrKeyboardUse -= OnJoystickAndKeyboardUse;
        PlayingState.onShowPlayingUI -= OnShowPlayingUI;
        CountDownState.onShowCountDownUI -= OnShowCountDownUI;
        EndState.onShowFinishUI -= OnShowFinishUI;
        EndState.onShowTimeoutUI -= OnShowTimeoutUI;
        WinState.onWinStateEnter -= OnWinStateEnter;
        WinState.onWinSateExit -= OnWinStateExit;
        GameOverState.onGameOverStateEnter -= OnGameOverStateEnter;
        GameOverState.onGameOverSateExit -= OnGameOverStateExit;
        PauseState.onPauseStateEnter -= OnPauseStateEnter;
        PauseState.onPauseSateExit -= OnPauseStateExit;

        GameEventManager.Instance.RemoveListener<SettingButtonClickEvent>(OnSettingsButtonClick);
        GameEventManager.Instance.RemoveListener<MenuButtonClickEvent>(OnMenuButtonClick);
        GameEventManager.Instance.RemoveListener<ExitButtonClickEvent>(OnExitButtonClick);
    }

    private void OnShowPlayingUI(bool value)
    {
        playingUI.SetActive(value);
    }

    private void OnShowCountDownUI(bool value)
    {
        countDownUI.SetActive(value);
    }



    private void OnShowFinishUI(bool value)
    { 
        finishUI.SetActive(value);
    }

    private void OnShowTimeoutUI(bool value)
    { 
        timeoutUI.SetActive(value);
    }

    private void OnPauseStateEnter()
    {
        pausePanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    private void OnPauseStateExit()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    private void OnWinStateEnter()
    {
        winPanel.SetActive(true);
    }

    private void OnWinStateExit()
    {
        winPanel.SetActive(false);
    }

    private void OnGameOverStateEnter()
    {
        gameOverPanel.SetActive(true);
    }

    private void OnGameOverStateExit()
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
