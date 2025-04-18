using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriftRotare : MonoBehaviour
{
	[SerializeField] Vector2 minMaxAbsAngularSpeed = new Vector2(1,2);
	[SerializeField] bool randomSpinDirection = true;

    float angularSpeed;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {   
        angularSpeed = Random.Range(minMaxAbsAngularSpeed.x, minMaxAbsAngularSpeed.y);
        if(randomSpinDirection )
            angularSpeed *= Mathf.Sign(Random.Range(-1,1));

        if (rb != null)
        {
            rb.angularVelocity = angularSpeed;
        }
    }

    private void Update()
    {
        if (rb != null) return;
        
        transform.Rotate(Vector3.forward, angularSpeed * Time.deltaTime);
    }
}