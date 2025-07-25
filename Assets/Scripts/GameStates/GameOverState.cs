class GameOverState : State<GameManager>
{
    public GameOverState(GameManager gameManager)
        : base(gameManager) { }

    public override void OnEnter()
    {
        GameEventManager.Instance.AddListener<ResetButtonClickEvent>(OnResetButtonClick);
        GameEventManager.Instance.TriggerEvent(new GameOverStateEnterEvent());
        GameEventManager.Instance.TriggerEvent(new PauseAllSoundEvent());
    }

    public override void OnExit()
    {
        GameEventManager.Instance.TriggerEvent(new GameOverStateExitEvent());
        GameEventManager.Instance.RemoveListener<ResetButtonClickEvent>(OnResetButtonClick);
        GameEventManager.Instance.TriggerEvent(new ResumeAllSoundEvent());
    }

    private void OnResetButtonClick(GameEvent gameEvent)
    {
        owner.SetCountDownState();
    }
}