using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    [SerializeField] int startingBombs = 1;
    public static int BombAmount = 1;
    public static int MaxBombs = 3;

    [SerializeField] InputSO Input;
    [SerializeField] float radius;
    [SerializeField] float visualDuration = .2f;
    [SerializeField] LayerMask layersToHit;
    [SerializeField] int damage = 100;
    [SerializeField] float coolDown = 3;

    float timeSinceUsedBomb = float.MaxValue;
    RaycastHit2D[] hits;
    EnemyHP enemyHP;

    void Start()
    {
        BombAmount = startingBombs;
        TurnOffSpriteRenderer();
    }

    void Update()
    {
        if (Input.IsSpecialing && BombAmount > 0 && timeSinceUsedBomb >= coolDown)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            Invoke(nameof(BombScript.TurnOffSpriteRenderer), visualDuration);

            hits = Physics2D.CircleCastAll(transform.position, radius, Vector2.zero, 0, layersToHit);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.TryGetComponent(out EnemyHP enemyHP))
                {
                    enemyHP.ChangeHP(-Mathf.Abs(damage));
                    //Debug.Log(enemyHP.name + " " + damage + " damage");
                }
                else if(hit.transform.TryGetComponent(out LaserMove enemyProjectile))
                {
                    enemyProjectile.DestroySequence();
                }
            }

            BombAmount--;
            timeSinceUsedBomb = 0;
        }

        timeSinceUsedBomb += Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void TurnOffSpriteRenderer()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
}