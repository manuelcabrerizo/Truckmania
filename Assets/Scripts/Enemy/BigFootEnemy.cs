using UnityEngine;

public class BigFootEnemy : Enemy
{
    private float timeToTrowBarril = 2.0f;
    private float timer = 0.0f;
    private Rigidbody playerBody = null;

    private void Awake()
    {
        PlayerMovement.onPlayerCreated += OnPlayerCreated;
    }

    private void OnDestroy()
    {
        PlayerMovement.onPlayerCreated -= OnPlayerCreated;
    }

    public override void OnGet()
    {
        base.OnGet();
        timer = timeToTrowBarril;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            BarrilProjectile projectile = ProjectileSpawner.Instance.Spawn<BarrilProjectile>();
            projectile.Lunch(transform.position, playerBody.position, playerBody.velocity);
            timer = timeToTrowBarril;
        }
    }

    private void OnPlayerCreated(PlayerMovement player)
    { 
        playerBody = player.GetComponent<Rigidbody>();
    }
}
