using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXScript : MonoBehaviour
{    
    VisualEffect effect;

    private void Awake()
    {
        effect = GetComponent<VisualEffect>();
    }

    private void OnEnable()
    {
        StartCoroutine(CheckIfSleep());
    }

    IEnumerator CheckIfSleep()
    {
        while (true)
        {
            yield return new WaitForSeconds(.1f);

            if (!effect.HasAnySystemAwake())
                gameObject.SetActive(false);
        }
    }
}