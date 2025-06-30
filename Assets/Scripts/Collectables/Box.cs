using System;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private SoundClipsSO clips;
    [SerializeField] private LayerMask playerLayer;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (Utils.CheckCollisionLayer(collision.gameObject, playerLayer))
        {
            AudioManager.onPlayClip?.Invoke(clips.box);
        }
    }


}
