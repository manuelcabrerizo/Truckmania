using System;

class GameOverState : State<GameManager>
{
    public static event Action onGameOverStateEnter;
    public static event Action onGameOverSateExit;

    public GameOverState(GameManager gameManager)
        : base(gameManager) { }

    public override void OnEnter()
    {
        GameEventManager.Instance.AddListener<ResetButtonClickEvent>(OnResetButtonClick);
        onGameOverStateEnter?.Invoke();
        AudioManager.onPauseAll?.Invoke();
    }

    public override void OnExit()
    {
        onGameOverSateExit?.Invoke();
        GameEventManager.Instance.RemoveListener<ResetButtonClickEvent>(OnResetButtonClick);
        AudioManager.onResumeAll?.Invoke();
    }

    private void OnResetButtonClick(GameEvent gameEvent)
    {
        owner.SetCountDownState();
    }
}