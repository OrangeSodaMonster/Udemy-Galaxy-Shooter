using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyProjectileShoot : MonoBehaviour
{
    [SerializeField] float baseShootCD = 4;
    [SerializeField] float shootCDVariation = 0.5f;
    [SerializeField] Transform projectileOrigin;
    [SerializeField] VisualEffect preShootVFX;
    [SerializeField] float preShootVFXTimePrior = 3;
    [SerializeField] LayerMask raycastLayers;

    float shootCD;
	
    void OnEnable()
    {
        shootCD = baseShootCD;
        preShootVFX.gameObject.SetActive(false);

        StartCoroutine(Shoot());
    }

    private void OnDisable()
    {
        AudioManager.Instance.StopEnemyCharge(gameObject.GetHashCode());
        StopAllCoroutines();
    }

    void Update()
    {
        if(GameStatus.IsGameover || GameStatus.IsStageClear)
            StopAllCoroutines();
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootCD - preShootVFXTimePrior);

            preShootVFX.gameObject.SetActive(true);
            AudioManager.Instance.PlayEnemyCharge(gameObject.GetHashCode());

            yield return new WaitForSeconds(preShootVFXTimePrior);

            //Instantiate(projectile, projectileOrigin.position, transform.rotation, transform.parent);
            GameObject projectile = EnemyPoolRef.s_projectilePool.GetPooledGameObject();
            projectile.transform.SetLocalPositionAndRotation(projectileOrigin.position, transform.rotation);
            projectile.GetComponent<LaserMove>().SourceHash = gameObject.GetHashCode();            
            projectile.SetActive(true);
            if(InsideCheck(out Collider2D inside))
            {
                projectile.GetComponent<LaserMove>().IgnoreCollision(inside, true);
            }

            preShootVFX.gameObject.SetActive(false);
            shootCD = Random.Range(-shootCDVariation, shootCDVariation) + baseShootCD;

            AudioManager.Instance.EnemyFireSound.PlayFeedbacks();
        } 
    }

    bool InsideCheck(out Collider2D inside)
    {
        RaycastHit2D hit = Physics2D.Raycast(projectileOrigin.position, projectileOrigin.rotation * Vector2.up, 20f, raycastLayers);
        int hitHash = 0;
        if (hit.collider)
        {
            //Debug.Log($"Hit1: {hit.collider.name}");
            hitHash = hit.collider.GetHashCode();
            RaycastHit2D newHit = Physics2D.Raycast(hit.point, projectileOrigin.rotation * Vector2.up, 20f);
            //Debug.Log($"Hit2: {newHit.collider.name}");

            if (newHit.collider.GetHashCode() != hitHash || !newHit)
            {
                inside = hit.collider;
                return true;
            }
        }
        inside = null;
        return false;
    }
}