using UnityEngine;
public class Utils
{
    public static float AdjustAngle(float angle)
    {
        while(angle >  Mathf.PI) angle -= Mathf.PI*2.0f;
        while(angle < -Mathf.PI) angle += Mathf.PI*2.0f;
        return angle;
    }

    public static float GetOrientationFromVector(float currentOrientation, Vector3 velocity)
    {
        velocity.y = 0.0f;
        if(velocity.magnitude > 0.0f)
        {
            velocity.Normalize();
            float angle = Mathf.Atan2(-velocity.x, velocity.z);
            if(angle < 0.0f)
            {
                angle += Mathf.PI*2.0f;
            }
            return angle;
        }
        else
        {
            return currentOrientation;
        }
    }
}