using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickSound : MonoBehaviour
{
    Button button;

    void Start()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(PlayClickSound);
    }

    public void PlayClickSound()
    {
        //AudioManager.Instance.ClickSound.PlayFeedbacks();
    }
}