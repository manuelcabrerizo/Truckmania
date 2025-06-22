using Unity.VisualScripting;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Transform barrilTransform;
    private ToxicBarrilProjectile barril = null;

    private void Awake()
    {
        ToxicBarrilProjectile.onBarrilPickUp += OnBarrilPickUp;
        InputManager.onShoot += OnShoot;
    }

    private void OnDestroy()
    {
        ToxicBarrilProjectile.onBarrilPickUp -= OnBarrilPickUp;
        InputManager.onShoot -= OnShoot;
    }

    private void OnBarrilPickUp(ToxicBarrilProjectile barril)
    {
        if (this.barril != null)
        {
            this.barril.SendReleaseEvent();
        }
        this.barril = barril;
        barril.gameObject.layer = LayerMask.NameToLayer("ToxicBarril");
    }

    private void Update()
    {
        if (barril != null)
        {
            barril.transform.position = barrilTransform.position;
            barril.transform.rotation = barrilTransform.rotation;
        }
    }

    private void OnShoot()
    {
        if (barril != null)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 200.0f, enemyMask);
            if (colliders.Length > 0)
            {
                int minIndex = -1;
                float minDistSq = float.MaxValue;
                for (int i = 0; i < colliders.Length; ++i)
                {
                    float distSq = (transform.position - colliders[i].transform.position).sqrMagnitude;
                    if (distSq < minDistSq)
                    {
                        minDistSq = distSq;
                        minIndex = i;
                    }
                }

                if (minIndex >= 0)
                {
                    barril.PrepareForLunch();
                    float attackRadioRatio = Mathf.Min(Mathf.Sqrt(minDistSq) / 200.0f, 1.0f);
                    float timeToTarget = 3.0f - (3.0f * (1.0f - attackRadioRatio));
                    Vector3 shootPos = colliders[minIndex].transform.position + Vector3.up * 5.0f;
                    barril.Lunch(barril.transform.position, shootPos, timeToTarget);
                    barril = null;
                }
            }
        }
    }
}
