using System;
using UnityEngine;

public class Box : MonoBehaviour
{
    public static event Action<Box> onBoxSpawn;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Rigidbody body;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        startPosition = transform.position;
        startRotation = transform.rotation;
        onBoxSpawn?.Invoke(this);
    }

    public void Restart()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
    }


}
