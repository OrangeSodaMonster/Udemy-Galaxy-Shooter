using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroisRandomizer : MonoBehaviour
{
    [SerializeField] AsteroidMatHolderSO mats;

    SpriteRenderer spriteRenderer;
    int index;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        index = GetRandomMatIndex();
        spriteRenderer.material = mats.Mats[index];
        spriteRenderer.flipX = RandomBool();
        spriteRenderer.flipY = RandomBool();
    }

    bool RandomBool()
    {
        return (UnityEngine.Random.Range(0, 2) == 1);
    }

    int GetRandomMatIndex()
    {
        return UnityEngine.Random.Range(0, mats.Mats.Length);
    }
}