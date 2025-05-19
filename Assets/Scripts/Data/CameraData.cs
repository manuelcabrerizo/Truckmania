using UnityEngine;

[CreateAssetMenu(fileName = "CameraData", menuName = "Camera/Data", order = 1)]

public class CameraData : ScriptableObject
{
    public float distance = 18;
    public float height = 1.0f;
    public float speed = 6.0f;
}

