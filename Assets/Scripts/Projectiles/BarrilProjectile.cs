using System;
using System.Collections;
using UnityEngine;

public class BarrilProjectile : Projectile
{
    public static event Action<BarrilProjectile> onBarrilExplote;
    public static event Action<BarrilProjectile> onBarrilExploteEnd;

    [SerializeField] private SoundClipsSO clips;
    [SerializeField] protected ParticleSystem particles;
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

    private void OnCollisionEnter(Collision collision)
    {
        IDamagable damagable = null;
        if (collision.gameObject.TryGetComponent<IDamagable>(out damagable))
        {
            Explote();
            damagable.TakeDamage(1);
        }
    }

    protected virtual void OnAwaken() { }
    protected virtual void OnDestroyed() { }

    public override void OnGet()
    {
        base.OnGet();
        particles.Stop();
        particles.Clear();
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

    public virtual void Explote()
    {
        particles.Play();
        barrilRenderer.enabled = false;
        collision.enabled = false;
        body.isKinematic = true;
        StartCoroutine(SendReleaseaEventAfterSeconds(particles.main.duration));
        onBarrilExplote?.Invoke(this);
        AudioManager.onPlayClip3D?.Invoke(clips.explosion, transform.position, 100, 400);

        Collider[] colliders = Physics.OverlapSphere(body.position, 50.0f);
        if (colliders.Length > 0)
        {
            foreach (Collider coll in colliders)
            {
                IDamagable damagable = null;
                if (coll.gameObject.TryGetComponent<IDamagable>(out damagable))
                {
                    Rigidbody target = null;
                    if (coll.TryGetComponent<Rigidbody>(out target))
                    {
                        target.AddExplosionForce(100, body.position, 75.0f, 75.0f, ForceMode.Impulse);
                    }
                }
            }
        }
    }

    protected IEnumerator SendReleaseaEventAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SendReleaseEvent();
        onBarrilExploteEnd?.Invoke(this);
    }
}
