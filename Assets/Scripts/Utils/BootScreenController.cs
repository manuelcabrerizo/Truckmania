using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootScreenController : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(GoToMainMenuAfterSeconds(2.0f));
    }

    protected IEnumerator GoToMainMenuAfterSeconds(float seconds)
    {
        ConfigurationManager.LoadConfigurations();
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("MainMenu");
    }
}