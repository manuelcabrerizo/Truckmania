using System;
using UnityEngine;

public class Coin : MonoBehaviour, IPickable
{
    [SerializeField] private SoundClipsSO clips;
    [SerializeField] private MeshRenderer meshRenderer;
    private Collider collision;

    private void Awake()
    {
        collision = GetComponent<Collider>();
    }

    private void Start()
    {
        GameEventManager.Instance.TriggerEvent(new CoinSpawnEvent(this));
        transform.Rotate(0.0f, UnityEngine.Random.Range(0.0f, 360.0f), 0.0f);
    }
    private void Update()
    {
        transform.Rotate(0.0f, Time.deltaTime * 200.0f, 0.0f);
    }

    public void Restart()
    {
        meshRenderer.enabled = true;
        collision.enabled = true;
    }

    public void PickUp()
    {
        meshRenderer.enabled = false;
        collision.enabled = false;
        AudioManager.onPlayClip?.Invoke(clips.coin);
        GameEventManager.Instance.TriggerEvent(new CoinPickEvent());
    }
}