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
        GameEventManager.Instance.AddListener<PauseEvent>(PauseGame);
        GameEventManager.Instance.AddListener<CoinSpawnEvent>(OnCoinSpawn);
        GameEventManager.Instance.AddListener<EnemySpawnEvent>(OnEnemySpawn);
        GameEventManager.Instance.AddListener<BoxSpawnEvent>(OnBoxSpawn);
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
        GameEventManager.Instance.RemoveListener<PauseEvent>(PauseGame);
        GameEventManager.Instance.RemoveListener<CoinSpawnEvent>(OnCoinSpawn);
        GameEventManager.Instance.RemoveListener<EnemySpawnEvent>(OnEnemySpawn);
        GameEventManager.Instance.RemoveListener<BoxSpawnEvent>(OnBoxSpawn);
    }

    private void Update()
    {
        // REMOVE !!!!
        GameEventManager.Instance.Update();
        // REMOVE !!!!


        fsm.Update();
    }

    public void SetPlayingState()
    {
        GameEventManager.Instance.TriggerEvent(new PlayMusicEvent());
        fsm.ChangeState(playingState);
    }

    public void SetCountDownState()
    {
        GameEventManager.Instance.TriggerEvent(new StopMusicEvent());
        fsm.ChangeState(countDownState);
    }

    public void SetGameOverState()
    {
        GameEventManager.Instance.TriggerEvent(new StopMusicEvent());
        fsm.ChangeState(gameOverState);
    }

    public void SetWinState()
    {
        GameEventManager.Instance.TriggerEvent(new StopMusicEvent());
        fsm.ChangeState(winState);
    }

    public void SetEndState()
    {
        fsm.ChangeState(endState);
    }

    public void PauseGame(GameEvent gameEvent)
    {
        if (fsm.PeekState() == playingState)
        {
            GameEventManager.Instance.TriggerEvent(new PauseMusicEvent());
            fsm.PushState(pauseState);
        }
        else if (fsm.PeekState() == pauseState)
        {
            GameEventManager.Instance.TriggerEvent(new PlayMusicEvent());
            fsm.PopState();
        }
    }

    public void ResumeGame()
    {
        GameEventManager.Instance.TriggerEvent(new PlayMusicEvent());
        fsm.PopState();
    }

    private void OnCoinSpawn(GameEvent gameEvent)
    {
        CoinSpawnEvent coinSpawnEvent = (CoinSpawnEvent)gameEvent;
        coins.Add(coinSpawnEvent.coin);
    }

    private void OnEnemySpawn(GameEvent gameEvent)
    { 
        EnemySpawnEvent enemySpawnEvent = (EnemySpawnEvent)gameEvent;
        enemies.Add(enemySpawnEvent.enemy);
    }

    private void OnBoxSpawn(GameEvent gameEvent)
    {
        BoxSpawnEvent boxSpawnEvent = (BoxSpawnEvent)gameEvent;
        boxes.Add(boxSpawnEvent.box);
    }

}
