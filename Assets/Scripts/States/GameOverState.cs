using System;

class GameOverState : GameState
{
    public static event Action onGameOverStateEnter;
    public static event Action onGameOverSateExit;

    public GameOverState(GameManager gameManager)
        : base(gameManager) { }

    public override void OnEnter()
    {
        UIManager.onResetButtonClick += OnResetButtonClick;
        onGameOverStateEnter?.Invoke();
    }

    public override void OnExit()
    {
        onGameOverSateExit?.Invoke();
        UIManager.onResetButtonClick -= OnResetButtonClick;
    }

    private void OnResetButtonClick()
    {
        gameManager.SetCountDownState();
    }
}