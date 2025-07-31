using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private SoundClipsSO clips;

    private void OnTriggerEnter(Collider other)
    {
        if (Utils.CheckCollisionLayer(other.gameObject, playerMask))
        {
            GameEventManager.Instance.TriggerEvent(WaterHitEnterEvent.GetEvent());
            GameEventManager.Instance.TriggerEvent(PlayAudioClipEvent.GetEvent(clips.water));
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
