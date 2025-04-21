using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorBeamScript : MonoBehaviour
{
    [SerializeField] float radiusMod = 1f;  // Base radius = scale
    [SerializeField] float basePullForce = 3f;
    public float TotalPullForce;
    public float TimeToMaxPullSpeed = 2f;

    Vector3 defaultScale;
    float defaultAlpha;
    float defaultTextureSpeed;
    SpriteRenderer spriteRenderer;
    Color defaultColor;
    Collider2D coll;
    List<Rigidbody2D> collectiblesToPull = new List<Rigidbody2D>();
    float radius;
    float puRadiusMod = 1;
    float pullForceMod = 1;
    float textureSpeedMod = 1;

    PlayerUpgradesManager upgradesManager;

    private void Awake()
    {
        defaultScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
        coll = GetComponent<Collider2D>();
    }

    private void Start()
    {
        radius = GetComponent<CircleCollider2D>().radius;
        radius *= transform.lossyScale.x;

        upgradesManager = FindObjectOfType<PlayerUpgradesManager>();

        SetEnabled();
        UpdateValues();
    }

    private void Update()
    {
        SetEnabled();
        UpdateValues();

        transform.localScale = defaultScale * radiusMod * puRadiusMod;        
        spriteRenderer.material.SetFloat("_Speed", defaultTextureSpeed * textureSpeedMod);
        TotalPullForce = basePullForce * pullForceMod;
    }

    void UpdateValues()
    {
        radiusMod = PlayerStats.Instance.Ship.Tractor.CurrentRange;
        basePullForce = PlayerStats.Instance.Ship.Tractor.CurrentForce;
        defaultAlpha = upgradesManager.ShipUpgradesInfo.TractorBeamUpgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.TractorBeamLevel - 1].Alpha;
        defaultTextureSpeed = upgradesManager.ShipUpgradesInfo.TractorBeamUpgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.TractorBeamLevel - 1].TextureSpeed;
    }

    void SetEnabled()
    {
        bool isEnable = PlayerStats.Instance.Ship.Tractor.Enabled && !GameStatus.IsPortal;

        spriteRenderer.enabled = isEnable;
        coll.enabled = isEnable;
    }

    public void PowerUpStart(float newAlpha, float puRadiusMod, float pullMod, float textureSpeedMod)
    {
        this.puRadiusMod = puRadiusMod;
        pullForceMod = pullMod;
        this.textureSpeedMod = textureSpeedMod;

        spriteRenderer.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, newAlpha);
    }

    public void PowerUpEnd()
    {
        puRadiusMod = 1;
        pullForceMod = 1;
        textureSpeedMod = 1;

        spriteRenderer.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, defaultAlpha);
    }
}