using UnityEngine;

public class ToxicBarrilSpawner : MonoBehaviour
{
    private float timeToSpawn = 5.0f;
    private float timer = 0.0f;
    private ToxicBarrilProjectile spawnedBarril = null;

    private void Awake()
    {
        ToxicBarrilProjectile.onBarrilPickUp += OnBarrilPickUp;
    }

    private void OnDestroy()
    {
        ToxicBarrilProjectile.onBarrilPickUp -= OnBarrilPickUp;
    }

    private void Update()
    {
        if (spawnedBarril == null)
        {
            timer -= Time.deltaTime;
            if (timer <= 0.0f)
            {
                ToxicBarrilProjectile barril = ProjectileSpawner.Instance.Spawn<ToxicBarrilProjectile>();
                barril.transform.position = transform.position + Vector3.up * 2;
                spawnedBarril = barril;
            }
        }
    }

    private void OnBarrilPickUp(ToxicBarrilProjectile barril)
    {
        if (spawnedBarril == barril)
        {
            spawnedBarril = null;
            timer = timeToSpawn;
        }
    }
}
