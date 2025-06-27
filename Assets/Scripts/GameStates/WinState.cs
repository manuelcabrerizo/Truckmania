using System;

class WinState : State<GameManager>
{
    public static event Action onWinStateEnter;
    public static event Action onWinSateExit;

    public WinState(GameManager gameManager)
    : base(gameManager) { }

    public override void OnEnter()
    {
        UIManager.onResetButtonClick += OnResetButtonClick;
        onWinStateEnter?.Invoke();
    }

    public override void OnExit()
    {
        onWinSateExit?.Invoke();
        UIManager.onResetButtonClick -= OnResetButtonClick;
    }

    private void OnResetButtonClick()
    {
        owner.SetCountDownState();
    }

}