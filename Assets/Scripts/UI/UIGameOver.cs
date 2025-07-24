using UnityEngine;
using UnityEngine.UI;

public class UIGameOver : MonoBehaviour
{
    [SerializeField] private Button gameOverResetButton;
    [SerializeField] private Button gameOverMenuButton;
    [SerializeField] private Button gameOverExitButton;

    // Start is called before the first frame update
    void Awake()
    {
        gameOverResetButton.onClick.AddListener(OnResetButtonClick);
        gameOverMenuButton.onClick.AddListener(OnMenuButtonClick);
        gameOverExitButton.onClick.AddListener(OnExitButtonClick);
    }

    // Update is called once per frame
    void OnDestroy()
    {
        gameOverResetButton.onClick.RemoveListener(OnResetButtonClick);
        gameOverMenuButton.onClick.RemoveListener(OnMenuButtonClick);
        gameOverExitButton.onClick.RemoveListener(OnExitButtonClick);
    }

    private void OnExitButtonClick()
    {
        GameEventManager.Instance.TriggerEvent(new ExitButtonClickEvent());
    }

    private void OnMenuButtonClick()
    {
        GameEventManager.Instance.TriggerEvent(new MenuButtonClickEvent());
    }

    private void OnResetButtonClick()
    {
        GameEventManager.Instance.TriggerEvent(new ResetButtonClickEvent());
    }
}
