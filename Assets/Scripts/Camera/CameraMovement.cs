using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private CameraData cameraData;
    [SerializeField] private PlayerMovement target;

    private Vector3 targetPosition;
    private Vector3 back;
    private bool isEnable = true;
    

    private void Awake()
    {
        back = -target.transform.forward;
        back.y = 0.0f;
        back.Normalize();
        targetPosition = target.transform.position + (back + Vector3.up * 0.3f) * cameraData.distance;
        transform.position = targetPosition;
        transform.LookAt(targetPosition, Vector3.up);
        isEnable = true;
    }

    private void FixedUpdate()
    {
        AlignToTarget();
    }

    public void SetEnable(bool value)
    {
        isEnable = value;
    }

    private void AlignToTarget()
    {
        if (target.IsGrounded() || !isEnable)
        {
            back = -target.transform.forward;
            back.y = 0.0f;
            back.Normalize();
        }
        targetPosition = target.transform.position + (back + Vector3.up * cameraData.height) * cameraData.distance;
        Vector3 movement = targetPosition - transform.position;
        transform.position += movement * (cameraData.speed * Time.fixedDeltaTime);
        transform.LookAt(target.transform.position, Vector3.up);
    }
}
