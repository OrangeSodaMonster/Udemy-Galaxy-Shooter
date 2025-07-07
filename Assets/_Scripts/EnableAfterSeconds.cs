using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAfterSeconds : MonoBehaviour
{

    [SerializeField] float time = 1.5f;        
    [SerializeField] List<GameObject> objects;        

    private void Awake()
    {
        GameStatus.OnGameStart.AddListener(() => StartCoroutine(Enabler()));
    }

    IEnumerator Enabler()
    {
        yield return new WaitForSeconds(time);

        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].SetActive(true);
        }
    }
}
