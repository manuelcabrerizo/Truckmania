using System;
using UnityEngine;

public class ToxicBarrilProjectile : BarrilProjectile, IPickable
{
    public static event Action<ToxicBarrilProjectile> onBarrilPickUp;

    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private ParticleSystem toxicParticleSystem;

    public override void OnGet()
    {
        base.OnGet();
        gameObject.layer = 0;
        transform.rotation = Quaternion.identity;
        barrilRenderer.enabled = true;
        collision.enabled = true;
        collision.isTrigger = true;
        body.velocity = Vector3.zero;
        body.isKinematic = true;
        body.useGravity = true;
    }

    public override void OnRelease()
    {
        base.OnRelease();
        barrilRenderer.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    public void PickUp()
    {
        if (collision.isTrigger == true)
        {
            barrilRenderer.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            collision.enabled = false;
            body.useGravity = false;

            onBarrilPickUp?.Invoke(this);
        }
    }

    public void PrepareForLunch()
    {
        barrilRenderer.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        collision.enabled = true;
        collision.isTrigger = false;
        body.isKinematic = false;
        body.useGravity = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Utils.CheckCollisionLayer(collision.gameObject, enemyMask))
        {
            Explote();
        }
    }

    private void Explote()
    {
        toxicParticleSystem.Play();
        barrilRenderer.enabled = false;
        collision.enabled = false;
        body.isKinematic = true;
        StartCoroutine(SendReleaseaEventAfterSeconds(toxicParticleSystem.main.duration));
    }
}
