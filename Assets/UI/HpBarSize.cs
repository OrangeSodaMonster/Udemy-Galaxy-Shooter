using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarSize : MonoBehaviour
{
	[SerializeField] float maxScaleInX;

	[SerializeField] RectTransform border;
	[SerializeField] float borderWidthAtMaxScale;

    RectTransform hpTransform;
    float defaultScaleInX = 1;
    float defaultBorderWidth;

    int minUpgradeHP;
    int maxUpgradeHP;
    int currentMaxHP;

    private void Awake()
    {
        hpTransform = GetComponent<RectTransform>();
        defaultScaleInX = hpTransform.localScale.x;
        defaultBorderWidth = border.rect.width;
    }

    private void Start()
    {
        minUpgradeHP = PlayerUpgradesManager.Instance.ShipUpgradesInfo.HP_Upgrade[0].HP;
        maxUpgradeHP = PlayerUpgradesManager.Instance.ShipUpgradesInfo.HP_Upgrade[PlayerUpgradesManager.Instance.ShipUpgradesInfo.HP_Upgrade.Length - 1].HP;
        GetCurrentMaxHP();
        SetHpBarSize();
    }

    public void SetHpBarSize()
    {
        StartCoroutine(Routine());

        IEnumerator Routine()
        {
            yield return null;

            float value = Remap(currentMaxHP, minUpgradeHP, maxUpgradeHP, 0, 1);
            float newXScale = Mathf.Lerp(defaultScaleInX, maxScaleInX, value);
            hpTransform.localScale = new Vector3(newXScale, hpTransform.localScale.y, hpTransform.localScale.z);

            Rect borderRect = border.rect;
            borderRect.width = Mathf.Lerp(defaultBorderWidth, borderWidthAtMaxScale, value);

            Debug.Log("Set HP Size: " + newXScale);
            Debug.Log("Set HP Size: " + newXScale);
        }
    }

    void GetCurrentMaxHP()
    {
        currentMaxHP = PlayerUpgradesManager.Instance.ShipUpgradesInfo.HP_Upgrade[PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.HPLevel - 1].HP;
    }

    float Remap(float source, float sourceFrom, float sourceTo, float targetFrom, float targetTo)
    {
        return targetFrom + (source-sourceFrom)*(targetTo-targetFrom)/(sourceTo-sourceFrom);
    }
}