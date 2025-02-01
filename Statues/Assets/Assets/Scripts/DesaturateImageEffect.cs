using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesaturateImageEffect : MonoBehaviour
{
    public Material effectMaterial;
    [Range(0, 1)] public float desaturateAmount = 0.5f;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (effectMaterial != null)
        {
            effectMaterial.SetFloat("_DesaturateAmount", desaturateAmount);
            Graphics.Blit(src, dest, effectMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}
