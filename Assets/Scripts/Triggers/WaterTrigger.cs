using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask playerMask;

    private void OnTriggerEnter(Collider other)
    {
        if (Utils.CheckCollisionLayer(other.gameObject, playerMask))
        {
            GameEventManager.Instance.TriggerEvent(WaterHitEnterEvent.GetEvent());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Utils.CheckCollisionLayer(other.gameObject, playerMask))
        {
            GameEventManager.Instance.TriggerEvent(WaterHitExitEvent.GetEvent());
        }
    }
}
