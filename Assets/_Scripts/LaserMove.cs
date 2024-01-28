using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LaserMove : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] bool isPlayer;
    public bool IsPlayer { get { return isPlayer; } }

    Rigidbody2D rb;
    Vector3 defaultScale = new();
    public Gradient VFXGradient;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();       
        defaultScale = transform.localScale;
    }

    private void OnEnable()
    {
        rb.velocity = (transform.up) * moveSpeed;
        transform.localScale = defaultScale;
    }


    void FixedUpdate()
    {
        //transform.Translate(moveSpeed*Time.deltaTime*transform.up, Space.World);
        transform.up = rb.velocity;
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<BlackHolePull>() != null || collision.GetComponent<BlackHoleHorizon>() != null) return;

        if (isPlayer)
        {
            GameObject vfx = VFXPoolerScript.Instance.LaserVFXPooler.GetPooledGameObject();
            vfx.transform.position = collision.ClosestPoint(transform.position);
            vfx.GetComponent<VisualEffect>().SetGradient("ColorOverLife", VFXGradient);
            vfx.SetActive(true);

            if(collision.TryGetComponent(out EnemyHP enemyHP))
            {
                if(enemyHP.IsAsteroid)
                    AudioManager.Instance.AsteroidHitSound.PlayFeedbacks();
            }
        }
        else
        {
            GameObject vfx = VFXPoolerScript.Instance.ProjectileVFXPooler.GetPooledGameObject();
            vfx.transform.position = collision.ClosestPoint(transform.position);
            vfx.SetActive(true);

            AudioManager.Instance.EnemyProjectileDestructionSound.PlayFeedbacks();
        }

        DestroySequence();
    }

    public void DestroySequence()
    {
        gameObject.SetActive(false);
    }

}