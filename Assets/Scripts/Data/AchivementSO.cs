using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AchivementSO", menuName = "AchivementSO/Data", order = 1)]
public class AchivementSO : ScriptableObject
{
    public List<Achivement> achivements;
}
