using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviourSingleton<LevelManager>
{
    [SerializeField] private LevelsData levels;

    private int currentLevel = 0;
    public int CurrentLevel => currentLevel;

    protected override void OnAwaken()
    {
        GetCurrentLevelData(out currentLevel);
    }

    public void LoadFirstLevel()
    {
        currentLevel = 0;
        SceneManager.LoadScene(levels.levelsData[0].levelName);
    }

    public void LoadLevelByIndex(int index)
    {
        SceneManager.LoadScene(levels.levelsData[index].levelName);
    }

    public void LoadNextLevel()
    {
        currentLevel = Math.Min(currentLevel + 1, levels.levelsData.Count - 1);
        SceneManager.LoadScene(levels.levelsData[currentLevel].levelName);

    }

    public void LoadPreviousLevel() 
    {
        currentLevel = Math.Max(currentLevel - 1, 0);
        SceneManager.LoadScene(levels.levelsData[currentLevel].levelName);
    }

    public int GetCurrentRoundTime()
    {
        return levels.levelsData[currentLevel].durationIsSeconds;
    }

    public int GetCurrentLevel()
    { 
        return currentLevel;
    }

    private LevelData GetCurrentLevelData(out int index)
    {
        string name = SceneManager.GetActiveScene().name;
        int counter = 0;
        foreach (LevelData data in levels.levelsData)
        {
            if (data.levelName == name)
            {
                index = counter;
                return data;
            }
            counter++;
        }
        LevelData error = new LevelData();
        error.durationIsSeconds = -1;
        index = -1;
        return error;
    }
}
