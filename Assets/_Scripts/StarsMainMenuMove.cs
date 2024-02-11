using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsMainMenuMove : MonoBehaviour
{
    Material material;
    [SerializeField] Vector2 moveDirection;
    [SerializeField] float starsSpeed;
    [SerializeField] float scrollSpeedLayerMod;

    private void Awake()
    {

        material = GetComponent<SpriteRenderer>().material;
    }

    void Start()
    {
        material.SetFloat("_ScrollSpeed", starsSpeed);
        material.SetFloat("_ScrollSpeedLayerMod", scrollSpeedLayerMod);
    }

    Vector2 scrollOffset = Vector2.zero;
    void Update()
    {
        scrollOffset += starsSpeed * moveDirection;

        material.SetVector("_ScrollOffset", new Vector4(scrollOffset.x, scrollOffset.y, 0, 0));
    }
}