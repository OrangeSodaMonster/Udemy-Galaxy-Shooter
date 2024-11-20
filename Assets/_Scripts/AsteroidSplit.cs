using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;


public class AsteroidSplit : MonoBehaviour
{
    //[SerializeField] bool drawGizmos;
    public bool IsObjective = false;
    [SerializeField] GameObject asteroidToSplitInto;
    public GameObject AsteroidToSplitInto => asteroidToSplitInto;
    [SerializeField] float spawnDistance = 1f;
    [SerializeField] float baseNewDirectionAngle = 20f;
    [SerializeField] float newDirectionVariance = 5f;
    [SerializeField] float newSpeedVarPerc = 10f;

    Vector3 moveDirection;
    float moveSpeed;
    Transform objParent;
    private void OnEnable()
    {
        StartCoroutine(GetVelocity());
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
        }

        if(transform.parent.GetComponent<ObjectiveSpawnArrow>() != null)
        {
            objParent = transform.parent;
        }

        if (IsObjective && !GameManager.IsSurvival)
            SetObjChildren(objParent);
    }

    public void SetObjChildren(Transform objParent = null)
    {
        if (transform.childCount >= 3) return;
        if (objParent != null) this.objParent = objParent;

        for (int i = 0; i < 3; i++)
        {
            GameObject child = Instantiate(asteroidToSplitInto, transform);
            if(child.TryGetComponent(out AsteroidSplit split) && split.IsObjective)
            {
                split.SetObjChildren(this.objParent);
            }
            child.SetActive(false);
        }
    }

    Vector3[] spawnPos = new Vector3[3];
    Vector3[] newMoveDir = new Vector3[3];
    public void Split(int extraDamage)
    {
        if (IsObjective)
        {
            SplitAsteroid(extraDamage);
            return;
        }

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

    public void SplitAsteroid(int extraDamage)
    {
        int[] damageToApply = CalculateDamageToApply(ref extraDamage);

        if (!GameManager.IsSurvival)
            SplitAsteroidParented(damageToApply);
        else
            SplitAsteroidSurvival(damageToApply);
    }

    void SplitAsteroidSurvival(int[] damageToApply)
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject child = ObjPools.PoolObjective(asteroidToSplitInto);
            child.transform.position = transform.position + (Quaternion.AngleAxis((-120+120*i), Vector3.forward) * moveDirection.normalized) * spawnDistance;
            child.transform.parent = transform.parent;
            child.SetActive(true);

            if (damageToApply[i] > 0 && child.TryGetComponent(out EnemyHP hp))
            {
                hp.ChangeHP(-Mathf.Abs(damageToApply[i]));
            }
        }
    }

    void SplitAsteroidParented(int[] damageToApply)
    {
        for (int i = 0; i < 3; i++)
        {
            //Debug.Log(transform.childCount);
            GameObject child = transform.GetChild(0).gameObject;
            child.transform.position = transform.position + (Quaternion.AngleAxis((-120+120*i), Vector3.forward) * moveDirection.normalized) * spawnDistance;
            child.transform.parent = objParent;
            child.SetActive(true);

            if (damageToApply[i] > 0 && child.TryGetComponent(out EnemyHP hp))
            {
                hp.ChangeHP(-Mathf.Abs(damageToApply[i]));
            }
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
        return damageToApply;
    }
}