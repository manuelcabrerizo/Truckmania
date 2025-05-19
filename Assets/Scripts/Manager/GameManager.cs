using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayStateData playStateData;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private CameraMovement cameraMovement;
    [SerializeField] private AudioSource music;

    private StateMachine fsm;
    private State countDownState;
    private State playingState;
    private State pauseState;
    private State gameOverState;
    private State winState;

    private List<Coin> coins = new List<Coin>();

    private void Awake()
    {
        Application.targetFrameRate = 60;

        InputManager.onPause += PauseGame;
        Coin.onCoinSpawn += OnCoinSpawn;

        fsm = new StateMachine();
        countDownState = new CountDownState(this);
        playingState = new PlayingState(this, playStateData.roundTime, coins);
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
    }

    private void Update()
    {
        fsm.Update();
    }

    public void SetPlayingState()
    {
        music.Play();
        playerMovement.SetEnable(true);
        cameraMovement.SetEnable(true);
        fsm.ChangeState(playingState);
    }

    public void SetCountDownState()
    {
        music.Stop();
        playerMovement.SetEnable(false);
        cameraMovement.SetEnable(false);
        fsm.ChangeState(countDownState);
    }

    public void SetGameOverState()
    {
        music.Stop();
        playerMovement.SetEnable(false);
        cameraMovement.SetEnable(false);
        fsm.ChangeState(gameOverState);
    }

    public void SetWinState()
    {
        music.Stop();
        playerMovement.SetEnable(false);
        cameraMovement.SetEnable(false);
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

}
