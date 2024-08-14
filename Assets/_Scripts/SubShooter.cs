using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SubShooter : MonoBehaviour
{
    [SerializeField, HorizontalGroup("1"), LabelWidth(120)] bool playChargeSound = false;
    [SerializeField, HorizontalGroup("1"), LabelWidth(100)] bool playFastCharge = false;
    [field: SerializeField] public GameObject projectilePref { get; private set; }
    [SerializeField] Transform projectileOrigin;
    public Transform ProjectileOrigin { get { return projectileOrigin; }}
    [SerializeField] VisualEffect preShootVFX;
    [SerializeField] float preShootVFXTimePrior = 2;
    [SerializeField] LayerMask raycastLayers;

    [HideInInspector] public bool IsShooting;
    PoolRefs poolRefs;

    void OnEnable()
    {
        preShootVFX.gameObject.SetActive(false);        
    }

    private void Start()
    {
        poolRefs = FindObjectOfType<PoolRefs>();
    }

    private void OnDisable()
    {
        if(playChargeSound)
            AudioManager.Instance.StopEnemyCharge(gameObject.GetHashCode());
        StopAllCoroutines();
    }

    [Button]
    public void StartShoot()
    {
        StartCoroutine(Shoot());
    }


    IEnumerator Shoot()
    {        
        IsShooting = true;
        preShootVFX.gameObject.SetActive(true);
        if (playChargeSound && !playFastCharge)
            AudioManager.Instance.PlayEnemyCharge(gameObject.GetHashCode());
        else if (playChargeSound && playFastCharge)
            AudioManager.Instance.PlayEnemyChargeFast(gameObject.GetHashCode());

        yield return new WaitForSeconds(preShootVFXTimePrior);

        GameObject projectile;
        if (poolRefs.Poolers.ContainsKey(projectilePref))
            projectile = poolRefs.Poolers[projectilePref].GetPooledGameObject();
        else
            projectile = Instantiate(projectilePref); // Add pool

        projectile.transform.SetLocalPositionAndRotation(projectileOrigin.position, transform.rotation);
        projectile.GetComponent<LaserMove>().SourceHash = gameObject.GetHashCode();
        projectile.SetActive(true);

        preShootVFX.gameObject.SetActive(false);
        IsShooting = false;

        AudioManager.Instance.EnemyFireSound.PlayFeedbacks();        
    }
}
