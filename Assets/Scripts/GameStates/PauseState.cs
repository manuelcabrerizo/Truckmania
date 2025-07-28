using UnityEngine;

public class PauseState : State<GameManager>
{
    public PauseState(GameManager gameManager) 
        : base(gameManager) { }
    public override void OnEnter()
    {
        GameEventManager.Instance.AddListener<ResumeButtonClickEvent>(OnResumeButtonClick);
        GameEventManager.Instance.AddListener<ResetButtonClickEvent>(OnResetButtonClick);
        Time.timeScale = 0.0f;
        GameEventManager.Instance.TriggerEvent(PauseStateEnterEvent.GetEvent());
        GameEventManager.Instance.TriggerEvent(PauseAllSoundEvent.GetEvent());
    }

    public override void OnExit()
    {
        Time.timeScale = 1.0f;
        GameEventManager.Instance.TriggerEvent(PauseStateExitEvent.GetEvent());
        GameEventManager.Instance.TriggerEvent(ResumeAllSoundEvent.GetEvent());

        GameEventManager.Instance.RemoveListener<ResumeButtonClickEvent>(OnResumeButtonClick);
        GameEventManager.Instance.RemoveListener<ResetButtonClickEvent>(OnResetButtonClick);
    }

    private void OnResumeButtonClick(GameEvent gameEvent)
    {
        owner.ResumeGame();
    }

    private void OnResetButtonClick(GameEvent gameEvent)
    {
        owner.ResumeGame();
        owner.SetCountDownState();
    }
}

