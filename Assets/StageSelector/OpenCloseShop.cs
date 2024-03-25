using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseShop : MonoBehaviour
{
    [Header("To hide")]
    [SerializeField] RectTransform upgradeButton;
    [SerializeField] RectTransform returnToMainMenuButton;
    [Space]
    [SerializeField] RectTransform ShopCanvas;

    [Button]
    public void OpenShopCanvas()
    {
        upgradeButton.gameObject.SetActive(false);
        returnToMainMenuButton.gameObject.SetActive(false);
        ShopCanvas.gameObject.SetActive(true);
    }

    [Button]
    public void CloseShopCanvas()
    {
        upgradeButton.gameObject.SetActive(true);
        returnToMainMenuButton.gameObject.SetActive(true);
        ShopCanvas.gameObject.SetActive(false);
    }
}