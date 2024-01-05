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
    float defaultTextureSpeed;
    SpriteRenderer spriteRenderer;
    Color defaultColor;
    Collider2D coll;

    float puRadiusMod = 1;
    float maxAtractionSpeedMod = 1;
    float timeToMaxSpeedMod = 1;
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
        upgradesManager = FindObjectOfType<PlayerUpgradesManager>();

        SetEnabled();
        UpdateValues();
    }

    private void Update()
    {
        SetEnabled();
        UpdateValues();

        transform.localScale = defaultScale * radiusMod * puRadiusMod;
        MaxPullSpeed = baseMaxPullSpeed * maxAtractionSpeedMod;
        TimeToMaxPullSpeed = baseTimeToMaxPullSpeed * timeToMaxSpeedMod;
        spriteRenderer.material.SetFloat("_Speed", defaultTextureSpeed * textureSpeedMod);
    }

    void UpdateValues()
    {
        radiusMod = upgradesManager.ShipUpgradesInfo.TractorBeamUpgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.TractorBeamLevel - 1].RadiusMod;
        baseMaxPullSpeed = upgradesManager.ShipUpgradesInfo.TractorBeamUpgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.TractorBeamLevel - 1].MaxPullSpeed;
        baseTimeToMaxPullSpeed = upgradesManager.ShipUpgradesInfo.TractorBeamUpgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.TractorBeamLevel - 1].TimeToMaxPull;
        defaultAlpha = upgradesManager.ShipUpgradesInfo.TractorBeamUpgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.TractorBeamLevel - 1].Alpha;
        defaultTextureSpeed = upgradesManager.ShipUpgradesInfo.TractorBeamUpgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.TractorBeamLevel - 1].TextureSpeed;
    }

    void SetEnabled()
    {
        spriteRenderer.enabled = upgradesManager.CurrentUpgrades.ShipUpgrades.TractorBeamEnabled;
        coll.enabled = upgradesManager.CurrentUpgrades.ShipUpgrades.TractorBeamEnabled;
    }

    public void PowerUpStart(float newAlpha, float puRadiusMod, float atractionMod, float timeToMaxSpeedMod, float textureSpeedMod)
    {
        this.puRadiusMod = puRadiusMod;
        maxAtractionSpeedMod = atractionMod;
        this.timeToMaxSpeedMod = timeToMaxSpeedMod;
        this.textureSpeedMod = textureSpeedMod;

        spriteRenderer.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, newAlpha);
    }

    public void PowerUpEnd()
    {
        puRadiusMod = 1;
        maxAtractionSpeedMod = 1;
        timeToMaxSpeedMod = 1;
        textureSpeedMod = 1;

        spriteRenderer.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, defaultAlpha);
    }
}