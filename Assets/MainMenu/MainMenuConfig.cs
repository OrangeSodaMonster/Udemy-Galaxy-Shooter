using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuConfig : MonoBehaviour
{
	[SerializeField] Canvas mainMenuCanvas;
	[SerializeField] Canvas configCanvas;

	public void OpenConfig()
	{
		mainMenuCanvas.gameObject.SetActive(false);
        configCanvas.gameObject.SetActive(true);
	}
    public void CloseConfig()
    {
        mainMenuCanvas.gameObject.SetActive(true);
        configCanvas.gameObject.SetActive(false);
    }
}