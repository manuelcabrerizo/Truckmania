using System;
using UnityEngine;

public class PlayerFallState : State<Player>
{
    public PlayerFallState(Player owner, Func<bool> enterCondition, Func<bool> exitCondition)
    : base(owner, enterCondition, exitCondition) { }

    public override void OnFixedUpdate()
    {
        PlayerData data = owner.Data;
        data.body.AddTorque(data.steer * data.body.transform.up * data.playerData.airRotVelocity, ForceMode.Acceleration);

        Ray groundRay = new Ray(owner.transform.position, -Vector3.up);
        RaycastHit hit;

        Quaternion current = owner.transform.rotation;
        Quaternion target;
        if (Physics.Raycast(groundRay, out hit, 40.0f, data.drivableLayer))
        {
            Vector3 normal = hit.normal;
            Vector3 right = owner.transform.right;
            Vector3 forward = Vector3.Cross(right, normal).normalized;
            target = Quaternion.LookRotation(forward);
        }
        else
        {
            Vector3 forward = owner.transform.forward;
            forward.y = 0;
            forward.Normalize();
            target = Quaternion.LookRotation(forward);
        }
        owner.transform.rotation = Quaternion.Slerp(current, target, Time.deltaTime * 2.0f);
    }
}
