using UnityEngine;
using UnityEngine.EventSystems;

public class UIOnEnter : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    [SerializeField] private SoundClipsSO clips;

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameEventManager.Instance.TriggerEvent(PlayAudioClipEvent.GetEvent(clips.select));
    }

    public void OnSelect(BaseEventData eventData)
    {
        GameEventManager.Instance.TriggerEvent(PlayAudioClipEvent.GetEvent(clips.select));
    }
}
