using UnityEngine;
public class Utils
{
    static public bool CheckCollisionLayer(GameObject gameObject, LayerMask layer)
    {
        return ((1 << gameObject.layer) & layer.value) > 0;
    }

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
    public static float LinearToDecibel(float linear)
    {
        float dB;
        if (linear != 0)
            dB = 20.0f * Mathf.Log10(linear);
        else
            dB = -144.0f;
        return dB;
    }

    public static float DecibelToLinear(float dB)
    {
        float linear = Mathf.Pow(10.0f, dB / 20.0f);
        return linear;
    }
}