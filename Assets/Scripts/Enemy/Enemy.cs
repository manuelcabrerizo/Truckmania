using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static event Action<GameObject> onEnemyKill;

    [SerializeField] protected int life;
    [SerializeField] protected LayerMask damagableLayer;

    private void Awake()
    {
        OnAwaken();
    }

    private void OnDestroy()
    {
        OnDestroyed();
    }

    protected virtual void OnAwaken() { }

    protected virtual void OnDestroyed() { }

    private void OnCollisionEnter(Collision collision)
    {
        if (Utils.CheckCollisionLayer(collision.gameObject, damagableLayer))
        {
            life--;
            if (life <= 0)
            {
                onEnemyKill?.Invoke(this.gameObject);
                gameObject.SetActive(false);
            }
        }
    }
}
