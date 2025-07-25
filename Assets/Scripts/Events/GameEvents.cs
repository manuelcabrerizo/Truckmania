using UnityEngine;

public abstract class GameEvent
{
    protected float timeStamp;
    public GameEvent()
    {
        this.timeStamp = Time.time;
    }
}

// Button Click Events
public class ResumeButtonClickEvent : GameEvent { }
public class NextButtonClickEvent : GameEvent { }
public class ResetButtonClickEvent : GameEvent { }
public class ExitButtonClickEvent : GameEvent { }
public class MenuButtonClickEvent : GameEvent { }
public class SettingButtonClickEvent : GameEvent { }

// Count Down State Events
public class CountDownStateEnterEvent : GameEvent { }
public class CountDownStateExitEvent : GameEvent { }
public class CountDownShowUIEvent : GameEvent 
{
    public bool show;
    public CountDownShowUIEvent(bool show)
    {
        this.show = show;
    }
}
public class CountDownChangeEvent : GameEvent
{
    public int countDown;
    public CountDownChangeEvent(int countDown)
    {
        this.countDown = countDown;
    }
}

// End State Events
public class EndStateEnterEvent : GameEvent { }
public class EndStateShowFinishUIEvent : GameEvent 
{
    public bool show;
    public EndStateShowFinishUIEvent(bool show)
    { 
        this.show = show;
    }
}
public class EndStateShowTimeoutUIEvent : GameEvent 
{
    public bool show;
    public EndStateShowTimeoutUIEvent(bool show)
    {
        this.show = show;
    }
}

// GameOver State Events
public class GameOverStateEnterEvent : GameEvent { }
public class GameOverStateExitEvent : GameEvent { }

// Pause State Events
public class PauseStateEnterEvent : GameEvent { }
public class PauseStateExitEvent : GameEvent { }

// Playing State Events
public class PlayingShowUIEvent : GameEvent
{
    public bool show;
    public PlayingShowUIEvent(bool show)
    { 
        this.show = show;
    }
}

public class UpdateCoinPickTextEvent : GameEvent
{
    public int coinCount;
    public int coinSpawn;
    public UpdateCoinPickTextEvent(int coinCount, int coinSpawn)
    {
        this.coinCount = coinCount;
        this.coinSpawn = coinSpawn;
    }
}

public class UpdateEnemyKillTextEvent : GameEvent
{
    public int enemyCount;
    public int enemySpawn;
    public UpdateEnemyKillTextEvent(int enemyCount, int enemySpawn)
    {
        this.enemyCount = enemyCount;
        this.enemySpawn = enemySpawn;
    }
}

public class UpdateTimeTextEvent : GameEvent
{
    public int seconds;
    public UpdateTimeTextEvent(int seconds)
    {
        this.seconds = seconds;
    }
}

// Win State Events
public class WinStateEnterEvent : GameEvent { }
public class WinStateExitEvent : GameEvent { }

public class CurrentTimeSetEvent : GameEvent
{
    public string text;
    public int seconds;
    public CurrentTimeSetEvent(string text, int seconds)
    {
        this.text = text;
        this.seconds = seconds;
    }
}

public class BestTimeSetEvent : GameEvent
{
    public string text;
    public int seconds;
    public BestTimeSetEvent(string text, int seconds)
    {
        this.text = text;
        this.seconds = seconds;
    }
}

// Collectables Events
public class CoinSpawnEvent : GameEvent
{ 
    public Coin coin;
    public CoinSpawnEvent(Coin coin) 
    {
        this.coin = coin;
    }
}

public class CoinPickEvent : GameEvent {}

public class BoxSpawnEvent : GameEvent
{ 
    public Box box;
    public BoxSpawnEvent(Box box) 
    {
        this.box = box;
    }
}

// Enemy Events
public class EnemySpawnEvent : GameEvent 
{
    public Enemy enemy;
    public EnemySpawnEvent(Enemy enemy)
    {
        this.enemy = enemy;
    }
}

public class EnemyKillEvent : GameEvent {}

// Bigfoot Events
public class BigfootKillEvent : GameEvent 
{
    public Enemy enemy;
    public BigfootKillEvent(Enemy enemy)
    {
        this.enemy = enemy;
    }
}

// Camera Events
public class CameraCreatedEvent : GameEvent
{
    public CameraMovement cameraMovement;
    public CameraCreatedEvent(CameraMovement cameraMovement)
    {
        this.cameraMovement = cameraMovement;
    }
}
public class TargetLockEvent : GameEvent { };
public class TargetUnlockEvent : GameEvent { };

// Projectiles Events
public class ProjectileReleaseEvent : GameEvent 
{
    public Projectile projectile;
    public ProjectileReleaseEvent(Projectile projectile)
    {
        this.projectile = projectile;
    }
};

public class ToxicBarrilPickEvent : GameEvent
{
    public ToxicBarrilProjectile barril;
    public ToxicBarrilPickEvent(ToxicBarrilProjectile barril)
    {
        this.barril = barril;
    }
}

// Player Events
public abstract class PlayerEvent : GameEvent
{ 
    public Player player;
    public PlayerEvent(Player player)
    {
        this.player = player;
    }
}

public class PlayerCreatedEvent : PlayerEvent
{
    public PlayerCreatedEvent(Player player) 
        : base(player) { }
}
public class PlayerHitEvent : GameEvent { }
public class PlayerShootEvent : PlayerEvent
{
    public PlayerShootEvent(Player player) 
        : base(player) { }
}
public class PlayerRestartEvent : PlayerEvent
{
    public PlayerRestartEvent(Player player)
        : base(player) { }
}