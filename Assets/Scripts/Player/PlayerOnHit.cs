using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnHit : MonoBehaviour
{
    public static event Action<bool> onOnShowResetText;

    [SerializeField] private LayerMask hitMask;
    [SerializeField] private List<Material> materials;

    private CameraMovement cameraMovement;
    private float upsideDownRatio = 1.0f;
    private bool isResettable = false;

    private List<Color> originalColors = new List<Color>();

    private void Awake()
    {
        InputManager.onResetCar += OnResetCar;
        CameraMovement.onCameraCreated += OnCameraCreated;

        foreach (Material mat in materials)
        { 
            originalColors.Add(mat.color);
        }
    }

    private void OnDestroy()
    {
        InputManager.onResetCar -= OnResetCar;
        CameraMovement.onCameraCreated -= OnCameraCreated;

        StopAllCoroutines();

        for (int i = 0; i < materials.Count; ++i)
        {
            materials[i].color = originalColors[i];
        }
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
            StartCoroutine(PlayColorAnimation(Color.white, 0.5f));
            pickable.PickUp();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Utils.CheckCollisionLayer(collision.gameObject, hitMask))
        {
            StartCoroutine(PlayColorAnimation(Color.red, 1.0f));
        }

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
            StopAllCoroutines();
            for (int i = 0; i < materials.Count; ++i)
            {
                materials[i].color = originalColors[i];
            }

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

    private IEnumerator PlayColorAnimation(Color color, float duration)
    {
        float time = 0;
        while (time <= duration)
        {
            for (int i = 0; i < materials.Count; ++i)
            {
                materials[i].color = Color.Lerp(originalColors[i], color, Mathf.Sin(time * 40.0f));
            }
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < materials.Count; ++i)
        {
            materials[i].color = originalColors[i];
        }
    }
}