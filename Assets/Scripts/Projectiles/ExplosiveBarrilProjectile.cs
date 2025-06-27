using UnityEngine;

public class ExplosiveBarrilProjectile : BarrilProjectile
{
    public override void OnGet()
    {
        base.OnGet();
        particles.Stop();
    }

    public override void Explote()
    {
        base.Explote();
        Collider[] colliders = Physics.OverlapSphere(body.position, 50.0f);
        if (colliders.Length > 0)
        {
            foreach(Collider coll in colliders)
            {
                IDamagable damagable = null;
                if (coll.gameObject.TryGetComponent<IDamagable>(out damagable))
                {
                    Rigidbody target = coll.GetComponent<Rigidbody>();
                    target.AddExplosionForce(100, body.position, 75.0f, 75.0f, ForceMode.Impulse);
                }
            }
        }
    }

}
