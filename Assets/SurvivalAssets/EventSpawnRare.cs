using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSpawnRare : MonoBehaviour
{    
    [Tooltip("Asteroids only")]
    [HorizontalGroup("0", 0.12f), PreviewField(50, Alignment = ObjectFieldAlignment.Left), HideLabel]
    public GameObject Spawn;
    [VerticalGroup("0/1"), LabelWidth(10), LabelText(""), ReadOnly]
    public string Name;
    [SerializeField, Range(0, 10),VerticalGroup("0/1")] float TimeToSpawn = 0;
    [SerializeField] UnityEvent OnSpawn = new UnityEvent();

    [Button]
    public void CallSpawnRare()
    {
        StartCoroutine(SpawnRare());
    }

    IEnumerator SpawnRare()
    {
        yield return new WaitForSeconds(TimeToSpawn);

        RareSpawnScript.Instance.SpawnRare(Spawn.gameObject);
        OnSpawn.Invoke();
    }

    private void OnValidate()
    {
        Name = Spawn.name;

        if(Spawn.GetComponent<AsteroidMove>() == null)
        {
            Debug.LogWarning($"Not a valid rare");
            Spawn = null;
        }
    }
}
