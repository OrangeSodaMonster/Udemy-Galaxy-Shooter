using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneControl : MonoBehaviour
{
    public bool isDrone1Enabled;
    public bool isDrone2Enabled;
    public bool isDrone3Enabled;

    [SerializeField] DroneAttackScript drone1;
    [SerializeField] DroneAttackScript drone2;
    [SerializeField] DroneAttackScript drone3;
    [SerializeField] float rotationSpeed;
    Transform player;

    void Start()
    {
        player = FindObjectOfType<PlayerMove>().transform;
    }

    void Update()
    {
        transform.position = player.position;
        transform.Rotate(rotationSpeed * Time.deltaTime * Vector3.forward);

        drone1.gameObject.SetActive(isDrone1Enabled);
        drone2.gameObject.SetActive(isDrone2Enabled);
        drone3.gameObject.SetActive(isDrone3Enabled);
    }
}