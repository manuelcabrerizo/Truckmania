using UnityEngine;

class WinState : State<GameManager>
{
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

        GameEventManager.Instance.TriggerEvent(new CurrentTimeSetEvent("Current Time: ", currentTime));
        if (currentTime < bestTime)
        {
            bestTime = currentTime;
            GameEventManager.Instance.TriggerEvent(new BestTimeSetEvent("New Best Time: ", bestTime));
        }
        else
        {
            GameEventManager.Instance.TriggerEvent(new BestTimeSetEvent("Best Time: ", bestTime));
        }
        PlayerPrefs.SetInt(KeyName, bestTime);

        GameEventManager.Instance.TriggerEvent(new WinStateEnterEvent());
        AudioManager.onPauseAll?.Invoke();
    }

    public override void OnExit()
    {
        GameEventManager.Instance.TriggerEvent(new WinStateExitEvent());

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