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

    // apenas quando os collectibles são dinâmicos (em um buraco negro por exemplo)
    //void FixedUpdate()
    //{
    //    foreach (var rb in collectiblesToPull)
    //    {
    //        if (rb == null || !rb.isKinematic)
    //        {
    //            collectiblesToPull.Remove(rb);
    //            return;
    //        }

    //        Vector2 direction = (transform.position - rb.transform.position).normalized;

    //        TotalPullForce = basePullForce * pullForceMod;
    //        rb.AddForce(TotalPullForce * direction, ForceMode2D.Force);
    //    }
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.GetComponent<CollectiblesPickUps>() != null && collision.TryGetComponent(out Rigidbody2D collRB)
    //        && !collRB.isKinematic)
    //        collectiblesToPull.Add(collRB);
    //}
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.TryGetComponent(out Rigidbody2D collRB) && collectiblesToPull.Contains(collRB))
    //        collectiblesToPull.Remove(collRB);
    //}

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