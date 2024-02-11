using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHolder : MonoBehaviour
{
	[field: SerializeField] public InputSO Input { get; private set; }

    public static InputHolder Instance;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
}