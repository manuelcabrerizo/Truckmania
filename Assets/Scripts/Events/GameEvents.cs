using UnityEngine;

public abstract class GameEvent {}

public abstract class StaticGameEvent<EventType> : GameEvent where EventType : class, new() 
{
    protected static EventType staticGameEvent = new EventType();

    public static EventType GetEvent()
    {
        return staticGameEvent;
    }
}

// Button UI Events
public class ResumeButtonClickEvent : StaticGameEvent<ResumeButtonClickEvent> { }
public class NextButtonClickEvent : StaticGameEvent<NextButtonClickEvent> { }
public class ResetButtonClickEvent : StaticGameEvent<ResetButtonClickEvent> { }
public class ExitButtonClickEvent : StaticGameEvent<ExitButtonClickEvent> { }
public class MenuButtonClickEvent : StaticGameEvent<MenuButtonClickEvent> { }
public class SettingButtonClickEvent : StaticGameEvent<SettingButtonClickEvent> { }
public class SettingBackButtonClickEvent : StaticGameEvent<SettingBackButtonClickEvent> { }
public class MusicSliderChangeEvent : StaticGameEvent<MusicSliderChangeEvent>
{
    public float value;
    public static MusicSliderChangeEvent GetEvent(float value)
    {
        staticGameEvent.value = value;
        return staticGameEvent;
    }
}
public class SfxSliderChangeEvent : StaticGameEvent<SfxSliderChangeEvent>
{
    public float value;
    public static SfxSliderChangeEvent GetEvent(float value)
    {
        staticGameEvent.value = value;
        return staticGameEvent;
    }
}

// Count Down State Events
public class CountDownStateEnterEvent : StaticGameEvent<CountDownStateEnterEvent> { }
public class CountDownStateExitEvent : StaticGameEvent<CountDownStateExitEvent> { }
public class CountDownShowUIEvent : StaticGameEvent<CountDownShowUIEvent> 
{
    public bool show;
    public static CountDownShowUIEvent GetEvent(bool show)
    {
        staticGameEvent.show = show;
        return staticGameEvent;
    }
}
public class CountDownChangeEvent : StaticGameEvent<CountDownChangeEvent>
{
    public int countDown;
    public static CountDownChangeEvent GetEvent(int countDown)
    {
        staticGameEvent.countDown = countDown;
        return staticGameEvent;
    }
}

// End State Events
public class EndStateEnterEvent : StaticGameEvent<EndStateEnterEvent> { }
public class EndStateShowFinishUIEvent : StaticGameEvent<EndStateShowFinishUIEvent>
{
    public bool show;
    public static EndStateShowFinishUIEvent GetEvent(bool show)
    {
        staticGameEvent.show = show;
        return staticGameEvent;
    }
}
public class EndStateShowTimeoutUIEvent : StaticGameEvent<EndStateShowTimeoutUIEvent> 
{
    public bool show;
    public static EndStateShowTimeoutUIEvent GetEvent(bool show)
    {
        staticGameEvent.show = show;
        return staticGameEvent;
    }
}

// GameOver State Events
public class GameOverStateEnterEvent : StaticGameEvent<GameOverStateEnterEvent> { }
public class GameOverStateExitEvent : StaticGameEvent<GameOverStateExitEvent> { }

// Pause State Events
public class PauseStateEnterEvent : StaticGameEvent<PauseStateEnterEvent> { }
public class PauseStateExitEvent : StaticGameEvent<PauseStateExitEvent> { }

// Playing State Events
public class PlayingShowUIEvent : StaticGameEvent<PlayingShowUIEvent>
{
    public bool show;
    public static PlayingShowUIEvent GetEvent(bool show)
    {
        staticGameEvent.show = show;
        return staticGameEvent;
    }
}

public class UpdateCoinPickTextEvent : StaticGameEvent<UpdateCoinPickTextEvent>
{
    public int coinCount;
    public int coinSpawn;
    public static UpdateCoinPickTextEvent GetEvent(int coinSount, int coinSpawn)
    {
        staticGameEvent.coinCount = coinSount;
        staticGameEvent.coinSpawn = coinSpawn;
        return staticGameEvent;
    }
}

