using System;
using UnityEngine;

public class PauseState : State<GameManager>
{
    public static event Action onPauseStateEnter;
    public static event Action onPauseSateExit;
    public PauseState(GameManager gameManager) 
        : base(gameManager) { }
    public override void OnEnter()
    {
        UIManager.onResumeButtonClick += OnResumeButtonClick;
        UIManager.onResetButtonClick += OnResetButtonClick;

        Time.timeScale = 0.0f;
        onPauseStateEnter?.Invoke();
    }

    public override void OnExit()
    {
        Time.timeScale = 1.0f;
        onPauseSateExit?.Invoke();

        UIManager.onResumeButtonClick -= OnResumeButtonClick;
        UIManager.onResetButtonClick -= OnResetButtonClick;
    }

    private void OnResumeButtonClick()
    {
        owner.ResumeGame();
    }

    private void OnResetButtonClick()
    {
        owner.ResumeGame();
        owner.SetCountDownState();
    }
}

