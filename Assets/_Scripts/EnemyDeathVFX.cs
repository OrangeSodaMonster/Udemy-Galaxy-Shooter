using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyDeathVFX : MonoBehaviour
{
    [SerializeField] bool isAsteroid;
    [SerializeField] float vfxScale = 1f;
    [GradientUsage(true)]
    [SerializeField] Gradient vfxColor;
    [GradientUsage(true)]
    [SerializeField] Gradient vfxRingsColor;

    private void OnEnable()
    {
        GetComponent<EnemyHP>().Died += CallDeathVFX;
    }
    private void OnDisable()
    {
        GetComponent<EnemyHP>().Died -= CallDeathVFX;
    }

    public void CallDeathVFX()
    {
        if (isAsteroid)
        {
            GameObject vfx = VFXPoolerScript.Instance.AsteroidDustVFXPooler.GetPooledGameObject();
            vfx.transform.position = transform.position;
            vfx.transform.localScale = vfxScale * Vector3.one;
            vfx.GetComponent<VisualEffect>().SetGradient("ColorVariation", vfxColor);
            vfx.GetComponent<VisualEffect>().SetGradient("ShockParticlesColor", vfxRingsColor);
            vfx.SetActive(true);
        }
        else
        {
            GameObject vfx = VFXPoolerScript.Instance.EnemyExplosionVFXPooler.GetPooledGameObject();
            vfx.transform.position = transform.position;
            vfx.transform.localScale = vfxScale * Vector3.one;
            vfx.GetComponent<VisualEffect>().SetGradient("ColorVariation", vfxColor);
            vfx.GetComponent<VisualEffect>().SetGradient("RingsColor", vfxRingsColor);
            vfx.SetActive(true);
        }
        
    }
}