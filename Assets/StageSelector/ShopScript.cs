using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
class ShopInfo
{
    public Button button;
    public ResourceNumber cost;
    public ResourceNumber buy;
}

public class ShopScript : MonoBehaviour
{
    [SerializeField] Color unavaliableColor;
    [SerializeField] ShopInfo[] shopInfos;

    Color avaliableColor;
    AudioManager audioManager;

    void Awake()
    {
        avaliableColor = shopInfos[0].button.GetComponent<Image>().color;
    }

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnEnable()
    {
        UpdateVisuals();
    }

    public void Buy(int index)
    {
        if (PlayerCollectiblesCount.ExpendResources(shopInfos[index].cost))
        {
            audioManager.UpgradeSound.PlayFeedbacks();
            PlayerCollectiblesCount.AddResourceNumber(shopInfos[index].buy);
            PlayerCollectiblesCount.ChangedCollectbleAmount();
        }
        else
        {
            audioManager.UpgradeFailSound.PlayFeedbacks();
        }
    }

    private void UpdateVisuals()
    {
        for (int i = 0; i < shopInfos.Length; i++)
        {
            if (!PlayerCollectiblesCount.CheckResourcesAmmount(shopInfos[i].cost))
            {
                shopInfos[i].button.GetComponent<Image>().color = unavaliableColor;
            }
            else
            {
                shopInfos[i].button.GetComponent<Image>().color = avaliableColor;
            }
        }
    }
}

