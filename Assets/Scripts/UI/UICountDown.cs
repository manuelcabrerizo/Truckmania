using TMPro;
using UnityEngine;

public class UICountDown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    private void Awake()
    {
        GameEventManager.Instance.AddListener<CountDownChangeEvent>(OnCountDownChange);
    }

    private void OnDestroy()
    {
        GameEventManager.Instance.RemoveListener<CountDownChangeEvent>(OnCountDownChange);
    }

    private void OnCountDownChange(GameEvent gameEvent)
    {
        CountDownChangeEvent changeEvent = gameEvent as CountDownChangeEvent;

        timerText.text = changeEvent.countDown.ToString();
    }
}
