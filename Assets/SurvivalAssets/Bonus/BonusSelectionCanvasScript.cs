using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSelectionCanvasScript : MonoBehaviour
{
    public static BonusSelectionCanvasScript Instance;

    [SerializeField] RectTransform leftSite;
    [SerializeField] RectTransform middleSite;
    [SerializeField] RectTransform rightSite;

    public void SetInstance()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        BonusSelection.Instance.GetBonusBoxes(out GameObject leftBox, out GameObject middleBox, out GameObject rightBox);

        Instantiate(leftBox, leftSite);
        Instantiate(rightBox, rightSite);
        Instantiate(middleBox, middleSite);
    }

    private void OnDisable()
    {
        Destroy(leftSite.GetChild(0).gameObject);
        Destroy(middleSite.GetChild(0).gameObject);
        Destroy(rightSite.GetChild(0).gameObject);
    }

    public void OpenBonusCanvas()
    {
        gameObject.SetActive(true);
        PauseAndUIManager.Instance.PauseDealer(true);
    }
    public void LeaveBonusCanvas()
    {
        gameObject.SetActive(false);
        PauseAndUIManager.Instance.PauseDealer(false);
    }


}
