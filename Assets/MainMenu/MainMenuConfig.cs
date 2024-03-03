using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuConfig : MonoBehaviour
{
	[SerializeField] Canvas mainMenuCanvas;
	[SerializeField] Canvas configCanvas;
	[SerializeField] Canvas SaveSlotCanvas;

	public void OpenConfig()
	{
		mainMenuCanvas.gameObject.SetActive(false);
        configCanvas.gameObject.SetActive(true);
	}

    public void OpenSaveSlot()
    {
        mainMenuCanvas.gameObject.SetActive(false);
        SaveSlotCanvas.gameObject.SetActive(true);
    }

    public void CloseConfig()
    {
        mainMenuCanvas.gameObject.SetActive(true);
        configCanvas.gameObject.SetActive(false);
    }

    public void CloseSaveSlot()
    {
        mainMenuCanvas.gameObject.SetActive(true);
        SaveSlotCanvas.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}