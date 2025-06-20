using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Transform barrilTransform;
    private ToxicBarrilProjectile barril = null;

    private void Awake()
    {
        ToxicBarrilProjectile.onBarrilPickUp += OnBarrilPickUp;
    }

    private void OnDestroy()
    {
        ToxicBarrilProjectile.onBarrilPickUp -= OnBarrilPickUp;
    }

    private void OnBarrilPickUp(ToxicBarrilProjectile barril)
    {
        if (this.barril != null)
        {
            this.barril.SendReleaseEvent();
        }
        this.barril = barril;
    }

    private void Update()
    {
        if (barril != null)
        {
            barril.transform.position = barrilTransform.position;
            barril.transform.rotation = barrilTransform.rotation;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                barril.PrepareForLunch();
                barril.Lunch(barril.transform.position, transform.position + transform.forward * 100, 1.0f);
                barril = null;
            }
        }
    }
}
