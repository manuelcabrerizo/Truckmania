using UnityEngine;

public class PlayerAimBar : MonoBehaviour
{
    [SerializeField] private GameObject aimBar;
    [SerializeField] private Vector3 offset;

    private MeshRenderer meshRenderer;
    private float timer = 0;

    private void Awake()
    {
        CameraMovement.onTargetLock += OnTargetLock;
        CameraMovement.onTargetUnlock += OnTargetUnlock;
        meshRenderer = aimBar.GetComponent<MeshRenderer>();
    }

    private void OnDestroy()
    {
        CameraMovement.onTargetLock -= OnTargetLock;
        CameraMovement.onTargetUnlock -= OnTargetUnlock;
    }

    private void Update()
    {
        if (aimBar.activeSelf)
        {
            timer += Time.deltaTime * 2;
            meshRenderer.material.SetFloat("_MarkerPosition", Mathf.Lerp(-4.1f, 4.1f, Mathf.Sin(timer) * 0.5f + 0.5f));
            aimBar.transform.position = transform.position + offset;
            aimBar.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        }
    }

    private void OnTargetLock()
    { 
        aimBar.SetActive(true);
    }

    private void OnTargetUnlock()
    { 
        aimBar?.SetActive(false);
    }
}
