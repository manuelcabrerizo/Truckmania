using System.Collections;
using UnityEngine;

public class PlayerAimBar : MonoBehaviour
{
    [SerializeField] private GameObject aimBar;
    [SerializeField] private Vector3 offset;

    private MeshRenderer meshRenderer;
    private float timer = 0;

    private float speed = 2.0f;
    private float currentSpeed = 2.0f;
    private float position = 0.0f;
    private bool isAiming = false;

    private void Awake()
    {
        CameraMovement.onTargetLock += OnTargetLock;
        CameraMovement.onTargetUnlock += OnTargetUnlock;

        meshRenderer = aimBar.GetComponent<MeshRenderer>();
        currentSpeed = speed;
        isAiming = false;
    }

    private void OnDestroy()
    {
        CameraMovement.onTargetLock -= OnTargetLock;
        CameraMovement.onTargetUnlock -= OnTargetUnlock;
        StopAllCoroutines();
    }

    private void Update()
    {
        if (aimBar.activeSelf)
        {
            timer += Time.deltaTime * currentSpeed;
            if (timer >= Mathf.PI * 2.0f)
            {
                timer -= Mathf.PI * 2.0f;
            }
            position = Mathf.Sin(timer);

            meshRenderer.material.SetFloat("_MarkerPosition", position);
            aimBar.transform.position = transform.position + offset;
            aimBar.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        }
    }

    private void OnTargetLock()
    {
        isAiming = true;
        aimBar.SetActive(true);
    }

    private void OnTargetUnlock()
    { 
        isAiming = false;
        aimBar?.SetActive(false);
    }

    public bool IsAiming()
    {
        return isAiming;
    }

    public float GetAimPercentage()
    {
        StartCoroutine(StopMarkerForSeconds(1.0f));
        StartCoroutine(PlayMarkerAnimation(1.0f, 50.0f));
        float aimPercentage = 1.0f - Mathf.Abs(position);
        return aimPercentage;
    }

    IEnumerator StopMarkerForSeconds(float seconds)
    {
        currentSpeed = 0.0f;
        yield return new WaitForSeconds(seconds);
        currentSpeed = speed;
    }

    IEnumerator PlayMarkerAnimation(float duration, float speed)
    {
        float time = 0;
        while (time < duration)
        { 
            time += Time.deltaTime;
            Color color = Color.Lerp(Color.black, new Color(0.5f, 0.5f, 0.5f), Mathf.Sin(time * speed) * 0.5f + 0.5f);
            meshRenderer.material.SetColor("_MarkerTint", color);
            yield return new WaitForEndOfFrame();
        }
        meshRenderer.material.SetColor("_MarkerTint", Color.black);
    }
}


