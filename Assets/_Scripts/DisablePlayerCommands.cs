using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePlayerCommands : MonoBehaviour
{
    PlayerMove playerMove;
    PlayerLasers playerLasers;
    BombScript bombScript;
    ThrusterControl[] thrusterControls;

    Collider2D[] colliders;
    Rigidbody2D rb;

    public static DisablePlayerCommands Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        playerMove = FindObjectOfType<PlayerMove>();
        playerLasers = FindObjectOfType<PlayerLasers>();
        bombScript = FindObjectOfType<BombScript>();
        thrusterControls = FindObjectsOfType<ThrusterControl>();

        Transform player = FindObjectOfType<PlayerMove>().transform;
        colliders = player.GetComponentsInChildren<Collider2D>();
        rb = player.GetComponent<Rigidbody2D>();
    }

    public void SetCommands(bool enable)
    {
        if (playerMove != null) playerMove.enabled = enable;
        if (playerLasers != null) playerLasers.enabled = enable;
        if (bombScript != null) bombScript.enabled = enable;

        if(thrusterControls.Length > 0)
            foreach (var thrusterControl in thrusterControls)
                thrusterControl.enabled = enable;
    }

    public void SetColliders(bool enable)
    {
        if (colliders.Length > 0)
            foreach (var col in colliders)
                col.enabled = enable;
    }

    public void SetRB(bool enable)
    {
        if (!enable)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
        }        

        rb.isKinematic = !enable;
    }
}