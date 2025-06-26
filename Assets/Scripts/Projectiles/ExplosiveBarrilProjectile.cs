using UnityEngine;

public class ExplosiveBarrilProjectile : BarrilProjectile
{
    [SerializeField] private LayerMask playerMask;

    public override void OnGet()
    {
        base.OnGet();
        particles.Stop();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Utils.CheckCollisionLayer(collision.gameObject, playerMask))
        {
            Explote();
        }
    }

    public override void Explote()
    {
        base.Explote();
        Collider[] colliders = Physics.OverlapSphere(body.position, 50.0f, playerMask);
        if (colliders.Length > 0)
        {
            Rigidbody playerBody = colliders[0].GetComponent<Rigidbody>();
            playerBody.AddExplosionForce(100, body.position, 75.0f, 75.0f, ForceMode.Impulse);
        }
    }

}
