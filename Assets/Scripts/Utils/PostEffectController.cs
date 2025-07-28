using System.Collections;
using UnityEngine;

public class PostEffectController : MonoBehaviour
{
    [SerializeField] private Shader postEffectShader;
    [SerializeField] private Material postEffectMaterial;

    [SerializeField] private Shader postEffectShader1;
    [SerializeField] private Material postEffectMaterial1;

    private bool isOnWater = false;

    private void Awake()
    {
        isOnWater = false;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        RenderTexture renderTexture = RenderTexture.GetTemporary(
            source.width, source.height, 0, source.format
        );

        if (isOnWater)
        {
            RenderTexture renderTexture1 = RenderTexture.GetTemporary(
                source.width, source.height, 0, source.format);

            Graphics.Blit(source, renderTexture, postEffectMaterial1);
            Graphics.Blit(renderTexture, renderTexture1, postEffectMaterial);
            Graphics.Blit(renderTexture1, destination);
            RenderTexture.ReleaseTemporary(renderTexture1);
        }
        else
        {
            Graphics.Blit(source, renderTexture, postEffectMaterial);
            Graphics.Blit(renderTexture, destination);
        }
        RenderTexture.ReleaseTemporary(renderTexture);
    }

    public void SetIsOnWater(bool value)
    {
        StopAllCoroutines();
        if (value)
        {
            StartCoroutine(EnterWaterAnimation(0.75f));
        }
        else
        {
            StartCoroutine(ExitWaterAnimation(3.0f));
        }
    }

    private IEnumerator EnterWaterAnimation(float duration)
    {
        isOnWater = true;
        float time = 0;
        while (time < duration)
        {
            postEffectMaterial1.SetFloat("_Intesity", (time/duration));
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }
        postEffectMaterial1.SetFloat("_Intesity", 1.0f);
    }

    private IEnumerator ExitWaterAnimation(float duration)
    {
        float time = 0;
        while (time < duration)
        {
            postEffectMaterial1.SetFloat("_Intesity", 1.0f - (time/duration));
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }
        postEffectMaterial1.SetFloat("_Intesity", 0.0f);
        isOnWater = false;
    }
}
