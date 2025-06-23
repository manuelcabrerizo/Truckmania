using System;
using UnityEngine;

public class Enemy : MonoBehaviour
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

    private void OnCollisionEnter(Collision collision)
    {
        OnTakeDamage(collision.gameObject);
    }

    protected virtual void OnAwaken() { }

    protected virtual void OnStart() { }

    protected virtual void OnDestroyed() { }

    protected virtual void OnTakeDamage(GameObject attacker) { }

    public virtual void Restart()
    {
        life = maxLife;
    }

    public void SendEnemyKillEvent()
    {
        onEnemyKill?.Invoke(this);
    }
}
