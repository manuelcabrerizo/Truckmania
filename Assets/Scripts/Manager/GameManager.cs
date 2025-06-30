using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   [SerializeField] private SoundClipsSO clips;
    private StateMachine fsm;
    private State<GameManager> countDownState;
    private State<GameManager> playingState;
    private State<GameManager> pauseState;
    private State<GameManager> gameOverState;
    private State<GameManager> winState;
    private State<GameManager> endState;

    private List<Coin> coins = new List<Coin>();
    private List<Enemy> enemies = new List<Enemy>();
    private List<Box> boxes = new List<Box>();

    public int coinsCollectedCount = 0;
    public int enemiesKillCount = 0;
    public int seconds;

    public SoundClipsSO Clips => clips;
    public List<Coin> Coins => coins;
    public List<Enemy> Enemies => enemies;
    public List<Box> Boxes => boxes;


    private void Awake()
    {
        InputManager.onPause += PauseGame;
        Coin.onCoinSpawn += OnCoinSpawn;
        Enemy.onEnemySpawn += OnEnemySpawn;
        Box.onBoxSpawn += OnBoxSpawn;
    }

    private void Start()
    {
        int roundTime = LevelManager.Instance.GetCurrentRoundTime();
        fsm = new StateMachine();
        countDownState = new CountDownState(this);
        playingState = new PlayingState(this, roundTime);
        pauseState = new PauseState(this);
        gameOverState = new GameOverState(this);
        winState = new WinState(this);
        endState = new EndState(this);

        SetCountDownState();
    }

    private void OnDestroy () 
    {
        fsm.Clear();
        InputManager.onPause -= PauseGame;
        Coin.onCoinSpawn -= OnCoinSpawn;
        Enemy.onEnemySpawn -= OnEnemySpawn;
        Box.onBoxSpawn -= OnBoxSpawn;
    }

    private void Update()
    {
        fsm.Update();
    }

    public void SetPlayingState()
    {
        AudioManager.onPlayMusic?.Invoke();
        fsm.ChangeState(playingState);
    }

    public void SetCountDownState()
    {
        AudioManager.onStopMusic?.Invoke();
        fsm.ChangeState(countDownState);
    }

    public void SetGameOverState()
    {
        AudioManager.onStopMusic?.Invoke();
        fsm.ChangeState(gameOverState);
    }

    public void SetWinState()
    {
        AudioManager.onStopMusic?.Invoke();
        fsm.ChangeState(winState);
    }

    public void SetEndState()
    {
        fsm.ChangeState(endState);
    }

    public void PauseGame()
    {
        if (fsm.PeekState() == playingState)
        {
            AudioManager.onPauseMusic?.Invoke();
            fsm.PushState(pauseState);
        }
        else if (fsm.PeekState() == pauseState)
        {
            AudioManager.onPlayMusic?.Invoke();
            fsm.PopState();
        }
    }

    public void ResumeGame()
    {
        AudioManager.onPlayMusic?.Invoke();
        fsm.PopState();
    }

    private void OnCoinSpawn(Coin coin)
    {

        coins.Add(coin);
    }

    private void OnEnemySpawn(Enemy enemy)
    { 
        enemies.Add(enemy);
    }

    private void OnBoxSpawn(Box box)
    {
        boxes.Add(box);
    }

}
