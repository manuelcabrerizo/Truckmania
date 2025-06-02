using System;
using UnityEngine;

public class Coin : MonoBehaviour, IPickable
{
    public static event Action<Coin> onCoinSpawn;
    public static event Action<Coin> onCoinPick;

    [SerializeField] private MeshRenderer meshRenderer;
    private Collider collision;
    private AudioSource pickupSound;

    private void Awake()
    {
        collision = GetComponent<Collider>();
        pickupSound = GetComponent<AudioSource>();
    }

    private void Start()
    {
        onCoinSpawn?.Invoke(this);
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
        pickupSound.Play();
        onCoinPick?.Invoke(this);
    }
}