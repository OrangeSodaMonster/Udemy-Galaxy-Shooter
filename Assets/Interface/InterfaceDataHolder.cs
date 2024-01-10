using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "InterfaceDataHolder", menuName = "MySOs/InterfaceDataHolder")]
public class InterfaceDataHolder : ScriptableObject
{
	[Header("Upgrade Box Colors")]
	public Color unavaliableColor = Color.grey;
    public Color avaliableColor = Color.white;
	public Color maxedColor = Color.yellow;

    [Header("Upgrade Costs")]
	public Sprite metalSprite = null;
	public Color metalColor;
    public Sprite alloySprite = null;
    public Color alloyColor;
    public Sprite energyCristalSprite = null;
    public Color energyCristalColor;
    public Sprite condensedCristalSprite = null;
    public Color condensedCristalColor;


    public void UpdateButtonVisual(int upgradeLevel, int upgradeInfoLenght, Image icon, Image border, TextMeshProUGUI upgradeLevelTxt, GameObject[] costs,
        ResourceNumber[] resourceNumber, bool isAvaliable = true)
    {
        Image costOne = costs[0].GetComponent<Image>();
        Image costTwo = costs[1].GetComponent<Image>();
        TextMeshProUGUI costOneTxt = costs[0].GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI costTwoTxt = costs[1].GetComponentInChildren<TextMeshProUGUI>();

        if (!isAvaliable)
        {
            border.color = unavaliableColor;
            upgradeLevelTxt.enabled = false;
            icon.color = Color.gray;

            SetCost(resourceNumber[0], costOne, costOneTxt);
            SetCost(resourceNumber[1], costTwo, costTwoTxt);

            return;
        }

        upgradeLevelTxt.text = $"{upgradeLevel}/{upgradeInfoLenght}";

        if (upgradeLevel == upgradeInfoLenght)
        {
            icon.color = Color.white;
            border.color = maxedColor;
            upgradeLevelTxt.enabled = true;
            upgradeLevelTxt.color = maxedColor;
            costOne.enabled = false;
            costOneTxt.enabled = false;
            costTwo.enabled = false;
            costTwoTxt.enabled = false;
        }
        else
        {
            icon.color = Color.white;
            border.color = avaliableColor;
            upgradeLevelTxt.enabled = true;
            upgradeLevelTxt.color = avaliableColor;

            SetCost(resourceNumber[0], costOne, costOneTxt);
            SetCost(resourceNumber[1], costTwo, costTwoTxt);
        }
    }

    private void SetCost(ResourceNumber resourceNumber, Image costImage, TextMeshProUGUI costText)
    {
        if (resourceNumber.ResourceType == ResourceType.Metal)
        {
            costImage.sprite = metalSprite;
            costText.text = resourceNumber.Amount.ToString();
            SetCostImageTextVisual(resourceNumber, costImage, costText);
        }
        else if (resourceNumber.ResourceType == ResourceType.Alloy)
        {
            costImage.sprite = alloySprite;
            costText.text = resourceNumber.Amount.ToString();
            SetCostImageTextVisual(resourceNumber, costImage, costText);
        }
        else if (resourceNumber.ResourceType == ResourceType.EnergyCristal)
        {
            costImage.sprite = energyCristalSprite;
            costText.text = resourceNumber.Amount.ToString();
            SetCostImageTextVisual(resourceNumber, costImage, costText);
        }
        else if (resourceNumber.ResourceType == ResourceType.CondensedEnergyCristal)
        {
            costImage.sprite = condensedCristalSprite;
            costText.text = resourceNumber.Amount.ToString();
            SetCostImageTextVisual(resourceNumber, costImage, costText);
        }
    }

    private void SetCostImageTextVisual(ResourceNumber resourceNumber, Image costImage, TextMeshProUGUI costText)
    {
        if (resourceNumber.Amount > PlayerCollectiblesCount.MetalAmount)
        {
            costText.color = unavaliableColor;
            costImage.color = Color.gray;
        }
        else
        {
            costText.color = Color.white;
            costImage.color = Color.white;
        }
    }

    public void SetUnlockedCostVisual(ResourceNumber[] resourceNumber, GameObject[] costs)
    {
        Image costOne = costs[0].GetComponent<Image>();
        Image costTwo = costs[1].GetComponent<Image>();
        TextMeshProUGUI costOneTxt = costs[0].GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI costTwoTxt = costs[1].GetComponentInChildren<TextMeshProUGUI>();

        SetCost(resourceNumber[0], costOne, costOneTxt);
        SetCost(resourceNumber[1], costTwo, costTwoTxt);
    }

    public void SetBoolButtonVisual(Image icon, Image border, TextMeshProUGUI upgradeLevelTxt, ResourceNumber[] resourceNumber, GameObject[] costs, bool isAvaliable = true, bool isMaxed = false)
    {
        upgradeLevelTxt.enabled = false;

        if (!isAvaliable)
        {
            border.color = unavaliableColor;            
            icon.color = Color.gray;

            costs[0].SetActive(true);
            costs[1].SetActive(true);
            SetUnlockedCostVisual(resourceNumber, costs);
        }
        else if (!isMaxed)
        {
            border.color = avaliableColor;
            icon.color = Color.white;
            costs[0].SetActive(false);
            costs[1].SetActive(false);
        }
        else
        {
            border.color = maxedColor;
            icon.color = Color.white;
            costs[0].SetActive(false);
            costs[1].SetActive(false);
        }
    }
}