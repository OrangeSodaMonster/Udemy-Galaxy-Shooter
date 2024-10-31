using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBObjectiveDealer : MonoBehaviour
{
    [SerializeField] ObjectivePointer objParent;
    [SerializeField] List<EnemyHP> possibleObjectives;
    [SerializeField, MinValue(1)] int numberOfObjectives = 1;
    [SerializeField, Tooltip("If false will now duplicate objectives")] bool getRandomObjectives = true;
    [Space]
    [SerializeField] List<Transform> sentinels = new List<Transform>();
    [SerializeField] float radius = 3.0f;
    [SerializeField] bool allSentinelsInTheSameObjective = true;

    List<Transform> selectedSpots = new List<Transform>();

    private void OnValidate()
    {
        if(transform.childCount < numberOfObjectives)
        {
            Debug.Log("<color=orange>Number of possible spots too low!</color>");
        }
    }

    private void Awake()
    {
        // Cria lista com os pontos possíveis
        List<Transform> possibleSpots = new List<Transform>();
        for(int i = 0; i < transform.childCount; i++)
        {
            possibleSpots.Add(transform.GetChild(i));
        }

        // Seleciona os pontos        
        for (int i = 0; i < numberOfObjectives; i++)
        {
            int randomIndex = Random.Range(0, possibleSpots.Count);
            selectedSpots.Add(possibleSpots[randomIndex]);
            possibleSpots.RemoveAt(randomIndex);
        }

        // Cria objetivos nos pontos selecionados (objetivo escolhido aleatoriamente da lista)
        for(int i = 0;i < selectedSpots.Count; i++)
        {
            Transform parent = Instantiate(objParent.transform, selectedSpots[i].position, Quaternion.identity, transform);

            int objIndex = 0;
            if (getRandomObjectives)
            {
                objIndex = Random.Range(0, possibleObjectives.Count);
            }            
                
            //GameObject obj = Instantiate(possibleObjectives[objIndex].gameObject, parent);
            Instantiate(possibleObjectives[objIndex].gameObject, parent);

            if (!getRandomObjectives && possibleObjectives.Count > 0)
            {
                possibleObjectives.RemoveAt(0);
            }
            else if (possibleObjectives.Count == 0)
            {
                Debug.Log("<color=orange>Not enouth objectives to not repeat!</color>");
            }
        }

        PositionSentinels();
    }

    void PositionSentinels()
    {
        if (sentinels.Count == 0) return;

        float originalAngle = Random.Range(0, 360);
        float angleVariation = 360/sentinels.Count;       

        Vector2 pos = selectedSpots[Random.Range(0, selectedSpots.Count)].position;

        for (int i = 0; i < sentinels.Count; i++)
        {
            float angle = (originalAngle + angleVariation * i) * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
            direction *= radius;

            sentinels[i].position = pos + direction;

            if (!allSentinelsInTheSameObjective)
            {
                pos = selectedSpots[Random.Range(0, selectedSpots.Count)].position;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        foreach (Transform t in transform)
        {
            Gizmos.color = Color.green;

            Gizmos.DrawWireSphere(t.position, radius);
        }
    }
}
