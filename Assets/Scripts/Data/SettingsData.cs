using UnityEngine;

[CreateAssetMenu(fileName = "SettingsData", menuName = "Settings/Data", order = 1)]
public class SettingsData : ScriptableObject
{
    public int[] MaxFrameRates;
}
