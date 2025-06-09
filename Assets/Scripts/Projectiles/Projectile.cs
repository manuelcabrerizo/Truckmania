using System;
using UnityEngine;

public class Projectile : MonoBehaviour, IPooleable
{
    public static event Action<Projectile> onProjectileRelease;

    public virtual void OnGet()
    {
        gameObject.SetActive(true);
    }

    public virtual void OnRelease()
    {
        gameObject.SetActive(false);
    }

    public void SendReleaseEvent()
    {
        onProjectileRelease?.Invoke(this);
    }
}
