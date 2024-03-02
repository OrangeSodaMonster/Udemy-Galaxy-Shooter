using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXScript : MonoBehaviour
{    
    VisualEffect effect;

    Vector3 defaultScale;

    private void Awake()
    {
        effect = GetComponent<VisualEffect>();
        defaultScale = transform.localScale;
    }

    private void OnDisable()
    {
        transform.localScale = defaultScale;
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