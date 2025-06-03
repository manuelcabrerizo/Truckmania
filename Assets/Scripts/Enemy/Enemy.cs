using UnityEngine;

public class Enemy : MonoBehaviour, IPooleable
{
    public virtual void OnGet()
    {
        gameObject.SetActive(true);
    }

    public virtual void OnRelease()
    {
        gameObject.SetActive(false);
    }
}
