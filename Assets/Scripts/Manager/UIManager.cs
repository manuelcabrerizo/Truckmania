using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private GameObject achivementPanel;
    [SerializeField] private UIAchivement uiAchivement;

    private void Awake()
    {
        GameEventManager.Instance.AddListener<ShowAchivementUnlockUIEvent>(OnShowAchivementUI);
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
        GameEventManager.Instance.AddListener<SettingBackButtonClickEvent>(OnSettingsBackButtonClick);
        GameEventManager.Instance.AddListener<MenuButtonClickEvent>(OnMenuButtonClick);
        GameEventManager.Instance.AddListener<ExitButtonClickEvent>(OnExitButtonClick);
    }

    private void OnDestroy()
    {
        GameEventManager.Instance.RemoveListener<ShowAchivementUnlockUIEvent>(OnShowAchivementUI);
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
        GameEventManager.Instance.RemoveListener<SettingBackButtonClickEvent>(OnSettingsBackButtonClick);
        GameEventManager.Instance.RemoveListener<MenuButtonClickEvent>(OnMenuButtonClick);
        GameEventManager.Instance.RemoveListener<ExitButtonClickEvent>(OnExitButtonClick);
    }

    private void OnShowAchivementUI(GameEvent gameEvent)
    {
        ShowAchivementUnlockUIEvent e = (ShowAchivementUnlockUIEvent)gameEvent;
        uiAchivement.SetAchivement(e.achivement);
        StopAllCoroutines();
        StartCoroutine(StartAchivementPanelAnimation());
    }

    private void OnShowPlayingUI(GameEvent gameEvent)
    {
        PlayingShowUIEvent showUIEvent = (PlayingShowUIEvent)gameEvent;
        playingUI.SetActive(showUIEvent.show);
    }

    private void OnShowCountDownUI(GameEvent gameEvent)
    {
        CountDownShowUIEvent showUIEvent = (CountDownShowUIEvent)gameEvent;
        countDownUI.SetActive(showUIEvent.show);
    }

    private void OnShowFinishUI(GameEvent gameEvent)
    {
        EndStateShowFinishUIEvent showUIEvent = (EndStateShowFinishUIEvent)gameEvent;
        finishUI.SetActive(showUIEvent.show);
    }

    private void OnShowTimeoutUI(GameEvent gameEvent)
    {
        EndStateShowTimeoutUIEvent showUIEvent = (EndStateShowTimeoutUIEvent)gameEvent;
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
        pausePanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    private void OnSettingsBackButtonClick(GameEvent @event)
    {
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);
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

    private IEnumerator StartAchivementPanelAnimation()
    {
        float speed = 200.0f;
        achivementPanel.SetActive(true);
        Vector3 startPosition = uiAchivement.transform.position;
        Vector3 targetPosition = startPosition;
        targetPosition.y -= 90.0f;

        while (uiAchivement.transform.position.y > targetPosition.y)
        {
            Vector3 newPosition = uiAchivement.transform.position;
            newPosition.y -= speed * Time.unscaledDeltaTime;
            uiAchivement.transform.position = newPosition;
            yield return new WaitForEndOfFrame();
        }
        uiAchivement.transform.position = targetPosition;
        
        yield return new WaitForSecondsRealtime(7.5f);
        
        while (uiAchivement.transform.position.y < startPosition.y)
        {
            Vector3 newPosition = uiAchivement.transform.position;
            newPosition.y += speed * Time.unscaledDeltaTime;
            uiAchivement.transform.position = newPosition;
            yield return new WaitForEndOfFrame();
        }
        uiAchivement.transform.position = startPosition;

        achivementPanel.SetActive(false);
    }
}
