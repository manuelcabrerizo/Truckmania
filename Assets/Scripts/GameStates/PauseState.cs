using System;
using UnityEngine;
using UnityEngine.EventSystems;

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

        AudioManager.onPauseAll?.Invoke();
    }

    public override void OnExit()
    {
        Time.timeScale = 1.0f;
        onPauseSateExit?.Invoke();

        UIManager.onResumeButtonClick -= OnResumeButtonClick;
        UIManager.onResetButtonClick -= OnResetButtonClick;
        AudioManager.onResumeAll?.Invoke();
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

