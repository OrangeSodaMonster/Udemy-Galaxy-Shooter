using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SnakeBossPart : MonoBehaviour
{
    SnakeBossMain snakeBossMain;
    [SerializeField] bool isTail = false;

    private void Awake()
    {
        snakeBossMain = GetComponentInParent<SnakeBossMain>();
    }

    private void OnDisable()
    {
        snakeBossMain.RemovePart(GetComponent<SplineAnimate>());
    }

}
