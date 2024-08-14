using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableComponentOnAwake : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    

    private void Awake()
    {
        spriteRenderer.enabled = false;
    }
}
