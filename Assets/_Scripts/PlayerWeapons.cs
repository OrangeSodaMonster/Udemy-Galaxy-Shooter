using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    public bool IsSpreadShotActivated = false;
    public bool IsSideShotActivated = false;
    public bool IsBackShotActivated = false;

    [SerializeField] InputSO Input;
    [SerializeField] GameObject basicLaser;
    [SerializeField] Transform[] frontLaserParents;
    [SerializeField] Transform[] spreadLaserParents;
    [SerializeField] Transform[] sideLaserParents;
    [SerializeField] Transform[] backLaserParents;
    [SerializeField] float frontLaserCD = 0.5f;
    [SerializeField] float spreadLaserCD = 0.5f;
    [SerializeField] float sideLaserCD = 0.5f;
    [SerializeField] float backLaserCD = 0.5f;

    [HideInInspector] public float currentLaserCDMod = 1;

    void Update()
    {
        FrontLasersShoot();
        SpreadLaserShoot();
        SideLasersShoot();
        BackLasersShoot();
    }

    float timeSinceFrontLaserShoot = float.MaxValue;
    bool justFiredLeftFrontLaser = false;
    void FrontLasersShoot()
    {
        if ((Input.IsAutoFire | Input.IsFiring) & timeSinceFrontLaserShoot >= (frontLaserCD/2)/currentLaserCDMod & !justFiredLeftFrontLaser)
        {
            Instantiate(basicLaser, transform.position + transform.TransformDirection(frontLaserParents[0].position),
                transform.rotation * frontLaserParents[0].rotation, frontLaserParents[0]);
            timeSinceFrontLaserShoot = 0;
            justFiredLeftFrontLaser = true;
        }
        else if ((Input.IsAutoFire | Input.IsFiring) & timeSinceFrontLaserShoot > (frontLaserCD/2)/currentLaserCDMod & justFiredLeftFrontLaser)
        {
            Instantiate(basicLaser, transform.position + transform.TransformDirection(frontLaserParents[1].position),
                transform.rotation * frontLaserParents[1].rotation, frontLaserParents[1]);
            timeSinceFrontLaserShoot = 0;
            justFiredLeftFrontLaser = false;
        }
        timeSinceFrontLaserShoot += Time.deltaTime;
    }

    float timeSinceSpreadLaserShoot = float.MaxValue;
    bool justFiredRightSpreadLaser = false;
    void SpreadLaserShoot()
    {
        if (IsSpreadShotActivated & (Input.IsAutoFire | Input.IsFiring) & timeSinceSpreadLaserShoot >= (spreadLaserCD/2)/currentLaserCDMod & !justFiredRightSpreadLaser)
        {
            Instantiate(basicLaser, transform.position + transform.TransformDirection(spreadLaserParents[1].position),
                transform.rotation * spreadLaserParents[1].rotation, spreadLaserParents[1]);
            timeSinceSpreadLaserShoot = 0;
            justFiredRightSpreadLaser = true;
        }
        else if (IsSpreadShotActivated & (Input.IsAutoFire | Input.IsFiring) & timeSinceSpreadLaserShoot >= (spreadLaserCD/2)/currentLaserCDMod & justFiredRightSpreadLaser)
        {
            Instantiate(basicLaser, transform.position + transform.TransformDirection(spreadLaserParents[0].position),
                transform.rotation * spreadLaserParents[0].rotation, spreadLaserParents[0]);
            timeSinceSpreadLaserShoot = 0;
            justFiredRightSpreadLaser = false;
        }
        timeSinceSpreadLaserShoot += Time.deltaTime;
    }

    float timeSinceSideLaserShoot = float.MaxValue;
    bool justFiredLeftSideLaser = false;
    void SideLasersShoot()
    {
        if (IsSideShotActivated & (Input.IsAutoFire | Input.IsFiring) & timeSinceSideLaserShoot >= (sideLaserCD/2)/currentLaserCDMod & !justFiredLeftSideLaser)
        {
            Instantiate(basicLaser, transform.position + transform.TransformDirection(sideLaserParents[0].position),
                transform.rotation * sideLaserParents[0].rotation, sideLaserParents[0]);
            timeSinceSideLaserShoot = 0;
            justFiredLeftSideLaser = true;
        }
        else if (IsSideShotActivated & (Input.IsAutoFire | Input.IsFiring) & timeSinceSideLaserShoot > (sideLaserCD/2)/currentLaserCDMod & justFiredLeftSideLaser)
        {
            Instantiate(basicLaser, transform.position + transform.TransformDirection(sideLaserParents[1].position),
                transform.rotation * sideLaserParents[1].rotation, sideLaserParents[1]);
            timeSinceSideLaserShoot = 0;
            justFiredLeftSideLaser = false;
        }
        timeSinceSideLaserShoot += Time.deltaTime;
    }

    float timeSinceBackLaserShoot = float.MaxValue;
    bool justFiredRightBackLaser = false;
    void BackLasersShoot()
    {
        if (IsBackShotActivated & (Input.IsAutoFire | Input.IsFiring) & timeSinceBackLaserShoot >= (backLaserCD/2)/currentLaserCDMod & !justFiredRightBackLaser)
        {
            Instantiate(basicLaser, transform.position + transform.TransformDirection(backLaserParents[1].position),
                transform.rotation * backLaserParents[1].rotation, backLaserParents[1]);
            timeSinceBackLaserShoot = 0;
            justFiredRightBackLaser = true;
        }
        else if (IsBackShotActivated & (Input.IsAutoFire | Input.IsFiring) & timeSinceBackLaserShoot >= (backLaserCD/2)/currentLaserCDMod & justFiredRightBackLaser)
        {
            Instantiate(basicLaser, transform.position + transform.TransformDirection(backLaserParents[0].position),
                transform.rotation * backLaserParents[0].rotation, backLaserParents[0]);
            timeSinceBackLaserShoot = 0;
            justFiredRightBackLaser = false;
        }
        timeSinceBackLaserShoot += Time.deltaTime;
    }
}