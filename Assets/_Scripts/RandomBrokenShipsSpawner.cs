using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBrokenShipsSpawner : MonoBehaviour
{
    [SerializeField] List<EnemyHP> possibleBrokenShips;
    [SerializeField, MinValue(1)] int numberOfSpawns = 1;
    [SerializeField, Tooltip("If false will now duplicate objectives")] bool getRandomSpawns = true;

    List<Transform> selectedSpots = new List<Transform>();

    private void OnValidate()
    {
        if (transform.childCount < numberOfSpawns)
        {
            Debug.Log("<color=orange>Number of possible spots too low!</color>");
        }
    }

    private void Awake()
    {
        // Cria lista com os pontos possíveis
        List<Transform> possibleSpots = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            possibleSpots.Add(transform.GetChild(i));
        }

        // Seleciona os pontos        
        for (int i = 0; i < numberOfSpawns; i++)
        {
            int randomIndex = Random.Range(0, possibleSpots.Count);
            selectedSpots.Add(possibleSpots[randomIndex]);
            possibleSpots.RemoveAt(randomIndex);
        }

        // Cria objetivos nos pontos selecionados (objetivo escolhido aleatoriamente da lista)
        for (int i = 0; i < selectedSpots.Count; i++)
        {
            int objIndex = 0;
            if (getRandomSpawns)
            {
                objIndex = Random.Range(0, possibleBrokenShips.Count);
            }

            GameObject obj = Instantiate(possibleBrokenShips[objIndex].gameObject);
            obj.transform.position = selectedSpots[i].position;

            if (!getRandomSpawns && possibleBrokenShips.Count > 0)
            {
                possibleBrokenShips.RemoveAt(0);
            }
            else if (possibleBrokenShips.Count == 0)
            {
                Debug.Log("<color=orange>Not enough objectives to not repeat!</color>");
            }
        }
    }
}
