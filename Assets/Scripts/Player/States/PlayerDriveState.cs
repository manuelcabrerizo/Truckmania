using System;
using UnityEngine;

public class PlayerDriveState : State<Player>
{
    public PlayerDriveState(Player owner, Func<bool> enterCondition) 
        : base(owner, enterCondition) { }



    public override void OnExit()
    {
        PlayerData data = owner.Data;
        data.wasDrifting = false;
    }

    public override void OnFixedUpdate()
    {
        ProcessGroundMovement();
    }

    private void ProcessGroundMovement()
    {
        PlayerData data = owner.Data;
        Transform transform = owner.transform;

        float dragCoefficient = data.playerData.dragCoefficient;
        float rotVelocity = Mathf.Lerp(data.playerData.rotVelcoity, data.playerData.breakingRotVelocity, data.breaking);

        data.body.AddTorque(data.steer * data.body.transform.up * rotVelocity * data.playerData.turnCurve.Evaluate(Mathf.Abs(data.velocityRatio)) * Mathf.Sign(data.velocityRatio), ForceMode.Acceleration);
        data.body.AddForce(data.body.transform.forward * data.accel * data.playerData.speedForce, ForceMode.Acceleration);
        if (data.localVelocity.z > 0)
        {
            data.body.AddForce(-data.body.transform.forward * data.breaking * data.playerData.breakFroce, ForceMode.Acceleration);
        }

        float sidewaySpeed = data.localVelocity.x;
        float dragMagnitude = -sidewaySpeed * dragCoefficient;
        Vector3 dragForce = transform.right * dragMagnitude;
        data.body.AddForceAtPosition(dragForce, data.body.worldCenterOfMass, ForceMode.Acceleration);

        Vector3 velocity = data.body.velocity;
        if (velocity.magnitude > data.playerData.maxVelocity)
        {
            velocity = velocity.normalized * data.playerData.maxVelocity;
        }
        data.body.velocity = velocity;
    }

}
