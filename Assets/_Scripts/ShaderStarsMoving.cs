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

        GameStatus.GameOver += StopStars;
    }
    private void OnDestroy()
    {
        GameStatus.GameOver -= StopStars;
    }

    float percentSpeedX;
    float percentSpeedY;
    Vector2 scrollOffset = Vector2.zero;
    void Update()
    {
        percentSpeedX = playerMove.PlayerVelocity.x / playerMove.MaxSpeed;
        percentSpeedY = playerMove.PlayerVelocity.y / playerMove.MaxSpeed;

        if (isStopStars)
        {
            percentSpeedX = playerMoveOverride.x / playerMove.MaxSpeed;
            percentSpeedY = playerMoveOverride.y / playerMove.MaxSpeed;
        }

        scrollOffset.x += Mathf.Abs(percentSpeedX) * Time.deltaTime * playerMove.PlayerVelocity.normalized.x * scrollSpeedAxisMod.x;
        scrollOffset.y += Mathf.Abs(percentSpeedY) * Time.deltaTime * playerMove.PlayerVelocity.normalized.y * scrollSpeedAxisMod.y;
               
        material.SetVector("_ScrollOffset", scrollOffset);
    }

    bool isStopStars;
    Vector2 playerMoveOverride;
    public void StopStars()
    {
        if(material == null) return;

        playerMoveOverride = playerMove.PlayerVelocity;
        DOTween.To(() => playerMoveOverride.x, x => playerMoveOverride.x = x, 0, .7f);
        DOTween.To(() => playerMoveOverride.y, x => playerMoveOverride.y = x, 0, .7f);
    
        isStopStars = true;        
    }
        
}
