using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private VolumeData volumeData;

    public static event Action onNextButtonClick;
    public static event Action onResumeButtonClick;
    public static event Action onResetButtonClick;

    public static event Action<float> onMusicSliderChange;
    public static event Action<float> onSfxSliderChange;

    // Playing ui
    [SerializeField] private GameObject playingUI;
    [SerializeField] private TextMeshProUGUI coinCountText;
    [SerializeField] private TextMeshProUGUI enemyCountText;
    [SerializeField] private TextMeshProUGUI pressRToRestartText;
    [SerializeField] private TextMeshProUGUI timeText;

    // CountDown ui
    [SerializeField] private GameObject countDownUI;
    [SerializeField] private TextMeshProUGUI timerText;

    // Finish and Timeout ui
    [SerializeField] private GameObject finishUI;
    [SerializeField] private GameObject timeoutUI;

    // Win ui
    [SerializeField] private GameObject winPanel;
    [SerializeField] private Button winNextButton;
    [SerializeField] private Button winResetButton;
    [SerializeField] private Button winMenuButton;
    [SerializeField] private Button winExitButton;
    [SerializeField] private TextMeshProUGUI winCurrentTimeText;
    [SerializeField] private TextMeshProUGUI winBestTimeText;

    // GameOver ui
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button gameOverResetButton;
    [SerializeField] private Button gameOverMenuButton;
    [SerializeField] private Button gameOverExitButton;

    // Pause ui
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button pauseResumeButton;
    [SerializeField] private Button pauseResetButton;
    [SerializeField] private Button pauseSettingsButton;
    [SerializeField] private Button pauseMenuButton;
    [SerializeField] private Button pauseExitButton;

    // Settings ui
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Slider settingsMusicSlider;
    [SerializeField] private Slider settingsSfxSlider;
    [SerializeField] private Button settingsBackButton;

    private void Awake()
    {
        PlayingState.onShowPlayingUI += OnShowPlayingUI;
        PlayingState.onUpdateCoinPickText += OnUpdateCoinPickText;
        PlayingState.onUpdateEnemyKillText += OnUpdateEnemyKillText;
        PlayingState.onUpdateTimeText += OnUpdateTimeText;
        PlayerRestartState.onOnShowResetText += OnShowResetText;
        CountDownState.onShowCountDownUI += OnShowCountDownUI;
        CountDownState.onCountDownChange += OnCountDownChange;
        EndState.onShowFinishUI += OnShowFinishUI;
        EndState.onShowTimeoutUI += OnShowTimeoutUI;

        WinState.onWinStateEnter += OnWinStateEnter;
        WinState.onWinSateExit += OnWinStateExit;
        WinState.onCurrentTimeSet += OnCurrentTimeSet;
        WinState.onBestTimeSet += OnBestTimeSet;
        winNextButton.onClick.AddListener(OnNextButtonClick);
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
        pauseSettingsButton.onClick.AddListener(OnSettingsButtonClick);
        pauseMenuButton.onClick.AddListener(OnMenuButtonClick);
        pauseExitButton.onClick.AddListener(OnExitButtonClick);

        settingsMusicSlider.onValueChanged.AddListener(OnMusicSliderChange);
        settingsSfxSlider.onValueChanged.AddListener(OnSfxSliderChange);
        settingsBackButton.onClick.AddListener(OnBackButtonClick);
    }

    private void OnDestroy()
    {
        PlayingState.onShowPlayingUI -= OnShowPlayingUI;
        PlayingState.onUpdateCoinPickText -= OnUpdateCoinPickText;
        PlayingState.onUpdateEnemyKillText -= OnUpdateEnemyKillText;
        PlayingState.onUpdateTimeText -= OnUpdateTimeText;
        PlayerRestartState.onOnShowResetText -= OnShowResetText;
        CountDownState.onShowCountDownUI -= OnShowCountDownUI;
        CountDownState.onCountDownChange -= OnCountDownChange;
        EndState.onShowFinishUI -= OnShowFinishUI;
        EndState.onShowTimeoutUI -= OnShowTimeoutUI;

        WinState.onWinStateEnter -= OnWinStateEnter;
        WinState.onWinSateExit -= OnWinStateExit;
        WinState.onCurrentTimeSet -= OnCurrentTimeSet;
        WinState.onBestTimeSet -= OnBestTimeSet;
        winNextButton.onClick.RemoveListener(OnNextButtonClick);
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
        pauseSettingsButton.onClick.RemoveListener(OnSettingsButtonClick);
        pauseMenuButton.onClick.RemoveListener(OnMenuButtonClick);
        pauseExitButton.onClick.RemoveListener(OnExitButtonClick);

        settingsMusicSlider.onValueChanged.RemoveListener(OnMusicSliderChange);
        settingsSfxSlider.onValueChanged.RemoveListener(OnSfxSliderChange);
        settingsBackButton.onClick.RemoveListener(OnBackButtonClick);
    }

    private void Start()
    {
        pressRToRestartText.gameObject.SetActive(false);

        settingsMusicSlider.value = volumeData.Music;
        settingsSfxSlider.value = volumeData.Sfx;
    }

    public void OnUpdateCoinPickText(int coinCount, int coinSpawn)
    {
        coinCountText.text = "You grabbed " + coinCount +" coins of " + coinSpawn;
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
        EventSystem.current.firstSelectedGameObject = pauseResumeButton.gameObject;
        pauseResumeButton.Select();
    }

    private void OnPauseStateExit()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    private void OnWinStateEnter()
    {
        winPanel.SetActive(true);
        EventSystem.current.firstSelectedGameObject = winNextButton.gameObject;
        winNextButton.Select();

    }

    private void OnWinStateExit()
    {
        winPanel.SetActive(false);
    }

    private void OnGameOverStateEnter()
    {
        gameOverPanel.SetActive(true);
        EventSystem.current.firstSelectedGameObject = gameOverResetButton.gameObject;
        gameOverResetButton.Select();
    }

    private void OnGameOverStateExit()
    {
        gameOverPanel.SetActive(false);
    }

    private void OnResumeButtonClick()
    {
        onResumeButtonClick?.Invoke();
    }

    private void OnNextButtonClick()
    {
        onNextButtonClick?.Invoke();
    }

    private void OnResetButtonClick()
    {
        onResetButtonClick?.Invoke();
    }

    private void OnSettingsButtonClick()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(true);
        EventSystem.current.firstSelectedGameObject = settingsMusicSlider.gameObject;
        settingsMusicSlider.Select();
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

    private void OnMusicSliderChange(float value)
    {
        onMusicSliderChange?.Invoke(value);
    }
    private void OnSfxSliderChange(float value)
    {
        onSfxSliderChange?.Invoke(value);
    }

    private void OnBackButtonClick()
    {
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);
        EventSystem.current.firstSelectedGameObject = pauseResumeButton.gameObject;
        pauseResumeButton.Select();
    }

    private void OnCurrentTimeSet(string text, int seconds)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        winCurrentTimeText.text = text + $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
    }
    private void OnBestTimeSet(string text, int seconds)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        winBestTimeText.text = text + $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
    }
}
