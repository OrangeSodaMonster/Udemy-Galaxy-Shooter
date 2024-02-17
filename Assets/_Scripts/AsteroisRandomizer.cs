using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroisRandomizer : MonoBehaviour
{
    [SerializeField] AsteroidMatHolderSO mats;

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.material = mats.Mats[GetRandomMatIndex()];
        spriteRenderer.flipX = RandomBool();
        spriteRenderer.flipY = RandomBool();
    }

    bool RandomBool()
    {
        return (Random.Range(0, 2) == 1);
    }

    int GetRandomMatIndex()
    {
        return Random.Range(0, mats.Mats.Length);
    }
}