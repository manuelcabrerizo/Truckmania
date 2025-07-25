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
        GameEventManager.Instance.TriggerEvent(new PauseStateEnterEvent());
        AudioManager.onPauseAll?.Invoke();
    }

    public override void OnExit()
    {
        Time.timeScale = 1.0f;
        GameEventManager.Instance.TriggerEvent(new PauseStateExitEvent());
        GameEventManager.Instance.RemoveListener<ResumeButtonClickEvent>(OnResumeButtonClick);
        GameEventManager.Instance.RemoveListener<ResetButtonClickEvent>(OnResetButtonClick);
        AudioManager.onResumeAll?.Invoke();
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

