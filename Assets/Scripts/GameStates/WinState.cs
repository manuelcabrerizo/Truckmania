using System;
using UnityEngine;

class WinState : State<GameManager>
{
    public static event Action onWinStateEnter;
    public static event Action onWinSateExit;
    public static event Action<string, int> onCurrentTimeSet;
    public static event Action<string, int> onBestTimeSet;


    public WinState(GameManager gameManager)
    : base(gameManager) { }

    public override void OnEnter()
    {
        GameEventManager.Instance.AddListener<NextButtonClickEvent>(OnNextButtonClick);
        GameEventManager.Instance.AddListener<ResetButtonClickEvent>(OnResetButtonClick);

        int roundTime = LevelManager.Instance.GetCurrentRoundTime();
        int levelIndex = LevelManager.Instance.GetCurrentLevel();

        int currentTime = roundTime - owner.seconds;
        int bestTime = currentTime;
        string KeyName = "BestTime" + levelIndex;
        if (PlayerPrefs.HasKey(KeyName))
        {
            bestTime = PlayerPrefs.GetInt(KeyName);
        }

        onCurrentTimeSet?.Invoke("Current Time: ", currentTime);
        if (currentTime < bestTime)
        {
            bestTime = currentTime;
            onBestTimeSet?.Invoke("New Best Time: ", bestTime);
        }
        else
        {
            onBestTimeSet?.Invoke("Best Time: ", bestTime);
        }
        PlayerPrefs.SetInt(KeyName, bestTime);

        onWinStateEnter?.Invoke();

        AudioManager.onPauseAll?.Invoke();
    }

    public override void OnExit()
    {
        onWinSateExit?.Invoke();
        GameEventManager.Instance.RemoveListener<NextButtonClickEvent>(OnNextButtonClick);
        GameEventManager.Instance.RemoveListener<ResetButtonClickEvent>(OnResetButtonClick);
        AudioManager.onResumeAll?.Invoke();
    }

    private void OnNextButtonClick(GameEvent gameEvent)
    { 
        LevelManager.Instance.LoadNextLevel();
    }

    private void OnResetButtonClick(GameEvent gameEvent)
    {
        owner.SetCountDownState();
    }

}