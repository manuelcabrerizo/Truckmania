using System.Collections;
using UnityEngine;

public class PostEffectController : MonoBehaviour
{
    [SerializeField] private Shader postEffectShader;
    [SerializeField] private Material postEffectMaterial;

    [SerializeField] private Shader postEffectShader1;
    [SerializeField] private Material postEffectMaterial1;


    private float waterInteisty = 0;
    [SerializeField] private float incSpeed = 1.0f;
    [SerializeField] private float decSpeed = 1.0f / 3.0f;

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
            StartCoroutine(EnterWaterAnimation());
        }
        else
        {
            StartCoroutine(ExitWaterAnimation());
        }
    }

    private IEnumerator EnterWaterAnimation()
    {
        isOnWater = true;
        while (waterInteisty < 1.0f)
        {
            waterInteisty += incSpeed * Time.deltaTime;
            postEffectMaterial1.SetFloat("_Intesity", waterInteisty);
            yield return new WaitForEndOfFrame();
        }
        waterInteisty = 1.0f;
        postEffectMaterial1.SetFloat("_Intesity", waterInteisty);
    }

    private IEnumerator ExitWaterAnimation()
    {
        while (waterInteisty > 0.0f)
        {
            waterInteisty -= decSpeed * Time.deltaTime;
            postEffectMaterial1.SetFloat("_Intesity", waterInteisty);
            yield return new WaitForEndOfFrame();
        }
        waterInteisty = 0.0f;
        postEffectMaterial1.SetFloat("_Intesity", waterInteisty);
        isOnWater = false;
    }
}
