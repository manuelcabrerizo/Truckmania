class GameOverState : State<GameManager>
{
    public GameOverState(GameManager gameManager)
        : base(gameManager) { }

    public override void OnEnter()
    {
        GameEventManager.Instance.AddListener<ResetButtonClickEvent>(OnResetButtonClick);
        GameEventManager.Instance.TriggerEvent(GameOverStateEnterEvent.GetEvent());
        GameEventManager.Instance.TriggerEvent(PauseAllSoundEvent.GetEvent());
    }

    public override void OnExit()
    {
        GameEventManager.Instance.TriggerEvent(GameOverStateExitEvent.GetEvent());
        GameEventManager.Instance.RemoveListener<ResetButtonClickEvent>(OnResetButtonClick);
        GameEventManager.Instance.TriggerEvent(ResumeAllSoundEvent.GetEvent());
    }

    private void OnResetButtonClick(GameEvent gameEvent)
    {
        owner.SetCountDownState();
    }
}