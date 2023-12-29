using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileShoot : MonoBehaviour
{
    [SerializeField] float baseShootCD = 4;
    [SerializeField] float shootCDVariation = 0.5f;
    [SerializeField] GameObject projectile;
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
            yield return new WaitForSeconds(baseShootCD);

            Instantiate(projectile, projectileOrigin.position, transform.rotation, transform.parent);
            shootCD = Random.Range(-shootCDVariation, shootCDVariation) + baseShootCD;
            
        } while (true);
    }
}