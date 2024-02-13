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

    float shootCD;
	
    void OnEnable()
    {
        shootCD = baseShootCD;
        preShootVFX.gameObject.SetActive(false);

        StartCoroutine(Shoot());
    }

    void Update()
    {
        if(GameStatus.IsGameover)
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
            preShootVFX.gameObject.SetActive(false);
            shootCD = Random.Range(-shootCDVariation, shootCDVariation) + baseShootCD;

            AudioManager.Instance.EnemyFireSound.PlayFeedbacks();
        } 
    }

    private void OnDisable()
    {
        AudioManager.Instance.StopEnemyCharge(gameObject.GetHashCode());
        StopAllCoroutines();
    }
}