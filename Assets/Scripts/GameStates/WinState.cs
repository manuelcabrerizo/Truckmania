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

        GameEventManager.Instance.TriggerEvent(WinStateEnterEvent.GetEvent());
        GameEventManager.Instance.TriggerEvent(PauseEngineSoundEvent.GetEvent());

        GameEventManager.Instance.TriggerEvent(CurrentTimeSetEvent.GetEvent("Current Time: ", currentTime));
        if (currentTime < bestTime)
        {
            bestTime = currentTime;
            GameEventManager.Instance.TriggerEvent(BestTimeSetEvent.GetEvent("New Best Time: ", bestTime));
        }
        else
        {
            GameEventManager.Instance.TriggerEvent(BestTimeSetEvent.GetEvent("Best Time: ", bestTime));
        }
        PlayerPrefs.SetInt(KeyName, bestTime);


    }

    public override void OnExit()
    {
        GameEventManager.Instance.TriggerEvent(WinStateExitEvent.GetEvent());
        GameEventManager.Instance.TriggerEvent(PlayEngineSoundEvent.GetEvent());

        GameEventManager.Instance.RemoveListener<NextButtonClickEvent>(OnNextButtonClick);
        GameEventManager.Instance.RemoveListener<ResetButtonClickEvent>(OnResetButtonClick);
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