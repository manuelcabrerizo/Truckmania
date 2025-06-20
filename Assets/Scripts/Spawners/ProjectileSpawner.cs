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

        Projectile.onProjectileRelease += OnProjectileRelease;
    }

    protected override void OnDestroyed()
    {
        Projectile.onProjectileRelease -= OnProjectileRelease;
    }

    private void OnProjectileRelease(Projectile projectile)
    {
        Projectile test = null;
        if (test = projectile as ExplosiveBarrilProjectile)
        {
            PoolManager.Instance.Release((ExplosiveBarrilProjectile)projectile);
        }
        else if (test = projectile as ToxicBarrilProjectile)
        {
            PoolManager.Instance.Release((ToxicBarrilProjectile)projectile);
        }
    }
}