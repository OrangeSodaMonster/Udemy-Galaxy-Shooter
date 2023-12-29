using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTractorBeam : MonoBehaviour
{
    [SerializeField] float radiusMod = 1f;  // Base radius = scale
    [SerializeField] float baseMaxPullSpeed = 3f;
    public float MaxPullSpeed;
    [SerializeField] float baseTimeToMaxPullSpeed = 2f;
    public float TimeToMaxPullSpeed;

    Vector3 defaultScale;
    float defaultAlpha;
    float defaultTexSpeed;
    Color defaultColor;

    float puRadiusMod = 1;
    float maxAtractionSpeedMod = 1;
    float timeToMaxSpeedMod = 1;

    private void Start()
    {
        defaultScale = transform.localScale;
        defaultAlpha = GetComponent<SpriteRenderer>().color.a;
        defaultTexSpeed = GetComponent<SpriteRenderer>().material.GetFloat("_Speed");
        defaultColor = GetComponent<SpriteRenderer>().color;

        MaxPullSpeed = baseMaxPullSpeed;
        TimeToMaxPullSpeed = baseTimeToMaxPullSpeed;
    }

    private void Update()
    {
        transform.localScale = defaultScale * radiusMod * puRadiusMod;
        MaxPullSpeed = baseMaxPullSpeed * maxAtractionSpeedMod;
        TimeToMaxPullSpeed = baseTimeToMaxPullSpeed * timeToMaxSpeedMod;
    }

    public void PowerUpStart(float newAlpha, float puRadiusMod, float atractionMod, float timeToMaxSpeedMod, float textureSpeedMod)
    {
        this.puRadiusMod = puRadiusMod;
        maxAtractionSpeedMod = atractionMod;
        this.timeToMaxSpeedMod = timeToMaxSpeedMod;

        GetComponent<SpriteRenderer>().material.SetFloat("_Speed", defaultTexSpeed * textureSpeedMod);
        GetComponent<SpriteRenderer>().color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, newAlpha);
    }

    public void PowerUpEnd()
    {
        puRadiusMod = 1;
        maxAtractionSpeedMod = 1;
        timeToMaxSpeedMod = 1;

        GetComponent<SpriteRenderer>().material.SetFloat("_Speed", defaultTexSpeed);
        GetComponent<SpriteRenderer>().color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, defaultAlpha);
    }
}