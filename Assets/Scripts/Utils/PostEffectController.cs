using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostEffectController : MonoBehaviour
{
    [SerializeField] private Shader postEffectShader;
    [SerializeField] private Material postEffectMaterial;
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        RenderTexture renderTexture = RenderTexture.GetTemporary(
            source.width, source.height, 0, source.format
        );
        Graphics.Blit(source, renderTexture, postEffectMaterial);
        Graphics.Blit(renderTexture, destination);
        RenderTexture.ReleaseTemporary(renderTexture);
    }
}
