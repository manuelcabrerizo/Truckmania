using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static event Action onResumeButtonClick;
    public static event Action onResetButtonClick;
    // Playing ui
    [SerializeField] private GameObject playingUI;
    [SerializeField] private TextMeshProUGUI coinCountText;
    [SerializeField] private TextMeshProUGUI pressRToRestartText;
    [SerializeField] private TextMeshProUGUI timeText;

    // CountDown ui
    [SerializeField] private GameObject countDownUI;
    [SerializeField] private TextMeshProUGUI timerText;

    // Win ui
    [SerializeField] private GameObject winPanel;
    [SerializeField] private Button winResetButton;
    [SerializeField] private Button winMenuButton;
    [SerializeField] private Button winExitButton;

    // GameOver ui
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button gameOverResetButton;
    [SerializeField] private Button gameOverMenuButton;
    [SerializeField] private Button gameOverExitButton;

    // Pause ui
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button pauseResumeButton;
    [SerializeField] private Button pauseResetButton;
    [SerializeField] private Button pauseMenuButton;
    [SerializeField] private Button pauseExitButton;

    private void Awake()
    {
        PlayingState.onShowPlayingUI += OnShowPlayingUI;
        PlayingState.onUpdateCoinPickText += OnUpdateCoinPickText;
        PlayingState.onUpdateTimeText += OnUpdateTimeText;
        PlayerOnHit.onOnShowResetText += OnShowResetText;
        CountDownState.onShowCountDownUI += OnShowCountDownUI;
        CountDownState.onCountDownChange += OnCountDownChange;

        WinState.onWinStateEnter += OnWinStateEnter;
        WinState.onWinSateExit += OnWinStateExit;
        winResetButton.onClick.AddListener(OnResetButtonClick);
        winMenuButton.onClick.AddListener(OnMenuButtonClick);
        winExitButton.onClick.AddListener(OnExitButtonClick);

        GameOverState.onGameOverStateEnter += OnGameOverStateEnter;
        GameOverState.onGameOverSateExit += OnGameOverStateExit;
        gameOverResetButton.onClick.AddListener(OnResetButtonClick);
        gameOverMenuButton.onClick.AddListener(OnMenuButtonClick);
        gameOverExitButton.onClick.AddListener(OnExitButtonClick);
        
        PauseState.onPauseStateEnter += OnPauseStateEnter;
        PauseState.onPauseSateExit += OnPauseStateExit;
        pauseResumeButton.onClick.AddListener(OnResumeButtonClick);
        pauseResetButton.onClick.AddListener(OnResetButtonClick);
        pauseMenuButton.onClick.AddListener(OnMenuButtonClick);
        pauseExitButton.onClick.AddListener(OnExitButtonClick);
    }

    private void OnDestroy()
    {
        PlayingState.onShowPlayingUI -= OnShowPlayingUI;
        PlayingState.onUpdateCoinPickText -= OnUpdateCoinPickText;
        PlayingState.onUpdateTimeText -= OnUpdateTimeText;
        PlayerOnHit.onOnShowResetText -= OnShowResetText;
        CountDownState.onShowCountDownUI -= OnShowCountDownUI;
        CountDownState.onCountDownChange -= OnCountDownChange;

        WinState.onWinStateEnter -= OnWinStateEnter;
        WinState.onWinSateExit -= OnWinStateExit;
        winResetButton.onClick.RemoveListener(OnResetButtonClick);
        winMenuButton.onClick.RemoveListener(OnMenuButtonClick);
        winExitButton.onClick.RemoveListener(OnExitButtonClick);

        GameOverState.onGameOverStateEnter -= OnGameOverStateEnter;
        GameOverState.onGameOverSateExit -= OnGameOverStateExit;
        gameOverResetButton.onClick.RemoveListener(OnResetButtonClick);
        gameOverMenuButton.onClick.RemoveListener(OnMenuButtonClick);
        gameOverExitButton.onClick.RemoveListener(OnExitButtonClick);

        PauseState.onPauseStateEnter -= OnPauseStateEnter;
        PauseState.onPauseSateExit -= OnPauseStateExit;
        pauseResumeButton.onClick.RemoveListener(OnResumeButtonClick);
        pauseResetButton.onClick.RemoveListener(OnResetButtonClick);
        pauseMenuButton.onClick.RemoveListener(OnMenuButtonClick);
        pauseExitButton.onClick.RemoveListener(OnExitButtonClick);
    }

    private void Start()
    {
        pressRToRestartText.gameObject.SetActive(false);
    }

    public void OnUpdateCoinPickText(int coinCount, int coinSpawn)
    {
        coinCountText.text = "Coin Coun: " + coinCount + " | " + coinSpawn;
        coinCountText.text = "You grabbed " + coinCount +" coins of " + coinSpawn;
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

    private void OnShowPlayingUI(bool value)
    {
        playingUI.SetActive(value);
    }

    private void OnShowCountDownUI(bool value)
    {
        countDownUI.SetActive(value);
    }

    private void OnCountDownChange(float value)
    {
        timerText.text = value.ToString();
    }

    private void OnPauseStateEnter()
    {
        pausePanel.SetActive(true);
    }

    private void OnPauseStateExit()
    {
        pausePanel.SetActive(false);
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

    private void OnResumeButtonClick()
    {
        onResumeButtonClick?.Invoke();
    }

    private void OnResetButtonClick()
    {
        onResetButtonClick?.Invoke();
    }

    private void OnMenuButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
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
}
