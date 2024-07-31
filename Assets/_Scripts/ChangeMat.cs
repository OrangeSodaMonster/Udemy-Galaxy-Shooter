using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMat : MonoBehaviour
{
    [SerializeField] Material OnMat;

    Material originalMat;
    Color originalColor;
    SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMat = spriteRenderer.material;
        originalColor = spriteRenderer.color;
    }

    [Button]
    public void ChangeMaterial()
    {
        spriteRenderer.color = Color.white;
        spriteRenderer.material = OnMat;
    }

    public void ReturnOriginalMaterial()
    {
        spriteRenderer.color = originalColor;
        spriteRenderer.material = originalMat;
    }
}
