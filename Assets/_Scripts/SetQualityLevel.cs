using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SetQualityLevel : MonoBehaviour
{
    private void Start()
    {
		GameManager.OnLoadedConfig.AddListener(SetQuality);
		SetQuality();
    }

    public void SetQuality(float index)
	{
		if(index < 2)
		{
			GameManager.IsLightWeightBG = true;
			GameManager.OnChangeBG?.Invoke();
			Camera.main.GetUniversalAdditionalCameraData().renderPostProcessing = false;
		}
		else
		{
            GameManager.IsLightWeightBG = false;
            GameManager.OnChangeBG?.Invoke();
			Camera.main.GetUniversalAdditionalCameraData().renderPostProcessing = true;
        }
		GameManager.QualityLevel = (int)index;

		QualitySettings.SetQualityLevel((int)index, false);
        Debug.Log($"Set Quality: {QualitySettings.GetQualityLevel()}");
    }

	void SetQuality()
	{
		SetQuality(GameManager.QualityLevel);

    }
}