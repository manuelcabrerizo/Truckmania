using UnityEngine;
using UnityEngine.EventSystems;
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

    private void OnEnable()
    {
        GameEventManager.Instance.AddListener<JoystickOrKeyboardUseEvent>(OnJoystickAndKeyboardUse);
        OnJoystickAndKeyboardUse(null);
    }

    private void OnDisable()
    {
        GameEventManager.Instance.RemoveListener<JoystickOrKeyboardUseEvent>(OnJoystickAndKeyboardUse);
    }

    private void OnExitButtonClick()
    {
        GameEventManager.Instance.TriggerEvent(ExitButtonClickEvent.GetEvent());
    }

    private void OnMenuButtonClick()
    {
        GameEventManager.Instance.TriggerEvent(MenuButtonClickEvent.GetEvent());
    }

    private void OnResetButtonClick()
    {
        GameEventManager.Instance.TriggerEvent(ResetButtonClickEvent.GetEvent());
    }

    private void OnJoystickAndKeyboardUse(GameEvent gameEvent)
    {
        EventSystem.current.firstSelectedGameObject = gameOverResetButton.gameObject;
        gameOverResetButton.Select();
    }
}