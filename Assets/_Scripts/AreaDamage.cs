using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AreaDamage : MonoBehaviour
{
    [SerializeField] int damage = 5;
    [SerializeField] float interval = 2;
    
    Dictionary<EnemyHP, Coroutine> enemyRoroutines = new();
    bool isDamagePlayer;
    Coroutine playerRoutine;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isDamagePlayer && collision.TryGetComponent(out PlayerHP playerHP))
        {
            //Debug.Log("Player at risk");
            isDamagePlayer = true;
            playerRoutine = StartCoroutine(DamageRoutine(null, playerHP));
        }
        else if(collision.TryGetComponent(out EnemyHP enemyHP) && !enemyRoroutines.ContainsKey(enemyHP))
        {
            //Debug.Log($"{collision.name} at risk");
            enemyRoroutines.Add(enemyHP, null);
            Coroutine enemyRoutine = StartCoroutine(DamageRoutine(enemyHP, null));
            enemyRoroutines[enemyHP] = enemyRoutine;
        }

        if(collision.TryGetComponent(out SpriteOverlayScript overlay))
        {
            overlay.StartHazardOverlay();
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerHP>() != null)
        {
            //Debug.Log("Player safe");
            isDamagePlayer = false;
            StopCoroutine(playerRoutine);
        }
        else if (collision.TryGetComponent(out EnemyHP enemyHP))
        {
            //Debug.Log($"{collision.name} safe");
            StopCoroutine(enemyRoroutines[enemyHP]);
            enemyRoroutines.Remove(enemyHP);
        }

        if (collision.TryGetComponent(out SpriteOverlayScript overlay))
        {
            overlay.StopHazardOverlay();
        }
    }

    IEnumerator DamageRoutine(EnemyHP enemyHP = null, PlayerHP playerHP = null)
    {
        while (playerHP != null && isDamagePlayer || enemyHP != null && enemyRoroutines.ContainsKey(enemyHP))
        {
            yield return new WaitForSeconds(interval);

            if (playerHP != null)
                PlayerHP.ChangePlayerHP(-Mathf.Abs(damage));
            else if (enemyHP != null)
                enemyHP.ChangeHP(-Mathf.Abs(damage));
        }
    }
}