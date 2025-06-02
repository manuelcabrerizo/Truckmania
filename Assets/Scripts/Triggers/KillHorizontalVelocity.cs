using UnityEngine;

public class KillHorizontalVelocity : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody body;
        if (other.TryGetComponent<Rigidbody>(out body))
        {
            body.velocity = Vector3.up * Vector3.Dot(body.velocity, Vector3.up);
        }
    }
}
