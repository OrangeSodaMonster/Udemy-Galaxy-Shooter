using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxMat : MonoBehaviour
{    
    Material mat;

    [SerializeField] float parallaxWeight;
    PlayerMove playerMove;

    float xDist = 0;
    float yDist = 0;



    void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
        playerMove = FindObjectOfType<PlayerMove>();
    }

    void Update()
    {
        float percentSpeedX = playerMove.PlayerVelocity.x / playerMove.MaxSpeed;
        float percentSpeedY = playerMove.PlayerVelocity.y / playerMove.MaxSpeed;

        xDist += Time.deltaTime * 1 * parallaxWeight;
        yDist += Time.deltaTime * 1 * parallaxWeight;

        mat.SetVector("_Offset", Vector2.right * parallaxWeight);
        //mat.SetTextureOffset("_MainTex", Vector2.right * parallaxWeight);
    }
}