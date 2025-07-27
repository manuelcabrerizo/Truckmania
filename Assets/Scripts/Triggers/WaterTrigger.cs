using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask playerMask;

    private void OnTriggerEnter(Collider other)
    {
        if (Utils.CheckCollisionLayer(other.gameObject, playerMask))
        {
            GameEventManager.Instance.TriggerEvent(new WaterHitEnterEvent());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Utils.CheckCollisionLayer(other.gameObject, playerMask))
        {
            GameEventManager.Instance.TriggerEvent(new WaterHitExitEvent());
        }
    }
}
