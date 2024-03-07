using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BackGroundParallax : MonoBehaviour
{
    [FormerlySerializedAs("Cam")]
    [SerializeField] Transform cam;
    [SerializeField] float parallaxWeight;
    [SerializeField] bool moveInX;
    [SerializeField] bool moveInY;

    Vector2 bgSize = new();
    Vector2 startPos = new();
    Vector2 camStartPos = new();
    bool gotCamStartPos;
    Vector2 refCoords = Vector2.zero;

    float lenght, startX;

    void Start()
    {        
        bgSize = GetComponent<SpriteRenderer>().bounds.size;
        startPos = transform.position;
        //startX = transform.position.x;
        //lenght = GetComponent<SpriteRenderer>().bounds.size.x;
        //Cam = Camera.main.gameObject;

        Debug.Log(bgSize);
    }

    void Update()
    {
        //float temp = (Cam.transform.position.x * (1 - parallaxWeight));
        //float dist = (Cam.transform.position.x * parallaxWeight);

        //transform.position = new Vector3(startX + dist, transform.position.y, transform.position.z);

        //if (temp > startX + lenght) startX += lenght;
        //else if (temp < startX - lenght) startX -= lenght;

        //if (GameStatus.IsPortal) return;
        if (!gotCamStartPos)
        {
            camStartPos = cam.position;
            gotCamStartPos = true;
        }

        Vector2 dist = ((Vector2)cam.transform.position) * parallaxWeight;
        transform.position = new Vector3(startPos.x + dist.x, startPos.y + dist.y, transform.position.z);


        if (moveInX)
        {
            float temp = (cam.position.x - refCoords.x) * (1 - parallaxWeight);

            if (temp > bgSize.x)
            {
                startPos.x += bgSize.x;
                refCoords = cam.position;
            }
            else if (temp < -bgSize.x)
            {
                startPos.x -= bgSize.x;
                refCoords = cam.position;
            }
        }

        if (moveInY)
        {
            float temp = (cam.position.y - refCoords.y) * (1 - parallaxWeight);

            if (temp > bgSize.y)
            {
                startPos.y += bgSize.y;
                refCoords = cam.position;
            }
            else if (temp < -bgSize.y)
            {
                startPos.y -= bgSize.y;
                refCoords = cam.position;
            }
        }
    }
}