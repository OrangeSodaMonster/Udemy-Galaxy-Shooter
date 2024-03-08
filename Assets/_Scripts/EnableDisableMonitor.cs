using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisableMonitor : MonoBehaviour
{
    public static bool isMonitor;
    [SerializeField] Canvas statsMonitor;

    private void OnEnable()
    {
        SetMonitor();
        GameStatus.UnPausedGame += SetMonitor;
        GameStatus.PausedGame += DisableMonitor;
        GameStatus.GameOver += DisableMonitor;
    }
    private void OnDisable()
    {
        GameStatus.UnPausedGame -= SetMonitor;
        GameStatus.PausedGame -= DisableMonitor;
        GameStatus.GameOver -= DisableMonitor;
    }

    void SetMonitor()
    {
        statsMonitor.gameObject.SetActive(isMonitor);
        //Debug.Log("Set Monitor");
    }

    void DisableMonitor()
    {
        statsMonitor.gameObject.SetActive(false);
        //Debug.Log("HideMonitor");
    }
}