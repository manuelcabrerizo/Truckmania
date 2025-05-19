using System;
using UnityEngine;

public class Coin : MonoBehaviour, IPickable
{
    public static event Action<Coin> onCoinSpawn;
    public static event Action<Coin> onCoinPick;

    [SerializeField] private MeshRenderer renderer;
    private Collider collider;
    private AudioSource pickupSound;

    private void Awake()
    {
        collider = GetComponent<Collider>();
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
        renderer.enabled = true;
        collider.enabled = true;
    }

    public void PickUp()
    {
        renderer.enabled = false;
        collider.enabled = false;
        pickupSound.Play();
        onCoinPick?.Invoke(this);
    }
}