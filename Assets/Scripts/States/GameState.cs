using UnityEngine;

// TODO: remove this and use a State<GameManager>
public class GameState : State<int>
{
    protected GameManager gameManager;
    public GameState(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }
}
