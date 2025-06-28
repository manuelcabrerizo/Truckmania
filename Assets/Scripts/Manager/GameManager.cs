using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayStateData playStateData;
    [SerializeField] private AudioSource music;

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

    public List<Coin> Coins => coins;
    public List<Enemy> Enemies => enemies;
    public List<Box> Boxes => boxes;


    private void Awake()
    {
        Application.targetFrameRate = 60;

        InputManager.onPause += PauseGame;
        Coin.onCoinSpawn += OnCoinSpawn;
        Enemy.onEnemySpawn += OnEnemySpawn;
        Box.onBoxSpawn += OnBoxSpawn;

        fsm = new StateMachine();
        countDownState = new CountDownState(this);
        playingState = new PlayingState(this, playStateData.roundTime, coins, enemies, boxes);
        pauseState = new PauseState(this);
        gameOverState = new GameOverState(this);
        winState = new WinState(this);
        endState = new EndState(this);
    }

    private void Start()
    {
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
        music.Play();
        fsm.ChangeState(playingState);
    }

    public void SetCountDownState()
    {
        music.Stop();
        fsm.ChangeState(countDownState);
    }

    public void SetGameOverState()
    {
        music.Stop();
        fsm.ChangeState(gameOverState);
    }

    public void SetWinState()
    {
        music.Stop();
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
            music.Pause();
            fsm.PushState(pauseState);
        }
        else if (fsm.PeekState() == pauseState)
        {
            music.Play();
            fsm.PopState();
        }
    }

    public void ResumeGame()
    {
        music.Play();
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
