using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DisablePortalScript : MonoBehaviour
{
    [ShowInInspector, ReadOnly] float timeToDisapear = 2.5f;
    [ShowInInspector, ReadOnly] float disapearDuration = .5f;

    private void Start()
    {
        StartCoroutine(DesapearRoutine());
    }

    IEnumerator DesapearRoutine()
    {
        yield return new WaitForSeconds(timeToDisapear);

        transform.DOScale(0, disapearDuration).SetAutoKill().OnComplete(() => gameObject.SetActive(false));
    }
}
