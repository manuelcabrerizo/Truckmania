using System;
using UnityEngine;

public class PlayerOnHit : MonoBehaviour
{
    public static event Action<bool> onOnShowResetText;

    private CameraMovement cameraMovement;
    private float upsideDownRatio = 1.0f;
    private bool isResettable = false;

    private void Awake()
    {
        InputManager.onResetCar += OnResetCar;
        CameraMovement.onCameraCreated += OnCameraCreated;
    }

    private void OnDestroy()
    {
        InputManager.onResetCar -= OnResetCar;
        CameraMovement.onCameraCreated -= OnCameraCreated;
    }

    private void Update()
    {
        upsideDownRatio = Vector3.Dot(transform.up, Vector3.up);
    }

    private void OnTriggerEnter(Collider other)
    {
        IPickable pickable = null;
        if (other.gameObject.TryGetComponent<IPickable>(out pickable))
        {
            pickable.PickUp();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (upsideDownRatio < 0.25f)
        {
            onOnShowResetText?.Invoke(true);
            isResettable = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        onOnShowResetText?.Invoke(false);
        isResettable = false;
    }

    private void OnResetCar()
    {
        if (isResettable)
        {
            transform.position += Vector3.up * 2.0f;
            Vector3 forward = cameraMovement.transform.forward;
            forward.y = 0f;
            forward.Normalize();
            transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
        }
    }

    private void OnCameraCreated(CameraMovement cameraMovement)
    { 
        this.cameraMovement = cameraMovement;
    } 
}