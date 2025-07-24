using TMPro;
using UnityEngine;

public class UICountDown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    private void Awake()
    {
        CountDownState.onCountDownChange += OnCountDownChange;
    }

    private void OnDestroy()
    {
        CountDownState.onCountDownChange -= OnCountDownChange;
    }

    private void OnCountDownChange(float value)
    {
        timerText.text = value.ToString();
    }
}
