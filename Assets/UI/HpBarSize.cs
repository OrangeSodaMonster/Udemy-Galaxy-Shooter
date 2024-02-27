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

    private void Awake()
    {
        hpTransform = GetComponent<RectTransform>();
        defaultScaleInX = hpTransform.localScale.x;
        defaultBorderWidth = border.rect.width;
    }

    public void SetHpBarSize(int currentMax)
    {
        minUpgradeHP = PlayerUpgradesManager.Instance.ShipUpgradesInfo.HP_Upgrade[0].HP;
        maxUpgradeHP = PlayerUpgradesManager.Instance.ShipUpgradesInfo.HP_Upgrade[PlayerUpgradesManager.Instance.ShipUpgradesInfo.HP_Upgrade.Length - 1].HP;

        float value = Remap(currentMax, minUpgradeHP, maxUpgradeHP, 0, 1);
        float newXScale = Mathf.Lerp(defaultScaleInX, maxScaleInX, value);
        hpTransform.localScale = new Vector3(newXScale, hpTransform.localScale.y, hpTransform.localScale.z);

        border.sizeDelta = new Vector2(Mathf.Lerp(defaultBorderWidth, borderWidthAtMaxScale, value), border.sizeDelta.y);
    }

    float Remap(float source, float sourceFrom, float sourceTo, float targetFrom, float targetTo)
    {
        return targetFrom + (source-sourceFrom)*(targetTo-targetFrom)/(sourceTo-sourceFrom);
    }
}