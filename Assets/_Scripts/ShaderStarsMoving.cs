using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderStarsMoving : MonoBehaviour
{
    Material material;
    [SerializeField] float maxStarsSpeed;
    [SerializeField] float scrollSpeedLayerMod;
    [Space]
    [SerializeField] Vector2 scrollSpeedAxisMod = Vector2.one;

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
        if (isStopStars) return;

        percentSpeedX = playerMove.PlayerVelocity.x / playerMove.MaxSpeed;
        percentSpeedY = playerMove.PlayerVelocity.y / playerMove.MaxSpeed;

        scrollOffset.x += Mathf.Abs(percentSpeedX) * Time.deltaTime * playerMove.PlayerVelocity.normalized.x * scrollSpeedAxisMod.x;
        scrollOffset.y += Mathf.Abs(percentSpeedY) * Time.deltaTime * playerMove.PlayerVelocity.normalized.y * scrollSpeedAxisMod.y;
               
        material.SetVector("_ScrollOffset", scrollOffset);
    }

    bool isStopStars;
    Vector2 starsSpeed;
    public void StopStars()
    {
        if(material == null) return;

        isStopStars = true;
        DOTween.To(() => scrollOffset.x, x => scrollOffset.x = x, 0, 1.2f).OnUpdate(() => UpdateStopping());
        DOTween.To(() => scrollOffset.y, x => scrollOffset.y = x, 0, 1.2f).OnUpdate(() => UpdateStopping());
    }

    void UpdateStopping()
    {
        material.SetVector("_ScrollOffset", scrollOffset);
    }
}
