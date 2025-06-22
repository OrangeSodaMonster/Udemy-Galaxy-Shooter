using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMat : MonoBehaviour
{
    public Material OnMat;
    public Material OffMat;

    Material originalMat;
    Color originalColor;
    SpriteRenderer spriteRenderer;
    Color offColor = Color.gray;

    private void Awake()
    {
        ColorUtility.TryParseHtmlString("#666666", out offColor);
    }
    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMat = spriteRenderer.material;
        originalColor = spriteRenderer.color;
    }

    [Button, PropertyOrder(-1)]
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


    [Button, HorizontalGroup("OnOff")]
    public void TurnOnMat()
    {
        spriteRenderer.color = Color.white;
        spriteRenderer.material = OnMat;
    }
    [Button, HorizontalGroup("OnOff")]
    public void TurnOffMat()
    {
        spriteRenderer.color = offColor;
        spriteRenderer.material = OffMat;
    }
}
