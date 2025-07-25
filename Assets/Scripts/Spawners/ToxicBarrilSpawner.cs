using UnityEngine;

public class ToxicBarrilSpawner : MonoBehaviour
{
    [SerializeField] private GameObject vfx;
    private float timeToSpawn = 5.0f;
    private float timer = 0.0f;
    private ToxicBarrilProjectile spawnedBarril = null;

    private void Awake()
    {
        GameEventManager.Instance.AddListener<ToxicBarrilPickEvent>(OnBarrilPickUp);
    }

    private void OnDestroy()
    {
        GameEventManager.Instance.RemoveListener<ToxicBarrilPickEvent>(OnBarrilPickUp);
        if (spawnedBarril)
        {
            spawnedBarril.SendReleaseEvent();
        }
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
                vfx.SetActive(true);
            }
        }
    }

    private void OnBarrilPickUp(GameEvent gameEvent)
    {
        ToxicBarrilPickEvent pickEvent = (ToxicBarrilPickEvent)gameEvent;
        ToxicBarrilProjectile barril = pickEvent.barril;
        if (spawnedBarril == barril)
        {
            spawnedBarril = null;
            timer = timeToSpawn;
            vfx.SetActive(false);
        }
    }
}
