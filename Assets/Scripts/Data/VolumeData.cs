using UnityEngine;

[CreateAssetMenu(fileName = "VolumeData", menuName = "Volume/Data", order = 1)]

public class VolumeData : ScriptableObject
{
    public float Master = 0;
    public float Music = 0;
    public float Sfx = 0;
}
