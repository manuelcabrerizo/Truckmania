using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static event Action<CameraMovement> onCameraCreated;
    public static event Action onTargetLock;
    public static event Action onTargetUnlock;


    [SerializeField] private CameraData cameraData;
    [SerializeField] private Player target;
    [SerializeField] private LayerMask enemyMask;
    private GameObject lockTarget = null;

    private Vector3 back;
    private bool isEnable = true;
    private bool isLock = false;

    private float lockTransitionTime = 0.5f;
    private float lockTimer = 0.0f;
    private float lockT = 0.0f;

    private void Awake()
    {
        InputManager.onLockCamera += OnLockCamera;
        Enemy.onEnemyKill += OnEnemyKill;

        back = -target.transform.forward;
        back.y = 0.0f;
        back.Normalize();
        transform.position = target.transform.position + (back + Vector3.up * 0.3f) * cameraData.distance;
        transform.LookAt(target.transform.position, Vector3.up);
        isEnable = true;
        isLock = false;
    }

    private void OnDestroy()
    {
        InputManager.onLockCamera -= OnLockCamera;
        Enemy.onEnemyKill -= OnEnemyKill;
    }

    private void Start()
    {
        onCameraCreated?.Invoke(this);
    }

    private void Update()
    {
        if (isLock)
        {
            if (lockTimer <= lockTransitionTime)
            {
                lockTimer += Time.deltaTime;
                lockT = Mathf.Min(lockTimer / lockTransitionTime, 1.0f);
            }
        }
        else
        {
            if (lockTimer >= 0.0f)
            {
                lockTimer -= Time.deltaTime;
                lockT = Mathf.Max(lockTimer / lockTransitionTime, 0.0f);
            }
        }
    }

    private void FixedUpdate()
    {
        if (isLock == false)
        {
            if (lockT > 0.0f && lockTarget != null)
            {
                Transition(lockT);
            }
            else
            {
                if (lockTarget)
                {
                    lockTarget = null;
                }
                AlignToTarget();
            }
        }
        else
        {
            if (lockT < 1.0f)
            {
                Transition(lockT);
            }
            else
            {
                LockToTarget();
            }
        }
    }

    public void SetEnable(bool value)
    {
        isEnable = value;
    }

    private void AlignToTarget()
    {
        if (target.Data.isGrounded)
        {
            back = -target.transform.forward;
            back.Normalize();
        }
        if (!target.Data.isGrounded || !isEnable)
        {
            back.y = 0.0f;
            back.Normalize();
        }

        Vector3 toCamera = (back + Vector3.up * cameraData.height) * cameraData.distance;
        Vector3 targetPosition = target.transform.position + toCamera;

        RaycastHit hit;
        if (Physics.SphereCast(target.transform.position, 0.2f, toCamera.normalized, out hit, toCamera.magnitude))
        {
            targetPosition = target.transform.position + toCamera.normalized * hit.distance;
        }

        Vector3 movement = targetPosition - transform.position;
        transform.position += movement * (cameraData.speed * Time.fixedDeltaTime);
        transform.LookAt(target.transform.position, Vector3.up);
    }

    private void LockToTarget()
    {
        Vector3 toTarget = -(lockTarget.transform.position - target.transform.position).normalized;
        Vector3 toCamera = (toTarget + Vector3.up * cameraData.height) * cameraData.distance * 1.5f;
        Vector3 targetPosition = target.transform.position + toCamera;

        RaycastHit hit;
        if (Physics.SphereCast(target.transform.position, 0.2f, toCamera.normalized, out hit, toCamera.magnitude))
        {
            targetPosition = target.transform.position + toCamera.normalized * hit.distance;
        }

        Vector3 movement = targetPosition - transform.position;
        transform.position += movement * (cameraData.speed * 1.5f * Time.fixedDeltaTime);
        transform.LookAt(lockTarget.transform.position, Vector3.up);
    }

    private void Transition(float t)
    {
        if (target.Data.isGrounded)
        {
            back = -target.transform.forward;
            back.Normalize();
        }
        if (!target.Data.isGrounded || !isEnable)
        {
            back.y = 0.0f;
            back.Normalize();
        }

        Vector3 toCamera0 = (back + Vector3.up * cameraData.height) * cameraData.distance;
        Vector3 targetPosition0 = target.transform.position + toCamera0;

        Vector3 toTarget = -(lockTarget.transform.position - target.transform.position).normalized;
        Vector3 toCamera1 = (toTarget + Vector3.up * cameraData.height) * cameraData.distance * 1.5f;
        Vector3 targetPosition1 = target.transform.position + toCamera1;

        Vector3 targetPosition = Vector3.Lerp(targetPosition0, targetPosition1, t);
        Vector3 movement = targetPosition - transform.position;
        transform.position += movement * (cameraData.speed * 1.5f * Time.fixedDeltaTime);

        Vector3 lookAtPosition0 = target.transform.position;
        Vector3 lookAtPosition1 = lockTarget.transform.position;
        Vector3 lookAtPosition = Vector3.Lerp(lookAtPosition0, lookAtPosition1, t*t*t);
        transform.LookAt(lookAtPosition, Vector3.up);
    }

    private void OnLockCamera()
    {
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
                    lockTimer = 0.0f;
                    onTargetLock?.Invoke();

                }
            }
        }
        else
        {
            isLock = false;
            onTargetUnlock?.Invoke();
        }

    }

    private void LockTargetLost(GameObject lockTarget)
    {
        if (this.lockTarget == lockTarget)
        {
            this.lockTarget = null;
            isLock = false;
            onTargetUnlock?.Invoke();
        }
    }

    private void OnEnemyKill(Enemy enemy)
    {
        LockTargetLost(enemy.gameObject);
    }
}
