using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/Data", order = 1)]
public class PlayerDataSo : ScriptableObject
{
    public float wheelsRadius = 0.9f;
    public float springTravel = 0.125f;
    public float springConstant = 120.0f;
    public float damperCoefficient = 75;
    public float dragCoefficient = 5.0f;
    public float restLength = 1.0f;
    public AnimationCurve turnCurve;
    public float maxVelocity = 35.0f;
    public float rotVelcoity = 8.0f;
    public float airRotVelocity = 12.0f;
    public float speedForce = 16.0f;
    public float breakFroce = 16.0f;

    public float breakingRotVelocity = 12.0f;
    public float driftAngle = 5.0f;
    public float driftRotVelocityMul = 1.5f;
    public float driftDragCoefficient = 1.125f;
}
