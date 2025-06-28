using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    public static event Action<Enemy> onEnemySpawn;
    public static event Action<Enemy> onEnemyKill;

    [SerializeField] protected int maxLife;
    [SerializeField] protected LayerMask damagableLayer;

    protected int life;

    private void Awake()
    {
        life = maxLife;
        OnAwaken();
    }

    private void OnDestroy()
    {
        OnDestroyed();
    }

    private void Start()
    {
        onEnemySpawn?.Invoke(this);
        OnStart();
    }

    protected virtual void OnAwaken() { }

    protected virtual void OnStart() { }

    protected virtual void OnDestroyed() { }

    public virtual void Restart()
    {
        life = maxLife;
    }

    public virtual void TakeDamage(int amount)
    {
        life = Mathf.Max(life - amount, 0);
        if (life == 0)
        {
            onEnemyKill?.Invoke(this);
        }
    }
}
