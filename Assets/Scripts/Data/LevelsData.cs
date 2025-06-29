using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsData", menuName = "Levels/Data", order = 1)]
public class LevelsData : ScriptableObject
{
    public List<LevelData> levelsData;
}