using System;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    public static event Action onEndTriggerHit;

    [SerializeField] private LayerMask playerMask;

    private void OnTriggerEnter(Collider other)
    {
        if (Utils.CheckCollisionLayer(other.gameObject, playerMask))
        {
            onEndTriggerHit?.Invoke();
        }
    }
}
