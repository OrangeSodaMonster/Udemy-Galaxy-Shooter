using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionMapManager : MonoBehaviour
{    
    enum maps
    {
        Playmode = 1,
        UI = 2,
    }

    [SerializeField] maps defaulActionMap = maps.Playmode;
    public static UdemyGalaxyShooter inputActions;
    PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {      
        GameStatus.PausedGame += SetUIMap;
        GameStatus.GameOver += SetUIMap;
        GameStatus.UnPausedGame += SetPlayerMap;

        StartCoroutine(SetMapRoutine());

        IEnumerator SetMapRoutine()
        {
            yield return null;

            if (defaulActionMap == maps.Playmode)
            {
                SetUIMap();
                SetPlayerMap();
            }
            else if (defaulActionMap == maps.UI)
            {
                SetPlayerMap();
                SetUIMap();
            }
        }
    }

    private void OnDisable()
    {
        GameStatus.PausedGame -= SetUIMap;
        GameStatus.GameOver -= SetUIMap;
        GameStatus.UnPausedGame -= SetPlayerMap;
    }

    //private void Update()
    //{
    //    if (InputHolder.Instance.Input.IsCancelUI)
    //    {
    //        Debug.Log("Cancel");
    //    }
    //    if (InputHolder.Instance.Input.IsSpecialing)
    //    {
    //        Debug.Log("Special");
    //    }
    //}

    void SetPlayerMap()
    {
        playerInput.SwitchCurrentActionMap("Player");
    }

    void SetUIMap()
    {
        playerInput.SwitchCurrentActionMap("UI");
    }
}