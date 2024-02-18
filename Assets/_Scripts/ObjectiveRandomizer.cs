using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveRandomizer : MonoBehaviour
{
    [SerializeField] AsteroidMatHolderSO mats;
    [SerializeField] ObjectiveColHolderSO colls;

    SpriteRenderer spriteRenderer;
    int index;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        index = GetRandomMatIndex();
        spriteRenderer.material = mats.Mats[index];
        Vector3 localScale = transform.localScale;
        localScale.x *= RandomSign();
        localScale.y *= RandomSign();
        transform.localScale = localScale;

        PolygonCollider2D col = gameObject.AddComponent<PolygonCollider2D>();
        col.points = colls.Colliders[index].points;
        //CopyComponent(colls.Colliders[index], gameObject);
    }

    int RandomSign()
    {
        return (int)(Mathf.Sign(UnityEngine.Random.Range(-1f, 1f)));
    }

    int GetRandomMatIndex()
    {
        return UnityEngine.Random.Range(0, mats.Mats.Length);
    }

    //Component CopyComponent(Component original, GameObject destination)
    //{
    //    System.Type type = original.GetType();
    //    Component copy = destination.AddComponent(type);
    //    // Copied fields can be restricted with BindingFlags
    //    System.Reflection.FieldInfo[] fields = type.GetFields();
    //    foreach (System.Reflection.FieldInfo field in fields)
    //    {
    //        field.SetValue(copy, field.GetValue(original));
    //    }
    //    return copy;
    //}
}