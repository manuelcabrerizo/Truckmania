using System;
using System.Threading;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static event Action<CameraMovement> onCameraCreated; 

    [SerializeField] private CameraData cameraData;
    [SerializeField] private PlayerMovement target;
    [SerializeField] private LayerMask enemyMask;
    private GameObject lockTarget = null;

    private Vector3 targetPosition;
    private Vector3 back;
    private bool isEnable = true;
    private bool isLock = false;

    private void Awake()
    {
        InputManager.onLockCamera += OnLockCamera;
        back = -target.transform.forward;
        back.y = 0.0f;
        back.Normalize();
        targetPosition = target.transform.position + (back + Vector3.up * 0.3f) * cameraData.distance;
        transform.position = targetPosition;
        transform.LookAt(targetPosition, Vector3.up);
        isEnable = true;
        isLock = false;
    }

    private void OnDestroy()
    {
        InputManager.onLockCamera -= OnLockCamera;
    }

    private void Start()
    {
        onCameraCreated?.Invoke(this);
    }

    private void FixedUpdate()
    {

        if (isLock == false)
        {
            AlignToTarget();
        }
        else
        {
            LockToTarget();
        }
    }

    public void SetEnable(bool value)
    {
        isEnable = value;
    }

    private void AlignToTarget()
    {
        if (target.IsGrounded())
        {
            back = -target.transform.forward;
            back.Normalize();
        }
        if(!target.IsGrounded() || !isEnable)
        {
            back.y = 0.0f;
            back.Normalize();
        }
        targetPosition = target.transform.position + (back + Vector3.up * cameraData.height) * cameraData.distance;
        Vector3 movement = targetPosition - transform.position;
        transform.position += movement * (cameraData.speed * Time.fixedDeltaTime);
        transform.LookAt(target.transform.position, Vector3.up);
    }

    private void LockToTarget()
    {
        Vector3 toTarget = (lockTarget.transform.position - target.transform.position).normalized;
        back = -toTarget;
        targetPosition = target.transform.position + (back + Vector3.up * cameraData.height) * cameraData.distance * 1.5f;
        Vector3 movement = targetPosition - transform.position;
        transform.position += movement * (cameraData.speed * 1.5f * Time.fixedDeltaTime);
        transform.LookAt(target.transform.position, Vector3.up);
        transform.LookAt(lockTarget.transform.position, Vector3.up);
    }


    private void OnLockCamera()
    {
        Debug.Log("Lock Camera");
        if (isLock == false)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 200.0f, enemyMask);
            if (colliders.Length > 0)
            {
                int minIndex = -1;
                float minDistSq = float.MaxValue;
                for (int i = 0; i < colliders.Length; ++i)
                {
                    float distSq = (transform.position - colliders[i].transform.position).sqrMagnitude;
                    if (distSq < minDistSq)
                    {
                        minDistSq = distSq;
                        minIndex = i;
                    }
                }

                if (minIndex >= 0)
                {
                    lockTarget = colliders[minIndex].gameObject;
                    isLock = true;
                }
            }
        }
        else
        {
            lockTarget = null;
            isLock = false;
        }

    }
}
