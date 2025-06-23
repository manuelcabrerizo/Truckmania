using System.Collections;
using UnityEngine;

public class BarrilProjectile : Projectile
{
    [SerializeField] protected MeshRenderer barrilRenderer;
    protected Rigidbody body;
    protected Collider collision;

    private void Awake()
    {
        OnAwaken();
        body = GetComponent<Rigidbody>();
        collision = GetComponent<Collider>();
    }

    private void OnDestroy()
    {
        OnDestroyed();
        StopAllCoroutines();
    }

    protected virtual void OnAwaken() { }
    protected virtual void OnDestroyed() { }

    public override void OnGet()
    {
        base.OnGet();
        barrilRenderer.enabled = true;
        collision.enabled = true;
        body.isKinematic = false;
    }

    public void Lunch(Vector3 startPosition, Vector3 targetPosition, float timeToTarget)
    {
        if (body.isKinematic)
        {
            SendReleaseEvent();
            return;
        }

        StartCoroutine(SendReleaseaEventAfterSeconds(20.0f));

        body.position = startPosition;
        body.velocity = Vector3.zero;

        Vector3 relPosition = targetPosition - startPosition;
        
        Vector3 up = Vector3.up;
        Vector3 right = relPosition;
        right.y = 0.0f;
        right.Normalize();

        float t = timeToTarget;
        float x0 = 0.0f;
        float y0 = 0.0f;
        float x = Vector3.Dot(relPosition, right);
        float y = targetPosition.y - startPosition.y;
        float v0x = (x - x0) / t;
        float v0y = (y - y0 - (0.5f * Physics.gravity.y * t * t)) / t;

        body.velocity = right * v0x + up * v0y;
    }

    protected IEnumerator SendReleaseaEventAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SendReleaseEvent();
    }
}
