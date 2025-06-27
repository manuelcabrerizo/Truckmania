using System;
using UnityEngine;

public class PlayerBarrilState : State<Player>
{
    public PlayerBarrilState(Player owner, Func<bool> enterCondition)
        : base(owner, enterCondition) { }

    public override void OnEnter()
    {
        InputManager.onShoot += OnShoot;
    }

    public override void OnExit() 
    {
        InputManager.onShoot -= OnShoot;
    }

    public override void OnUpdate()
    {
        PlayerData data = owner.Data;
        if (data.barril)
        {
            data.barril.transform.position = data.barrilTransform.position;
            data.barril.transform.rotation = data.barrilTransform.rotation;
        }
    }

    private void OnShoot()
    {
        PlayerData data = owner.Data;
        ToxicBarrilProjectile barril = data.barril;
        Transform transform = owner.transform;

        if (barril != null && data.aimBar.IsAiming())
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 200.0f, data.enemyMask);
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
                    float aimPercentage = data.aimBar.GetAimPercentage();
                    barril.PrepareForLunch();

                    if (aimPercentage < 0.2f)
                    {
                        barril.Explote();
                        owner.TakeDamage(1);
                    }
                    else
                    {
                        float attackRadioRatio = Mathf.Min(Mathf.Sqrt(minDistSq) / 200.0f, 1.0f);
                        float timeToTarget = 3.0f - (3.0f * (1.0f - attackRadioRatio));
                        Vector3 shootPos = colliders[minIndex].transform.position + Vector3.up * 20.0f;
                        if (aimPercentage >= 0.8f)
                        {
                            barril.Lunch(barril.transform.position, shootPos, timeToTarget);

                        }
                        else
                        {
                            Vector3 toTarget = shootPos + barril.transform.position;
                            toTarget.y = 0.0f;
                            toTarget.Normalize();
                            Vector3 right = Vector3.Cross(toTarget, Vector3.up);
                            Vector3 offset = right * UnityEngine.Random.Range(-1.0f, 1.0f);
                            offset.Normalize();
                            offset *= (100.0f * (1.0f - aimPercentage));
                            barril.Lunch(barril.transform.position, shootPos + offset, timeToTarget);
                        }
                    }
                    data.barril = null;
                }
            }
        }
    }
}
