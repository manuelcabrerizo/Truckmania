using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootScreenController : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(GoToMainMenuAfterSeconds(2.0f));
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    protected IEnumerator GoToMainMenuAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("MainMenu");
    }
}