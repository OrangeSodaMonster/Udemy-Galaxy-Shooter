using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;


public class AsteroidSplit : MonoBehaviour
{
    //[SerializeField] bool drawGizmos;
    [SerializeField] GameObject asteroidToSplitInto;
    [SerializeField] float spawnDistance = 1f;
    [SerializeField] float baseNewDirectionAngle = 20f;
    [SerializeField] float newDirectionVariance = 5f;
    [SerializeField] float newSpeedVarPerc = 10f;

    Vector3 moveDirection;
    float moveSpeed;
    private void OnEnable()
    {
        StartCoroutine(GetVelocity());

        //if (drawGizmos)
        //{
           
        //    for (int i = 0; i < spawnPos.Length; i++)
        //    {
        //        //Relative to object pos
        //        spawnPos[i] = (Quaternion.AngleAxis((0+120*i), Vector3.forward) * moveDirection.normalized) * spawnDistance;

        //        float angleVariance = Mathf.Abs(Random.Range(-newDirectionVariance, newDirectionVariance) + baseNewDirectionAngle);
        //        newMoveDir[i] = (Quaternion.AngleAxis((-angleVariance + angleVariance * i), Vector3.forward) * moveDirection.normalized);
        //        float newMoveSpeed = Mathf.Abs(Random.Range(moveSpeed - moveSpeed*(newSpeedVarPerc/100), moveSpeed + moveSpeed*(newSpeedVarPerc/100)));
        //    }
            
        //}
    }

    IEnumerator GetVelocity()
    {
        yield return null;

        if (TryGetComponent(out AsteroidMove asteroidMove))
        {
            moveDirection = asteroidMove.MoveDirection;
            moveSpeed = asteroidMove.MoveSpeed;
        }
        else
        {
            moveDirection = Random.insideUnitCircle.normalized;
            moveSpeed = 0;
            //Debug.Log(moveDirection);
        }
    }

    //void Update()
    //{
    //    if (drawGizmos)
    //    {
    //        for (int i = 0; i < spawnPos.Length; i++)
    //        {
    //            Debug.DrawRay(transform.position, spawnPos[i], Color.blue);
    //            Debug.DrawRay(transform.position, newMoveDir[i], Color.cyan);
    //        }
    //    }
    //}

    Vector3[] spawnPos = new Vector3[3];
    Vector3[] newMoveDir = new Vector3[3];
    public void Split(int extraDamage)
    {
        int[] damageToApply = CalculateDamageToApply(ref extraDamage);

        for (int i = 0; i < spawnPos.Length; i++)
        {
            spawnPos[i] = transform.position + (Quaternion.AngleAxis((-120+120*i), Vector3.forward) * moveDirection.normalized) * spawnDistance;

            float angleVariance = Mathf.Abs(Random.Range(-newDirectionVariance, newDirectionVariance) + baseNewDirectionAngle);
            newMoveDir[i] = (Quaternion.AngleAxis((-angleVariance + angleVariance * i), Vector3.forward) * moveDirection.normalized);
            float newMoveSpeed = Mathf.Abs(Random.Range(moveSpeed, moveSpeed + moveSpeed*(newSpeedVarPerc/100)));

            EnemySpawner.Instance.SpawnAsteroid(asteroidToSplitInto, spawnPos[i], newMoveDir[i], newMoveSpeed, damageToApply[i]);
        }
    }    

    public void SplitParented(int extraDamage, Transform parent)
    {
        int[] damageToApply = CalculateDamageToApply(ref extraDamage);

        for (int i = 0; i < spawnPos.Length; i++)
        {
            spawnPos[i] = transform.position + (Quaternion.AngleAxis((-120+120*i), Vector3.forward) * moveDirection.normalized) * spawnDistance;

            float angleVariance = Mathf.Abs(Random.Range(-newDirectionVariance, newDirectionVariance) + baseNewDirectionAngle);
            newMoveDir[i] = (Quaternion.AngleAxis((-angleVariance + angleVariance * i), Vector3.forward) * moveDirection.normalized);
            float newMoveSpeed = Mathf.Abs(Random.Range(moveSpeed, moveSpeed + moveSpeed*(newSpeedVarPerc/100)));

            EnemySpawner.Instance.SpawnAsteroidParented(asteroidToSplitInto, spawnPos[i], newMoveDir[i], newMoveSpeed, damageToApply[i], parent);
        }
    }

    private int[] CalculateDamageToApply(ref int extraDamage)
    {
        extraDamage = Mathf.Abs(extraDamage);
        //Debug.Log(extraDamage);
        int[] damageToApply = new int[spawnPos.Length];

        int i = Random.Range(0, spawnPos.Length);        
        while (extraDamage >= 5)
        {
            damageToApply[i] += 5;
            extraDamage -= 5;

            i++;
            if(i >= spawnPos.Length)
                i = 0;
        }
        //Debug.Log(damageToApply[0] + " " + damageToApply[1] + " " + damageToApply[2]);
        return damageToApply;
    }
}