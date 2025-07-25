using UnityEngine;

class CountDownState : State<GameManager>
{
    private float timer = 0;
    private int secondCount = 0;
    private int timeToWait = 3;

    public CountDownState(GameManager gameManager)
        : base(gameManager) { }

    public override void OnEnter()
    {
        GameEventManager.Instance.TriggerEvent(new CountDownStateEnterEvent());
        GameEventManager.Instance.TriggerEvent(new CountDownShowUIEvent(true));
        GameEventManager.Instance.TriggerEvent(new CountDownChangeEvent(timeToWait));

        timer = 0;
        secondCount = 0;

        foreach (Coin coin in owner.Coins)
        {
            coin.Restart();
        }

        foreach (Enemy enemy in owner.Enemies)
        {
            enemy.Restart();
        }

        foreach (Box box in owner.Boxes)
        {
            box.Restart();
        }
    }

    public override void OnExit()
    {
        timer = 0;
        secondCount = 0;
        GameEventManager.Instance.TriggerEvent(new CountDownShowUIEvent(false));
        GameEventManager.Instance.TriggerEvent(new CountDownStateExitEvent());
    }

    public override void OnUpdate()
    {
        if(timer >= 1.0f)
        {
            GameEventManager.Instance.TriggerEvent(new PlayAudioClipEvent(owner.Clips.countDown));
            secondCount++;
            GameEventManager.Instance.TriggerEvent(new CountDownChangeEvent(timeToWait - secondCount));
            timer -= 1.0f;
        }
        timer += Time.deltaTime;

        if(secondCount == timeToWait)
        {
            owner.SetPlayingState();
        }
    }
}