public class UpdateEnemyKillTextEvent : StaticGameEvent<UpdateEnemyKillTextEvent>
{
    public int enemyCount;
    public int enemySpawn;
    public static UpdateEnemyKillTextEvent GetEvent(int enemyCount, int enemySpawn)
    {
        staticGameEvent.enemyCount = enemyCount;
        staticGameEvent.enemySpawn = enemySpawn;
        return staticGameEvent;
    }
}

public class UpdateTimeTextEvent : StaticGameEvent<UpdateTimeTextEvent>
{
    public int seconds;
    public static UpdateTimeTextEvent GetEvent(int seconds)
    {
        staticGameEvent.seconds = seconds;
        return staticGameEvent;
    }
}

public class ShowResetTextEvent : StaticGameEvent<ShowResetTextEvent>
{
    public bool show;
    public static ShowResetTextEvent GetEvent(bool show)
    {
        staticGameEvent.show = show;
        return staticGameEvent;
    }
}

// Win State Events
public class WinStateEnterEvent : StaticGameEvent<WinStateEnterEvent> { }
public class WinStateExitEvent : StaticGameEvent<WinStateExitEvent> { }

public class CurrentTimeSetEvent : StaticGameEvent<CurrentTimeSetEvent>
{
    public string text;
    public int seconds;
    public static CurrentTimeSetEvent GetEvent(string text, int seconds)
    {
        staticGameEvent.text = text;
        staticGameEvent.seconds = seconds;
        return staticGameEvent;
    }
}

public class BestTimeSetEvent : StaticGameEvent<BestTimeSetEvent>
{
    public string text;
    public int seconds;
    public static BestTimeSetEvent GetEvent(string text, int seconds)
    {
        staticGameEvent.text = text;
        staticGameEvent.seconds = seconds;
        return staticGameEvent;
    }
}

// Collectables Events
public class CoinSpawnEvent : StaticGameEvent<CoinSpawnEvent>
{ 
    public Coin coin;
    public static CoinSpawnEvent GetEvent(Coin coin)
    {
        staticGameEvent.coin = coin;
        return staticGameEvent;
    }
}

public class CoinPickEvent : StaticGameEvent<CoinPickEvent> {}

public class BoxSpawnEvent : StaticGameEvent<BoxSpawnEvent>
{ 
    public Box box;
    public static BoxSpawnEvent GetEvent(Box box)
    {
        staticGameEvent.box = box;
        return staticGameEvent;
    }
}

// Enemy Events
public class EnemySpawnEvent : StaticGameEvent<EnemySpawnEvent> 
{
    public Enemy enemy;
    public static EnemySpawnEvent GetEvent(Enemy enemy)
    {
        staticGameEvent.enemy = enemy;
        return staticGameEvent;
    }
}

public class EnemyKillEvent : StaticGameEvent<EnemyKillEvent> {}

// Bigfoot Events
public class BigfootKillEvent : StaticGameEvent<BigfootKillEvent> 
{
    public Enemy enemy;
    public static BigfootKillEvent GetEvent(Enemy enemy)
    {
        staticGameEvent.enemy = enemy;
        return staticGameEvent;
    }
}

// Camera Events
public class CameraCreatedEvent : StaticGameEvent<CameraCreatedEvent>
{
    public CameraController cameraController;
    public static CameraCreatedEvent GetEvent(CameraController cameraController)
    {
        staticGameEvent.cameraController = cameraController;
        return staticGameEvent;
    }
}
public class TargetLockEvent : StaticGameEvent<TargetLockEvent> { };
public class TargetUnlockEvent : StaticGameEvent<TargetUnlockEvent> { };

// Projectiles Events
public class ProjectileReleaseEvent : StaticGameEvent<ProjectileReleaseEvent> 
{
    public Projectile projectile;
    public static ProjectileReleaseEvent GetEvent(Projectile projectile)
    {
        staticGameEvent.projectile = projectile;
        return staticGameEvent;
    }
};

