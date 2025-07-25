using UnityEngine;

public class Projectile : MonoBehaviour, IPooleable
{
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
        GameEventManager.Instance.TriggerEvent(new ProjectileReleaseEvent(this));
    }
}
