using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRandomizer : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] bool sizeVariation = false;
    [SerializeField] float sizeVariationPerc = 5;

    SpriteRenderer spriteRenderer;
    int index;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        index = GetRandomMatIndex();
        spriteRenderer.sprite = sprites[index];
        spriteRenderer.flipX = RandomBool();
        spriteRenderer.flipY = RandomBool();

        if (sizeVariation)
            ApplySizeVariation();
    }

    private void Start()
    {
        transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward);
    }

    bool RandomBool()
    {
        return (UnityEngine.Random.Range(0, 2) == 1);
    }
    int RandomSign()
    {
        return (int)(Mathf.Sign(UnityEngine.Random.Range(-1f, 1f)));
    }

    int GetRandomMatIndex()
    {
        return UnityEngine.Random.Range(0, sprites.Length);
    }

    void ApplySizeVariation()
    {
        float variation = GetSizeVariation();
        transform.localScale += transform.localScale * (variation * 0.01f);
    }

    float GetSizeVariation()
    {
        return UnityEngine.Random.Range(0f, sizeVariationPerc) * RandomSign();
    }
}