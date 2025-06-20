using UnityEngine;

public class ExplosiveBarrilProjectile : BarrilProjectile
{
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private ParticleSystem explosionParticleSystem;

    public override void OnGet()
    {
        base.OnGet();
        explosionParticleSystem.Stop();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Utils.CheckCollisionLayer(collision.gameObject, playerMask))
        {
            Explote();
        }
    }

    private void Explote()
    {
        explosionParticleSystem.Play();
        barrilRenderer.enabled = false;
        collision.enabled = false;
        body.isKinematic = true;

        Collider[] colliders = Physics.OverlapSphere(body.position, 50.0f, playerMask);
        if (colliders.Length > 0)
        {
            Rigidbody playerBody = colliders[0].GetComponent<Rigidbody>();
            playerBody.AddExplosionForce(100, body.position, 75.0f, 75.0f, ForceMode.Impulse);
        }
        StartCoroutine(SendReleaseaEventAfterSeconds(explosionParticleSystem.main.duration));
    }

}
