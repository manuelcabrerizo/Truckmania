using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UICrontrolsMenu: MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        backButton.onClick.AddListener(OnBackButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);
    }

    private void OnDestroy()
    {
        backButton.onClick.RemoveListener(OnBackButtonClick);
        exitButton.onClick.RemoveListener(OnExitButtonClick);
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

}
