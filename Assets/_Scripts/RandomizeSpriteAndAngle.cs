using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomizeSpritesAndAngle : MonoBehaviour
{
    [SerializeField] bool randomizeOnEnable;
    [SerializeField] List<SpriteRenderer> targetRenderers = new List<SpriteRenderer>();
    [SerializeField] bool dontRepeatSprite = true;
    [SerializeField] List<Sprite> sprites = new List<Sprite>();
    [SerializeField] bool randomizeAngle = true;
    [SerializeField] List<float> angles = new List<float>();

    private void OnEnable()
    {
        if (randomizeOnEnable)
            Randomize();
    }

    void Start()
    {
        if (!randomizeOnEnable)
            Randomize();
    }    

    void Randomize()
    {
        for (int i = 0; i < targetRenderers.Count; i++)
        {
            int spriteIndex = Random.Range(0, sprites.Count-1);
            targetRenderers[i].sprite = sprites[spriteIndex];
            if (dontRepeatSprite)
                sprites.RemoveAt(spriteIndex);

            if (randomizeAngle)
                targetRenderers[i].transform.eulerAngles = new Vector3(0, 0, angles[Random.Range(0, angles.Count-1)]);
        }
    }
}
