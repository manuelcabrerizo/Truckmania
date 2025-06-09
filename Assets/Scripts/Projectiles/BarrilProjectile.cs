using System.Collections;
using UnityEngine;

public class BarrilProjectile : Projectile
{
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private ParticleSystem explosionParticleSystem;
    [SerializeField] private MeshRenderer barrilRenderer;

    private Rigidbody body;
    private Collider collision;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        collision = GetComponent<Collider>();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public override void OnGet()
    {
        base.OnGet();
        barrilRenderer.enabled = true;
        collision.enabled = true;
        body.isKinematic = false;
        explosionParticleSystem.Stop();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Utils.CheckCollisionLayer(collision.gameObject, playerMask))
        {
            Explote();
        }
    }

    public void Lunch(Vector3 startPosition, Vector3 targetPosition, float timeToTarget)
    {
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

        body.velocity = right * v0x + up * v0y; ;
    }

    private void Explote()
    {
        explosionParticleSystem.Play();
        barrilRenderer.enabled = false;
        collision.enabled = false;
        body.isKinematic = true;
        StartCoroutine(SendReleaseaEventAfterSeconds(explosionParticleSystem.main.duration));
    }

    private IEnumerator SendReleaseaEventAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SendReleaseEvent();
    }

}
