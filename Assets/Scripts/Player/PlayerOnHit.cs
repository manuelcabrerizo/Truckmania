using System;
using UnityEngine;

public class PlayerOnHit : MonoBehaviour
{
    public static event Action<bool> onOnShowResetText;

    private float upsideDownRatio = 1.0f;
    private bool isResettable = false;

    private void Awake()
    {
        InputManager.onResetCar += OnResetCar;
    }

    private void OnDestroy()
    {
        InputManager.onResetCar -= OnResetCar;
    }

    private void Update()
    {
        upsideDownRatio = Vector3.Dot(transform.up, Vector3.up);
    }

    private void OnTriggerEnter(Collider other)
    {
        IPickable pickable = null;
        if(other.gameObject.TryGetComponent<IPickable>(out pickable))
        {
            pickable.PickUp();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (upsideDownRatio < 0.25f)
        {
            onOnShowResetText?.Invoke(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        onOnShowResetText?.Invoke(false);
        isResettable = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        isResettable = (upsideDownRatio < 0.25f);
    }

    private void OnResetCar()
    {
        if (isResettable)
        {
            transform.position += Vector3.up * 2.0f;
            transform.rotation = Quaternion.identity;
        }
    }
}