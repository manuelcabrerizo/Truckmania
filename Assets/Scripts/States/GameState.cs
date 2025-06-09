using UnityEngine;

public class GameState : State
{
    protected GameManager gameManager;
    public GameState(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }
}
