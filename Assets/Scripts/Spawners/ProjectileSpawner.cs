using UnityEngine;
using UnityEngine.Assertions;

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
        if (projectile == null)
        {
            return;
        }

        Projectile test = null;
        if (test = projectile as ExplosiveBarrilProjectile)
        {
            Assert.IsNotNull(test);
            Assert.IsNotNull(projectile);
            Assert.IsNotNull(PoolManager.Instance);
            PoolManager.Instance.Release((ExplosiveBarrilProjectile)projectile);
        }
        else if (test = projectile as ToxicBarrilProjectile)
        {
            Assert.IsNotNull(test);
            Assert.IsNotNull(projectile);
            Assert.IsNotNull(PoolManager.Instance);
            PoolManager.Instance.Release((ToxicBarrilProjectile)projectile);
        }
    }
}