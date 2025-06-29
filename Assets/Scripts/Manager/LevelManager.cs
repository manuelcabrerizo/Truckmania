using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviourSingleton<LevelManager>
{
    [SerializeField] private LevelsData data;

    private int currentLevel = 0;
    public int CurrentLevel => currentLevel;

    protected override void OnAwaken()
    {
        currentLevel = 0;
    }

    public void LoadFirstLevel()
    {
        SceneManager.LoadScene(data.levelsData[0].levelName);
    }

    public void LoadLevelByIndex(int index)
    {
        SceneManager.LoadScene(data.levelsData[index].levelName);
    }

    public void LoadNextLevel()
    {
        currentLevel = Math.Min(currentLevel + 1, data.levelsData.Count - 1);
        SceneManager.LoadScene(data.levelsData[currentLevel].levelName);

    }

    public void LoadPreviousLevel() 
    {
        currentLevel = Math.Max(currentLevel - 1, 0);
        SceneManager.LoadScene(data.levelsData[currentLevel].levelName);
    }

    public int GetCurrentRoundTime()
    {
        return data.levelsData[currentLevel].durationIsSeconds;
    }

    public int GetCurrentLevel()
    { 
        return currentLevel;
    }
}
