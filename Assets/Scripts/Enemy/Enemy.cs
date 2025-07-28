using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
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
        GameEventManager.Instance.TriggerEvent(EnemySpawnEvent.GetEvent(this));
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
            GameEventManager.Instance.TriggerEvent(EnemyKillEvent.GetEvent());
        }
    }
}
