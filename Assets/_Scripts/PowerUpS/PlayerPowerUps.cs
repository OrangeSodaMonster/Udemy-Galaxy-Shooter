using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUps : MonoBehaviour
{
    PlayerLasers playerWeapons;
    bool isFasterShooting = false;
    float fasterShootingRunTime = float.MaxValue;
    float fasterShootingTotalDuration;

    TractorBeamScript tractorBeam;
    bool isTractorPU = false;
    float tractorPURunTime = float.MaxValue;
    float tractorPUTotalDuration;

    ShieldScript shield;
    bool isShieldPU = false;
    float shieldPURunTime = float.MaxValue;
    float shieldPUTotalDuration;

    PlayerHeal healing;
    bool isHealingPU = false;
    float healingPURunTime = float.MaxValue;
    float healingPUTotalDuration;

    void Start()
    {
        playerWeapons = GetComponent<PlayerLasers>();
        tractorBeam = GetComponentInChildren<TractorBeamScript>();
        shield = transform.parent.GetComponentInChildren<ShieldScript>();
        healing = GetComponent<PlayerHeal>();
    }

    private void Update()
    {
        StopFasterShootingCD();
        StopTractorPUCD();
        StopShieldPUCD();
        StopHealingPUCD();
    }

    int lastCollHash = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.layer == 9 && collision.GetHashCode() != lastCollHash)
        {
            lastCollHash = collision.GetHashCode();
            StartCoroutine(CleanLastHash());

            if (collision.GetComponent<FasterShootingPowerUp>() != null)
                StartFasterShooting(collision);

            if (collision.GetComponent<TractorBeamPowerUp>() != null)
                StartTractorPU(collision);

            if (collision.GetComponent<ShieldPowerUp>() != null)
                StartShieldPU(collision);

            if (collision.GetComponent<HealPowerUp>() != null)
                StartHealingPU(collision);

            if (collision.GetComponent<BombPowerUp>() != null)
                GetBomb(collision);            
        }
    }

    void DestroyPowerUpPickUp( Collider2D collision)
    {
        collision.enabled = false;
        Destroy(collision.gameObject);
    }

    // Faster Shooting
    private void StartFasterShooting(Collider2D collision)
    {
        if (!isFasterShooting) 
            playerWeapons.PowerUpStart(collision.GetComponent<FasterShootingPowerUp>().ShootingMultiplier);

        fasterShootingRunTime = 0;
        isFasterShooting = true;
        fasterShootingTotalDuration = collision.GetComponent<FasterShootingPowerUp>().Duration;

        AudioManager.Instance.FasterShootingPUSound.PlayFeedbacks();

        DestroyPowerUpPickUp(collision);
    }
    private void StopFasterShootingCD()
    {
        if (isFasterShooting)
        {
            fasterShootingRunTime += Time.deltaTime;
            if (fasterShootingRunTime >= fasterShootingTotalDuration)
            {
                isFasterShooting=false;
                playerWeapons.PowerUpEnd();

                AudioManager.Instance.EndPUSound.PlayFeedbacks();
            }
        }
    }

    // Tractor Beam
    private void StartTractorPU(Collider2D collision)
    {
        if (!isTractorPU)
        {
           TractorBeamPowerUp tractorPU = collision.GetComponent<TractorBeamPowerUp>();
           tractorBeam.PowerUpStart(tractorPU.NewColorAlpha, tractorPU.RadiusMod, tractorPU.PullForceMod, tractorPU.TextureSpeedMod);
        }

        tractorPURunTime = 0;
        isTractorPU = true;
        tractorPUTotalDuration = collision.GetComponent<TractorBeamPowerUp>().Duration;

        AudioManager.Instance.TractorBeamPUSound.PlayFeedbacks();


        DestroyPowerUpPickUp(collision);
    }
    private void StopTractorPUCD()
    {
        if (isTractorPU)
        {
            tractorPURunTime += Time.deltaTime;
            if (tractorPURunTime >= tractorPUTotalDuration)
            {
                isTractorPU = false;
                tractorBeam.PowerUpEnd();

                AudioManager.Instance.EndPUSound.PlayFeedbacks();
            }
        }
    }

    // Shield
    private void StartShieldPU(Collider2D collision)
    {
        if (!isShieldPU)
        {
            ShieldPowerUp shieldPU = collision.GetComponent<ShieldPowerUp>();
            shield.PowerUpStart(shieldPU.RegenMod, shieldPU.ExtraStrPerc, shieldPU.PUAddAlpha);
        }

        shieldPURunTime = 0;
        isShieldPU = true;
        shieldPUTotalDuration = collision.GetComponent<ShieldPowerUp>().Duration;

        AudioManager.Instance.ShieldPUSound.PlayFeedbacks();

        DestroyPowerUpPickUp(collision);
    }
    private void StopShieldPUCD()
    {
        if (isShieldPU)
        {
            shieldPURunTime += Time.deltaTime;
            if (shieldPURunTime >= shieldPUTotalDuration)
            {
                isShieldPU = false;
                shield.PowerUpEnd();

                AudioManager.Instance.EndPUSound.PlayFeedbacks();
            }
        }
    }

    // Heal
    private void StartHealingPU(Collider2D collision)
    {
        if (!isShieldPU)
        {
            HealPowerUp healPU = collision.GetComponent<HealPowerUp>();
            healing.PowerUpStart(healPU.HealCD);
        }

        healingPURunTime = 0;
        isHealingPU = true;
        healingPUTotalDuration = collision.GetComponent<HealPowerUp>().Duration;

        AudioManager.Instance.HealPUSound.PlayFeedbacks();

        DestroyPowerUpPickUp(collision);
    }
    private void StopHealingPUCD()
    {
        if (isHealingPU)
        {
            healingPURunTime += Time.deltaTime;
            if (healingPURunTime >= healingPUTotalDuration)
            {
                isHealingPU = false;
                healing.PowerUpEnd();

                AudioManager.Instance.EndPUSound.PlayFeedbacks();
            }
        }
    }

    private void GetBomb(Collider2D collision)
    {
        if (BombScript.BombAmount < BombScript.MaxBombs)
        {
            BombPowerUp bombPowerUp = collision.GetComponent<BombPowerUp>();
            BombScript.BombAmount += bombPowerUp.NumberOfCharges;

            AudioManager.Instance.BombPickSound.PlayFeedbacks();

            DestroyPowerUpPickUp(collision);
        }
    }

    WaitForSeconds wait1 = new WaitForSeconds(1);
    IEnumerator CleanLastHash()
    {
        yield return wait1;

        lastCollHash = 0;
    }
}