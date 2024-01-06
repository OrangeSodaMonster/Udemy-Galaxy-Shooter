using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTractorBeam : MonoBehaviour
{
    [SerializeField] float radiusMod = 1f;  // Base radius = scale
    [SerializeField] float basePullForce = 3f;
    public float TotalPullForce;
    public float TimeToMaxPullSpeed = .5f;

    Vector3 defaultScale;
    float defaultAlpha;
    float defaultTextureSpeed;
    SpriteRenderer spriteRenderer;
    Color defaultColor;
    Collider2D coll;
    List<Rigidbody2D> collectiblesToPull = new List<Rigidbody2D>();

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
    }

    void FixedUpdate()
    {
        foreach (var rb in collectiblesToPull)
        {
            if (rb == null)
            {
                collectiblesToPull.Remove(rb);
                return;
            }

            Vector2 direction = (transform.position - rb.transform.position).normalized;

            TotalPullForce = basePullForce * pullForceMod;
            //Vector2 perpendicularVector = new Vector2(direction.y, -direction.x).normalized;
            //float perpendicularPullForce = Vector2.Dot(rb.velocity, perpendicularVector);
            rb.AddForce(TotalPullForce * direction, ForceMode2D.Force);
            //rb.AddForce(-TotalPullForce * perpendicularVector * 0.9f * Mathf.Abs(perpendicularPullForce), ForceMode2D.Force);

            //rb.velocity = (direction * TotalPullForce);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CollectiblesPickUps>() != null && collision.TryGetComponent(out Rigidbody2D collRB))
            collectiblesToPull.Add(collRB);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Rigidbody2D collRB) && collectiblesToPull.Contains(collRB))
            collectiblesToPull.Remove(collRB);
    }

    void UpdateValues()
    {
        radiusMod = upgradesManager.ShipUpgradesInfo.TractorBeamUpgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.TractorBeamLevel - 1].RadiusMod;
        basePullForce = upgradesManager.ShipUpgradesInfo.TractorBeamUpgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.TractorBeamLevel - 1].PullForce;;
        defaultAlpha = upgradesManager.ShipUpgradesInfo.TractorBeamUpgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.TractorBeamLevel - 1].Alpha;
        defaultTextureSpeed = upgradesManager.ShipUpgradesInfo.TractorBeamUpgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.TractorBeamLevel - 1].TextureSpeed;
    }

    void SetEnabled()
    {
        spriteRenderer.enabled = upgradesManager.CurrentUpgrades.ShipUpgrades.TractorBeamEnabled;
        coll.enabled = upgradesManager.CurrentUpgrades.ShipUpgrades.TractorBeamEnabled;
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