public class State : IState
{
    protected GameManager gameManager;

    public State(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public virtual void OnEnter()
    {
    }

    public virtual void OnExit()
    {
    }

    public virtual void OnUpdate()
    {
    }
}

