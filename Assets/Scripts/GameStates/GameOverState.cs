using System;

class GameOverState : State<GameManager>
{
    public GameOverState(GameManager gameManager)
        : base(gameManager) { }

    public override void OnEnter()
    {
        GameEventManager.Instance.AddListener<ResetButtonClickEvent>(OnResetButtonClick);
        GameEventManager.Instance.TriggerEvent(new GameOverStateEnterEvent());
        AudioManager.onPauseAll?.Invoke();
    }

    public override void OnExit()
    {
        GameEventManager.Instance.TriggerEvent(new GameOverStateExitEvent());
        GameEventManager.Instance.RemoveListener<ResetButtonClickEvent>(OnResetButtonClick);
        AudioManager.onResumeAll?.Invoke();
    }

    private void OnResetButtonClick(GameEvent gameEvent)
    {
        owner.SetCountDownState();
    }
}