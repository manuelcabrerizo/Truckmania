using UnityEngine;

public class ProjectileSpawner : Spawner<ProjectileSpawner, Projectile>
{
    [SerializeField] private BarrilProjectile barrilPrefab;
    [SerializeField] private int initialBarrilCount = 20;
    
    protected override void OnAwaken()
    {
        PoolManager.Instance.InitPool(barrilPrefab, transform, initialBarrilCount);
        Projectile.onProjectileRelease += OnProjectileRelease;
    }

    protected override void OnDestroyed()
    {
        Projectile.onProjectileRelease -= OnProjectileRelease;
    }

    private void OnProjectileRelease(Projectile projectile)
    {
        Projectile test = null;
        if (test = projectile as BarrilProjectile)
        {
            PoolManager.Instance.Release((BarrilProjectile)projectile);
        }
    }
}