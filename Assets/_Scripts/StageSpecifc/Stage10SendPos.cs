using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage10SendPos : MonoBehaviour
{
    Stage10Script stage10Script;

    private void Awake()
    {
        stage10Script = transform.parent.GetComponent<Stage10Script>();
    }

    public void SendPos()
    {
        stage10Script.spawnPos = transform.position;
        stage10Script.IncreaseObjCount();
    }
}