public class ToxicBarrilPickEvent : StaticGameEvent<ToxicBarrilPickEvent>
{
    public ToxicBarrilProjectile barril;
    public static ToxicBarrilPickEvent GetEvent(ToxicBarrilProjectile barril)
    { 
        staticGameEvent.barril = barril;
        return staticGameEvent;
    }
}

// Player Events


public class PlayerCreatedEvent : StaticGameEvent<PlayerCreatedEvent> 
{
    public Player player;

    public static PlayerCreatedEvent GetEvent(Player player)
    {
        staticGameEvent.player = player;
        return staticGameEvent;
    }
}
public class PlayerShootEvent : StaticGameEvent<PlayerShootEvent>
{
    public Player player;

    public static PlayerShootEvent GetEvent(Player player)
    {
        staticGameEvent.player = player;
        return staticGameEvent;
    }
}
public class PlayerRestartEvent : StaticGameEvent<PlayerRestartEvent> 
{
    public Player player;

    public static PlayerRestartEvent GetEvent(Player player)
    {
        staticGameEvent.player = player;
        return staticGameEvent;
    }
}
public class PlayerHitEvent : StaticGameEvent<PlayerHitEvent> { }


public class EndTriggerHitEvent : StaticGameEvent<EndTriggerHitEvent> { }
public class WaterHitEnterEvent : StaticGameEvent<WaterHitEnterEvent> { }
public class WaterHitExitEvent : StaticGameEvent<WaterHitExitEvent> { }


// Input Events
public class PauseEvent : StaticGameEvent<PauseEvent> { }
public class LockCameraEvent : StaticGameEvent<LockCameraEvent> { }
public class WinCheatEvent : StaticGameEvent<WinCheatEvent> { }
public class LoseCheatEvent : StaticGameEvent<LoseCheatEvent> { }
public class GodModeCheatEvent : StaticGameEvent<GodModeCheatEvent> { }
public class JoystickOrKeyboardUseEvent : StaticGameEvent<JoystickOrKeyboardUseEvent> { }
public class JoystickUseEvent : StaticGameEvent<JoystickUseEvent> { }
public class KeyboardUseEvent : StaticGameEvent<KeyboardUseEvent> { }
public class ResetInputEvent : StaticGameEvent<ResetInputEvent> { }

// Audio Events
public class PlayMusicEvent : StaticGameEvent<PlayMusicEvent> { }
public class StopMusicEvent : StaticGameEvent<StopMusicEvent> { }
public class PauseMusicEvent : StaticGameEvent<PauseMusicEvent> { }
public class PlayEngineSoundEvent : StaticGameEvent<PlayEngineSoundEvent> { }
public class PauseEngineSoundEvent : StaticGameEvent<PauseEngineSoundEvent> { }
public class PlayAudioClipEvent : StaticGameEvent<PlayAudioClipEvent>
{ 
    public AudioClip audioClip;
    public static PlayAudioClipEvent GetEvent(AudioClip audioClip)
    {
        staticGameEvent.audioClip = audioClip;
        return staticGameEvent;
    }
}
public class PlayAudioClip3DEvent : StaticGameEvent<PlayAudioClip3DEvent>
{
    public AudioClip audioClip;
    public Vector3 position;
    public float min;
    public float max;
    public static PlayAudioClip3DEvent GetEvent(AudioClip audioClip, Vector3 position, float min, float max)
    {
        staticGameEvent.audioClip = audioClip;
        staticGameEvent.position = position;
        staticGameEvent.min = min;
        staticGameEvent.max = max;
        return staticGameEvent;
    }
}

public class DiscordUpdateStateEvent : StaticGameEvent<DiscordUpdateStateEvent>
{
    public string state;
    public static DiscordUpdateStateEvent GetEvent(string state)
    { 
        staticGameEvent.state = state;
        return staticGameEvent;
    }
}