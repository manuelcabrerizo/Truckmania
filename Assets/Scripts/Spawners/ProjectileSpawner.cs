using UnityEngine;

public class ProjectileSpawner : Spawner<ProjectileSpawner, Projectile>
{
    [SerializeField] private ExplosiveBarrilProjectile explosiveBarrilPrefab;
    [SerializeField] private ToxicBarrilProjectile toxicBarrilProjectile;
   
    [SerializeField] private int initialBarrilCount = 20;
    
    protected override void OnAwaken()
    {
        PoolManager.Instance.InitPool(explosiveBarrilPrefab, transform, initialBarrilCount);
        PoolManager.Instance.InitPool(toxicBarrilProjectile, transform, initialBarrilCount);
        GameEventManager.Instance.AddListener<ProjectileReleaseEvent>(OnProjectileRelease);
    }

    protected override void OnDestroyed()
    {
        GameEventManager.Instance.RemoveListener<ProjectileReleaseEvent>(OnProjectileRelease);
    }

    private void OnProjectileRelease(GameEvent gameEvent)
    {
        ProjectileReleaseEvent projectileReleaseEvent = (ProjectileReleaseEvent)gameEvent;
        Projectile projectile = projectileReleaseEvent.projectile;

        if (projectile == null)
        {
            return;
        }

        Projectile test = null;
        if (test = projectile as ExplosiveBarrilProjectile)
        {
            PoolManager.Instance?.Release((ExplosiveBarrilProjectile)projectile);
        }
        else if (test = projectile as ToxicBarrilProjectile)
        {
            PoolManager.Instance?.Release((ToxicBarrilProjectile)projectile);
        }
    }
}