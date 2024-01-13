using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileShoot : MonoBehaviour
{
    [SerializeField] float baseShootCD = 4;
    [SerializeField] float shootCDVariation = 0.5f;
    [SerializeField] Transform projectileOrigin;

    float shootCD;
	
    void Start()
    {
        shootCD = baseShootCD;

        StartCoroutine(Shoot());
    }


    IEnumerator Shoot()
    {
        do
        {
            yield return new WaitForSeconds(shootCD);

            //Instantiate(projectile, projectileOrigin.position, transform.rotation, transform.parent);
            GameObject projectile = EnemyPoolRef.s_projectilePool.GetPooledGameObject();
            projectile.transform.SetLocalPositionAndRotation(projectileOrigin.position, transform.rotation);
            projectile.SetActive(true);
            shootCD = Random.Range(-shootCDVariation, shootCDVariation) + baseShootCD;
            
        } while (true);
    }
}