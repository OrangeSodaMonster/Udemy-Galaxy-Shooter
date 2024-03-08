using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPS : MonoBehaviour
{
	[SerializeField] int redBellow = 20;
	[SerializeField] int yellowBellow = 45;
	[SerializeField] int framesToAverage = 6;

    [SerializeField] TextMeshProUGUI text;

    float[] deltas;
    int currentFrame = 0;

    private void Start()
    {
        deltas = new float[framesToAverage];
    }

    private void Update()
    {
        deltas[currentFrame] = Time.deltaTime;
        currentFrame++;

        if(currentFrame == framesToAverage)
        {
            float deltaSum = 0;
            for(int i = 0; i < framesToAverage; i++)
            {
                deltaSum += deltas[i];
            }

            float deltaAvr = deltaSum / framesToAverage;
            int fps = (int)(1 / deltaAvr);

            text.text = fps.ToString();
            if (fps <= redBellow) text.color = Color.red;
            else if (fps <= yellowBellow) text.color = Color.yellow;
            else text.color = Color.green;

            currentFrame = 0;
        }
    }

}