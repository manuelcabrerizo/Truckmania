using UnityEngine;

public class CameraDepthTexture : MonoBehaviour
{
    [SerializeField] private DepthTextureMode depthTextureMode;

    private void OnValidate()
    {
        GetComponent<Camera>().depthTextureMode = depthTextureMode;
    }

    private void Awake()
    {
        GetComponent<Camera>().depthTextureMode = depthTextureMode;
    }
}
