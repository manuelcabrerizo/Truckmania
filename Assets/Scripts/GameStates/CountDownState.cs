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
        GameEventManager.Instance.TriggerEvent(CountDownStateEnterEvent.GetEvent());
        GameEventManager.Instance.TriggerEvent(CountDownShowUIEvent.GetEvent(true));
        GameEventManager.Instance.TriggerEvent(CountDownChangeEvent.GetEvent(timeToWait));

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
        GameEventManager.Instance.TriggerEvent(CountDownShowUIEvent.GetEvent(false));
        GameEventManager.Instance.TriggerEvent(CountDownStateExitEvent.GetEvent());
    }

    public override void OnUpdate()
    {
        if(timer >= 1.0f)
        {
            GameEventManager.Instance.TriggerEvent(PlayAudioClipEvent.GetEvent(owner.Clips.countDown));
            secondCount++;
            GameEventManager.Instance.TriggerEvent(CountDownChangeEvent.GetEvent(timeToWait - secondCount));
            timer -= 1.0f;
        }
        timer += Time.deltaTime;

        if(secondCount == timeToWait)
        {
            owner.SetPlayingState();
        }
    }
}