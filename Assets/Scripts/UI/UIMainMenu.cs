using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button controlsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(OnPlayButtonClick);
        controlsButton.onClick.AddListener(OnControlsButtonClick);
        creditsButton.onClick.AddListener(OnCreditsButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);

        InputManager.onJoystickOrKeyboardUse += OnJoystickAndKeyboardUse;

    }

    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(OnPlayButtonClick);
        controlsButton.onClick.RemoveListener(OnControlsButtonClick);
        creditsButton.onClick.RemoveListener(OnCreditsButtonClick);
        exitButton.onClick.RemoveListener(OnExitButtonClick);

        InputManager.onJoystickOrKeyboardUse -= OnJoystickAndKeyboardUse;

    }

    private void OnPlayButtonClick()
    {
        LevelManager.Instance.LoadFirstLevel();
    }

    private void OnControlsButtonClick()
    {
        SceneManager.LoadScene("Controls");
    }

    private void OnCreditsButtonClick()
    {
        SceneManager.LoadScene("Credits");
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
        EventSystem.current.firstSelectedGameObject = playButton.gameObject;
        playButton.Select();
    }
}
