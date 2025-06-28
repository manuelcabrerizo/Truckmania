using System;
using UnityEngine;

class CountDownState : State<GameManager>
{
    public static event Action onCountDownEnter;
    public static event Action onCountDownExit;

    public static event Action<bool> onShowCountDownUI;
    public static event Action<float> onCountDownChange;

    private float timer = 0;
    private int secondCount = 0;
    private int timeToWait = 3;

    public CountDownState(GameManager gameManager)
        : base(gameManager) { }

    public override void OnEnter()
    {
        onCountDownEnter?.Invoke();
        onShowCountDownUI?.Invoke(true);
        onCountDownChange?.Invoke(timeToWait);
        timer = 0;
        secondCount = 0;
    }

    public override void OnExit()
    {
        timer = 0;
        secondCount = 0;
        onShowCountDownUI?.Invoke(false);
        onCountDownExit?.Invoke();
    }

    public override void OnUpdate()
    {
        if(timer >= 1.0f)
        {
            secondCount++;
            onCountDownChange?.Invoke(timeToWait - secondCount);
            timer -= 1.0f;
        }
        timer += Time.deltaTime;

        if(secondCount == timeToWait)
        {
            owner.SetPlayingState();
        }
    }
}