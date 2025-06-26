using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //[SerializeField] private Player player;
    //[SerializeField] private CameraMovement cameraMovement;

    [SerializeField] private PlayStateData playStateData;
    [SerializeField] private AudioSource music;

    private StateMachine fsm;
    private State<int> countDownState;
    private State<int> playingState;
    private State<int> pauseState;
    private State<int> gameOverState;
    private State<int> winState;

    private List<Coin> coins = new List<Coin>();
    private List<Enemy> enemies = new List<Enemy>();

    private void Awake()
    {
        Application.targetFrameRate = 60;

        InputManager.onPause += PauseGame;
        Coin.onCoinSpawn += OnCoinSpawn;
        Enemy.onEnemySpawn += OnEnemySpawn;

        fsm = new StateMachine();
        countDownState = new CountDownState(this);
        playingState = new PlayingState(this, playStateData.roundTime, coins, enemies);
        pauseState = new PauseState(this);
        gameOverState = new GameOverState(this);
        winState = new WinState(this);
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
    }

    private void Update()
    {
        fsm.Update();
    }

    public void SetPlayingState()
    {
        music.Play();
        //playerMovement.SetEnable(true);
        //cameraMovement.SetEnable(true);
        fsm.ChangeState(playingState);
    }

    public void SetCountDownState()
    {
        music.Stop();
        //playerMovement.SetEnable(false);
        //cameraMovement.SetEnable(false);
        fsm.ChangeState(countDownState);
    }

    public void SetGameOverState()
    {
        music.Stop();
        //playerMovement.SetEnable(false);
        //cameraMovement.SetEnable(false);
        fsm.ChangeState(gameOverState);
    }

    public void SetWinState()
    {
        music.Stop();
        //playerMovement.SetEnable(false);
        //cameraMovement.SetEnable(false);
        fsm.ChangeState(winState);
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

}
