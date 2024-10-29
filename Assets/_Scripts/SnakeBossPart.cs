using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SnakeBossPart : MonoBehaviour
{
    SnakeBossMain snakeBossMain;
    public bool isTail = false;

    private void Awake()
    {
        snakeBossMain = GetComponentInParent<SnakeBossMain>();
    }

    private void OnDisable()
    {
        if (!GameStatus.IsRestart && !GameStatus.IsStageClear)
            snakeBossMain.RemovePart(GetComponent<SplineAnimate>(), isTail);
    }

}
