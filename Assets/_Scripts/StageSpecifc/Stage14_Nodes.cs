using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage14_Nodes : MonoBehaviour
{
    [SerializeField] Material OnMat;

    SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    [Button]
    public void TurnOn()
    {
        spriteRenderer.color = Color.white;
        spriteRenderer.material = OnMat;
    }
}
