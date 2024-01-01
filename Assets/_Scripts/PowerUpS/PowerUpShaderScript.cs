using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpShaderScript : MonoBehaviour
{
    [ColorUsage(true, true)]
    [SerializeField] Color glowColor;
    [SerializeField] Color iconBorderColor;
    [SerializeField] Color bgFillColor;

    [SerializeField] GameObject bgBorder;
    [SerializeField] GameObject bgFill;
    [SerializeField] GameObject iconBorder;
    [SerializeField] GameObject iconFill;
    [SerializeField] Texture2D iconFillTex;


    void Start()
    {
        Material material;
        if (bgBorder != null)
        {
            material = bgBorder.GetComponent<SpriteRenderer>().material;
            material.SetColor("_GlowColor", glowColor);
        }        

        material = iconFill.GetComponent<SpriteRenderer>().material;
        material.SetTexture("_MainTexture", iconFillTex);
        material.SetColor("_GlowColor", glowColor);

        SpriteRenderer sr = bgFill.GetComponent<SpriteRenderer>();
        sr.color = bgFillColor;

        sr = iconBorder.GetComponent<SpriteRenderer>();
        sr.color = iconBorderColor;
    }
}