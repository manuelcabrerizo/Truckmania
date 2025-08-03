using Discord;
using System;
using System.Collections;
using UnityEngine;

public class DiscordPlugin : MonoBehaviour
{
    private static DiscordPlugin instance = null;

    private const long clientId = 1399389665026510948;
    private Discord.Discord discord = null;
    private Discord.Activity activity;
    private float timer = 0;


    private void Awake()
    {
        GameEventManager.Instance.AddListener<DiscordUpdateStateEvent>(OnUpdateState);

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        GameEventManager.Instance.RemoveListener<DiscordUpdateStateEvent>(OnUpdateState);
        StopAllCoroutines();
    }

    private void Start()
    {
        if (discord == null)
        {
            TryToConnect();
        }
    }

    private void Update()
    {
        if (discord == null)
        {
            timer += Time.deltaTime;
            if (timer >= 1.0f)
            {
                TryToConnect();
                timer = 0.0f;
            }
        }
    }

    private void TryToConnect()
    {
        try
        {
            discord = new Discord.Discord(clientId, (UInt64)Discord.CreateFlags.NoRequireDiscord);
            discord.SetLogHook(Discord.LogLevel.Debug, (level, message) =>
            {
                Debug.Log("Log " + level + message);
            });

            activity = new Activity();
            activity.Timestamps.Start = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            activity.Assets.LargeImage = "truckmaniaicon";
            activity.Assets.SmallImage = "truckmaniaicon";

            StartCoroutine(Updater());
        }
        catch (Exception ex)
        {
            discord = null;
        }
    }

    private IEnumerator Updater()
    {
        try
        {
            discord.GetActivityManager().UpdateActivity(activity, result => { });
            discord.RunCallbacks();
        }
        catch (Exception ex)
        {
            discord = null;
            StopAllCoroutines();
        }

        yield return new WaitForEndOfFrame();
        StartCoroutine(Updater());
    }

    private void OnUpdateState(GameEvent gameEvent)
    {
        DiscordUpdateStateEvent e = (DiscordUpdateStateEvent)gameEvent;
        activity.State = e.state;
    }
}
