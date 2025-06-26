using System;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerDriftState : State<Player>
{
    public PlayerDriftState(Player owner, Func<bool> enterCondition, Func<bool> exitCondition)
        : base(owner, enterCondition, exitCondition) { }

    public override void OnEnter()
    {
        Debug.Log("Drift State OnEnter");
        PlayerData data = owner.Data;
        data.dirtLeft.Play();
        data.dirtRight.Play();
    }

    public override void OnExit()
    {
        Debug.Log("Drift State OnExit");
        PlayerData data = owner.Data;
        data.dirtLeft.Stop();
        data.dirtRight.Stop();
    }


    public override void OnFixedUpdate()
    {
        PlayerData data = owner.Data;
        Transform transform = owner.transform;

        Plane xzPlane = new Plane(transform.up, 0.0f);
        Vector3 forwardVel = xzPlane.ClosestPointOnPlane(data.body.velocity);
        forwardVel.Normalize();

        data.sliptAngle = 0.0f;
        if (data.localVelocity.z >= 0.0f)
        {
            data.sliptAngle = Vector3.Angle(transform.forward, forwardVel);
        }
        else
        {
            data.sliptAngle = Vector3.Angle(-transform.forward, forwardVel);
        }

        float dragCoefficient = data.playerData.driftDragCoefficient;
        float rotVelocity = Mathf.Lerp(data.playerData.rotVelcoity, data.playerData.breakingRotVelocity, data.breaking) * data.playerData.driftRotVelocityMul;

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
