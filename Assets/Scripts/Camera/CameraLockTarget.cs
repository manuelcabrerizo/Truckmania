using UnityEngine;

public class CameraLockTarget : MonoBehaviour
{
    [SerializeField] private CameraData cameraData;
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private LayerMask enemyMask;

    private Player target;
    private bool isEnable;
    private GameObject lockTarget = null;

    private void Awake()
    {
        isEnable = false;
        GameEventManager.Instance.AddListener<PlayerCreatedEvent>(OnPlayerCreated);
    }

    private void OnDestroy()
    {
        GameEventManager.Instance.RemoveListener<PlayerCreatedEvent>(OnPlayerCreated);
    }

    private void FixedUpdate()
    {
        if (!isEnable)
        {
            return;
        }

        LockToTarget();
    }


    private void OnPlayerCreated(GameEvent gameEvent)
    {
        PlayerCreatedEvent playerCreatedEvent = (PlayerCreatedEvent)gameEvent;
        target = playerCreatedEvent.player;
        isEnable = true;
    }

    private void LockToTarget()
    {
        if (lockTarget == null) return;

        Vector3 toTarget = -(lockTarget.transform.position - target.transform.position).normalized;
        Vector3 toCamera = (toTarget + Vector3.up * cameraData.height) * cameraData.distance * 1.5f;
        Vector3 targetPosition = target.transform.position + toCamera;

        RaycastHit hit;
        if (Physics.SphereCast(target.transform.position, 0.2f, toCamera.normalized, out hit, toCamera.magnitude, collisionMask))
        {
            targetPosition = target.transform.position + toCamera.normalized * hit.distance;
        }

        Vector3 movement = targetPosition - transform.position;
        transform.position += movement * (cameraData.speed * 1.5f * Time.fixedDeltaTime);
        transform.LookAt(lockTarget.transform.position, Vector3.up);
    }

    public bool TryToLock()
    {
        Vector3 position = target.transform.position;
        Collider[] colliders = Physics.OverlapSphere(position, 200.0f, enemyMask);
        if (colliders.Length > 0)
        {
            int minIndex = -1;
            float minDistSq = float.MaxValue;
            for (int i = 0; i < colliders.Length; ++i)
            {
                float distSq = (position - colliders[i].transform.position).sqrMagnitude;
                if (distSq < minDistSq)
                {
                    minDistSq = distSq;
                    minIndex = i;
                }
            }
            if (minIndex >= 0)
            {
                transform.position = target.transform.position;
                lockTarget = colliders[minIndex].gameObject;
                return true;
            }
        }
        return false;
    }

    public void Unlock()
    {
        lockTarget = null;
    }

    public bool TargetLost(GameObject target)
    {
        if (lockTarget == target)
        {
            Unlock();
            return true;
        }
        return false;
    }

    public void Restart()
    {
        isEnable = true;
        Unlock();
    }

    public void SetEnable(bool value)
    { 
        isEnable = value;
    }
}
