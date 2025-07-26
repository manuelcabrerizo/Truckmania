using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private CameraData cameraData;
    [SerializeField] private LayerMask collisionMask;

    private Player target;
    private Vector3 back;
    private bool isEnable;

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
        AlignToTarget();
    }

    private void OnPlayerCreated(GameEvent gameEvent)
    {
        PlayerCreatedEvent playerCreatedEvent = (PlayerCreatedEvent)gameEvent;
        target = playerCreatedEvent.player;

        isEnable = true;
        back = -target.transform.forward;
        back.y = 0.0f;
        back.Normalize();
        transform.position = target.transform.position + (back + Vector3.up * 0.3f) * cameraData.distance;
        transform.LookAt(target.transform.position, Vector3.up);
    }

    private void AlignToTarget()
    {
        if (target.Data.isGrounded)
        {
            back = -target.transform.forward;
            back.Normalize();
        }
        if (!target.Data.isGrounded)
        {
            back.y = 0.0f;
            back.Normalize();
        }

        Vector3 toCamera = (back + Vector3.up * cameraData.height) * cameraData.distance;
        Vector3 targetPosition = target.transform.position + toCamera;

        RaycastHit hit;
        if (Physics.SphereCast(target.transform.position, 0.2f, toCamera.normalized, out hit, toCamera.magnitude, collisionMask))
        {
            targetPosition = target.transform.position + toCamera.normalized * hit.distance;
        }

        Vector3 movement = targetPosition - transform.position;
        transform.position += movement * (cameraData.speed * Time.fixedDeltaTime);
        transform.LookAt(target.transform.position, Vector3.up);
    }

    public void Restart()
    {
        isEnable = true;
        back = -target.transform.forward;
        back.Normalize();
        Vector3 toCamera = (back + Vector3.up * cameraData.height) * cameraData.distance;
        Vector3 targetPosition = target.transform.position + toCamera;
        transform.position = targetPosition;
    }

    public void SetEnable(bool value)
    {
        isEnable = value;
    }
}