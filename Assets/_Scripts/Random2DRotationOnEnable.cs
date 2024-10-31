using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random2DRotationOnEnable : MonoBehaviour
{
    private void OnEnable()
    {
        transform.eulerAngles = new Vector3(0,0,Random.Range(0,360));
    }
}
