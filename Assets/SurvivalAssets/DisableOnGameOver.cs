using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnGameOver : MonoBehaviour
{
    [SerializeField] List<GameObject> objectsToDisable;

    private void Awake()
    {
        GameStatus.GameOver += HideObjects;
    }

    private void OnDestroy()
    {
        GameStatus.GameOver -= HideObjects;
    }

    void HideObjects()
    {
        for(int i = 0; i < objectsToDisable.Count; i++)
        {
            objectsToDisable[i].gameObject.SetActive(false);
        }
    }
}
