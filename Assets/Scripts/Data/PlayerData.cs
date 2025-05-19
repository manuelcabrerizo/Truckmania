using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/Data", order = 1)]
public class PlayerData : ScriptableObject
{
    public float wheelsRadius = 0.9f;
    public float springTravel = 0.125f;
    public float springConstant = 120.0f;
    public float damperCoefficient = 75;
    public float dragCoefficient = 3.0f;
    public float restLength = 1.0f;
    public AnimationCurve turnCurve;
    public float maxVelocity = 35.0f;
    public float rotVelcoity = 12.0f;
    public float speedForce = 16.0f;
    public float breakFroce = 16.0f;
}
