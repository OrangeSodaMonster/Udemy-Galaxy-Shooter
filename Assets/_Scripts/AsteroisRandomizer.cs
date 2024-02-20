using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroisRandomizer : MonoBehaviour
{
    [SerializeField] AsteroidMatHolderSO mats;
    [SerializeField] bool sizeVariation = false;
    [SerializeField] float sizeVariationPerc = 10;

    SpriteRenderer spriteRenderer;
    int index;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        index = GetRandomMatIndex();
        spriteRenderer.material = mats.Mats[index];
        spriteRenderer.flipX = RandomBool();
        spriteRenderer.flipY = RandomBool();

        if (sizeVariation)
            ApplySizeVariation();
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
        return UnityEngine.Random.Range(0, mats.Mats.Length);
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