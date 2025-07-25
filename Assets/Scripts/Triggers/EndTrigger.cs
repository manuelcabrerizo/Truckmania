using System;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask playerMask;

    private void OnTriggerEnter(Collider other)
    {
        if (Utils.CheckCollisionLayer(other.gameObject, playerMask))
        {
            GameEventManager.Instance.TriggerEvent(new EndTriggerHitEvent());
        }
    }
}
