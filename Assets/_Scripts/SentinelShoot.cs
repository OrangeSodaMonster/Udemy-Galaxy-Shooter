using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentinelShoot : MonoBehaviour
{
    [SerializeField] float shootInterval = 2.35f;
    public float ShootInterval { get => shootInterval; set { shootInterval = value; } }
    [SerializeField] float shootDistance = 10;
    public float ShootDistance { get => shootDistance; set { shootDistance = value; } }
    [Space]
    [SerializeField] bool allowRotation = true;
    [SerializeField, Tooltip("Will also rotate after shooting")] float rotateInterval = 4;
    public float RotateInterval { get => rotateInterval; set { rotateInterval = value; } }
    [SerializeField] float rotateAfterShootInterval = 2.1f;
    public float RotateAfterShootInterval { get => rotateAfterShootInterval; set { rotateAfterShootInterval = value; } }

    [HideInInspector] public int ShooterIndex = -1;
    float shootTimer = 0;
    float shootVerificationInterval = 0.1f;
    float shootVerificationTimer = 0f;
    float rotateTimer = 0f;
    float rotateAfterShootTimer = 0f;
    bool isRotating;
    Transform player;
    List<BossShooter> shooters = new();

    void Start()
    {
        player = FindObjectOfType<PlayerMove>().transform;
        GetComponentsInChildren<BossShooter>(true, shooters);
    }

    void Update()
    {
        if(shootVerificationTimer >= shootVerificationInterval)
        {
            shootVerificationTimer = 0;
            
            if(shootTimer >= shootInterval && !isRotating && DotVerification(out BossShooter bossShooter))
            {
                Shoot(bossShooter);
                shootTimer = 0;
                rotateTimer = 0;
            }
        }

        //if(rotateTimer > rotateInterval + 1)
        //    isRotating = false;

        if(rotateTimer >= rotateInterval && !isRotating)
        {
            CallRotare();
        }
        else if (rotateAfterShootTimer >= rotateAfterShootInterval && !isRotating)
        {
            CallRotare();
        }

        shootTimer += Time.deltaTime;
        shootVerificationTimer += Time.deltaTime;
        rotateTimer += Time.deltaTime;
        rotateAfterShootTimer += Time.deltaTime;
    }

    Vector3 rotationValor = new(0, 0, 30);
    void CallRotare()
    {
        rotateAfterShootTimer = 0;
        rotateTimer = 0;        

        if (Vector2.SqrMagnitude(player.position - transform.position) > shootDistance * shootDistance)
            return;

        if (!allowRotation) return;
        isRotating = true;

        transform.DORotate(rotationValor, .3f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetRelative().OnComplete(
                () => isRotating = false);
    }

    bool DotVerification(out BossShooter bossShooter)
    {
        bossShooter = null;        

        if (Vector2.SqrMagnitude(player.position - transform.position) > shootDistance * shootDistance)
            return false;

        float bestDotSoFar = -1;
        for (int i = 0; i < shooters.Count; i++)
        {
            if (!shooters[i].gameObject.activeInHierarchy)
            {
                shooters.Remove(shooters[i]);
                i--;
                continue;
            }
            if (bossShooter == null)
            {
                bossShooter = shooters[i];
                bestDotSoFar = Vector2.Dot(shooters[i].transform.up, (player.position - shooters[i].transform.position).normalized);
                continue;
            }            

            float dot = Vector2.Dot(shooters[i].transform.up, (player.position - shooters[i].transform.position).normalized);
            if (dot > bestDotSoFar && !shooters[i].IsShooting)
            {  
                bestDotSoFar = dot;
                bossShooter = shooters[i];
            }
        }
        
        if(ShooterIndex < 0)
            ShooterIndex = shooters.Count;

        if (bestDotSoFar >= 0.95f)
            return true;
        else
            return false;
    }

    public void Shoot(BossShooter bossShooter)
    {
        bossShooter?.StartShoot();
        ShooterIndex = shooters.IndexOf(bossShooter);
        rotateAfterShootTimer = 0;
        rotateTimer = 0;
    }

    private void OnDisable()
    {
        DOTween.KillAll();
    }
}
