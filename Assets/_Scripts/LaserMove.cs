using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
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
    [HideInInspector] public int SourceHash = 0;

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
        float anglesToRotate = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg - 90f;
        rb.MoveRotation(anglesToRotate);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<BlackHolePull>() != null || collision.GetComponent<BlackHoleHorizon>() != null ||
            (!isPlayer && (collision.gameObject.GetHashCode() == SourceHash || collision.TryGetComponent(out EnemyHP enemyHP) && !enemyHP.IsAsteroid)))
            return;
       
        DestroySequence(collision);
    }

    public void DestroySequence(Collider2D collision)
    {
        if (isPlayer)
        {
            GameObject vfx = VFXPoolerScript.Instance.LaserVFXPooler.GetPooledGameObject();
            vfx.transform.position = collision.ClosestPoint(transform.position);
            vfx.GetComponent<VisualEffect>().SetGradient("ColorOverLife", VFXGradient);
            vfx.SetActive(true);

            if (collision.TryGetComponent(out EnemyHP enemyHP))
            {
                if (enemyHP.IsAsteroid)
                    AudioManager.Instance.PlayAsteroidHitSound();
                else
                    AudioManager.Instance.EnemyHitSound.PlayFeedbacks();
            }
        }
        else
        {
            GameObject vfx = VFXPoolerScript.Instance.ProjectileVFXPooler.GetPooledGameObject();
            vfx.transform.position = collision.ClosestPoint(transform.position);
            vfx.SetActive(true);

            AudioManager.Instance.EnemyProjectileDestructionSound.PlayFeedbacks();

            if (collision.TryGetComponent(out PlayerHP playerHP))
            {
                AudioManager.Instance.PlayerHitSound.PlayFeedbacks();
            }
        }

        gameObject.SetActive(false);
    }

    public void DestroySequence()
    {
        if (isPlayer)
        {
            GameObject vfx = VFXPoolerScript.Instance.LaserVFXPooler.GetPooledGameObject();
            vfx.transform.position = transform.position;
            vfx.GetComponent<VisualEffect>().SetGradient("ColorOverLife", VFXGradient);
            vfx.SetActive(true);            
        }
        else
        {
            GameObject vfx = VFXPoolerScript.Instance.ProjectileVFXPooler.GetPooledGameObject();
            vfx.transform.position = transform.position;
            vfx.SetActive(true);

            AudioManager.Instance.EnemyProjectileDestructionSound.PlayFeedbacks();            
        }

        gameObject.SetActive(false);
    }

    public void DestroySilently()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if(collIgnored != null)
            IgnoreCollision(collIgnored, false);
    }

    Collider2D collIgnored = null;
    public void IgnoreCollision(Collider2D coll, bool ignore)
    {
        collIgnored = coll;
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), coll, ignore);
    }

}