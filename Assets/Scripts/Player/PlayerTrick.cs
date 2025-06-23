using System;
using UnityEngine;

public class PlayerTrick : MonoBehaviour
{
    public static event Action<string> onTickDone;

    private PlayerMovement player;
    private Rigidbody body;

    private bool wasGrounded = false;

    private Vector3 trickAcc = Vector3.zero;

    private void Awake()
    {
        player = GetComponent<PlayerMovement>();
        body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float roll = transform.rotation.eulerAngles.z;
        float pitch = transform.rotation.eulerAngles.y;
        float yaw = transform.rotation.eulerAngles.x;

        Vector3 localAngularVelocity = transform.InverseTransformDirection(body.angularVelocity);
        
        if (player.IsGrounded() && !wasGrounded)
        {
            trickAcc = Vector3.zero;
        }
        
        if (wasGrounded && !player.IsGrounded())
        {
            trickAcc = Vector3.zero;
        }

        if (!player.IsGrounded())
        {
            trickAcc += localAngularVelocity;

            if (trickAcc.x >= 270.0f)
            {
                onTickDone?.Invoke("Front Flip");
                trickAcc.x = 0.0f;
            }
            else if (trickAcc.x <= -270.0f)
            {
                onTickDone?.Invoke("Back Flip");
                trickAcc.x = 0.0f;
            }

            if (trickAcc.y >= 270.0f || trickAcc.y <= -270.0f)
            {
                onTickDone?.Invoke("360");
                trickAcc.y = 0.0f;
            }

            if (trickAcc.z >= 270.0f || trickAcc.z <= -270.0f)
            {
                onTickDone?.Invoke("Side Flip");
                trickAcc.z = 0.0f;
            }
        }

        wasGrounded = player.IsGrounded();
    }

}
