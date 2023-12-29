using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriftRotare : MonoBehaviour
{
	[SerializeField] Vector2 minMaxAbsAngularSpeed = new Vector2(1,2);

    float angularSpeed;

    private void Start()
    {
        angularSpeed = Random.Range(minMaxAbsAngularSpeed.x, minMaxAbsAngularSpeed.y);
        angularSpeed *= Mathf.Sign(Random.Range(-1,1));
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward, angularSpeed * Time.deltaTime);
    }
}