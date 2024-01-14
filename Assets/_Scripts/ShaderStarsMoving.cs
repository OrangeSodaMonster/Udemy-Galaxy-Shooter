using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderStarsMoving : MonoBehaviour
{
    Material material;
    [SerializeField] float maxStarsSpeed;
    [SerializeField] float scrollSpeedLayerMod;

    PlayerMove playerMove;

    private void Awake()
    {

        material = GetComponent<SpriteRenderer>().material;
    }

    void Start()
    {
        playerMove = FindObjectOfType<PlayerMove>();
        material.SetFloat("_ScrollSpeed", maxStarsSpeed);
        material.SetFloat("_ScrollSpeedLayerMod", scrollSpeedLayerMod);
    }

    float percentSpeedX;
    float percentSpeedY;
    Vector2 scrollOffset = Vector2.zero;
    void Update()
    {
        percentSpeedX = playerMove.PlayerVelocity.x / playerMove.MaxSpeed;
        percentSpeedY = playerMove.PlayerVelocity.y / playerMove.MaxSpeed;

        scrollOffset.x += Mathf.Abs(percentSpeedX) * Time.deltaTime * playerMove.PlayerVelocity.normalized.x;
        scrollOffset.y += Mathf.Abs(percentSpeedY) * Time.deltaTime * playerMove.PlayerVelocity.normalized.y;
               
        material.SetVector("_ScrollOffset", new Vector4(scrollOffset.x, scrollOffset.y, 0, 0));
    }
}
