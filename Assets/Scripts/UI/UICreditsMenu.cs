using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UICreditsMenu : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        backButton.onClick.AddListener(OnBackButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);
        InputManager.onJoystickOrKeyboardUse += OnJoystickAndKeyboardUse;

    }

    private void OnDestroy()
    {
        backButton.onClick.RemoveListener(OnBackButtonClick);
        exitButton.onClick.RemoveListener(OnExitButtonClick);
        InputManager.onJoystickOrKeyboardUse -= OnJoystickAndKeyboardUse;
    }

    private void OnBackButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnExitButtonClick()
    {
#if UNITY_WEBGL
        return;
#endif
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void OnJoystickAndKeyboardUse()
    {
        EventSystem.current.firstSelectedGameObject = backButton.gameObject;
        backButton.Select();
    }

}
