using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class AsteroidDeathVFX : MonoBehaviour
{
    [SerializeField] float vfxScale = 1f;
    [SerializeField] Gradient vfxColor;

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
        GameObject vfx = VFXPoolerScript.Instance.AsteroidDustVFXPooler.GetPooledGameObject();
        vfx.transform.position = transform.position;
        vfx.transform.localScale = vfxScale * Vector3.one;
        vfx.GetComponent<VisualEffect>().SetGradient("ColorVariation", vfxColor);
        vfx.SetActive(true);
    }
}