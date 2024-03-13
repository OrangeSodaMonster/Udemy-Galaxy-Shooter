using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{   
    [Header("")]
    [SerializeField] RectTransform pauseCanvas;
    [Header("")]
    [SerializeField] RectTransform gameoverCanvas;
    static RectTransform s_gameoverCanvas;
    [Header("Upgrade Canvas")]
    [SerializeField] RectTransform shipUpgradePage;
    [SerializeField] RectTransform laserUpgradePage;
    [SerializeField] RectTransform shieldUpgradePage;
    [SerializeField] RectTransform ionStreamUpgradePage;
    [SerializeField] RectTransform dronesUpgradePage;
    [SerializeField] RectTransform audioPage;
    [SerializeField] RectTransform touchPage;
    [SerializeField] RectTransform[] disableOnPause;

    public static UnityEvent PauseGame = new();

    bool isOnPage;

    public static UIManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        s_gameoverCanvas = gameoverCanvas;
        PauseGame.AddListener(SetPause);
    }

    private void OnEnable()
    {
        InputHolder.Instance.Pause += SetPause;
    }
    private void OnDisable()
    {
        InputHolder.Instance.Pause -= SetPause;

        Time.timeScale = 1;
        GameStatus.IsPaused = false;
        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.UnmuteTrack, MMSoundManager.MMSoundManagerTracks.Sfx);
    }

    private void SetPause()
    {
        if (!GameStatus.IsPaused && !isOnPage && MySceneManager.IsFeedbackEnabled)
        {
            StartPause();
            CallGarbageColector.CallGC();
        }
        else if (GameStatus.IsPaused && !isOnPage)
        {
            LeavePause();
        }
    }

    public void StartPause()
    {
        pauseCanvas.gameObject.SetActive(true);
        GameStatus.IsPaused = true;
        Time.timeScale = 0;

        DisablePlayerCommands.Instance.SetCommands(false);

        foreach(RectTransform rect in disableOnPause)
        {
            rect.gameObject.SetActive(false);
        }
        AudioTrackConfig.Instance.MuteVFX();
    }

    public void LeavePause()
    {
        pauseCanvas.gameObject.SetActive(false);
        GameStatus.IsPaused = false;
        Time.timeScale = 1;

        DisablePlayerCommands.Instance.SetCommands(true);

        foreach (RectTransform rect in disableOnPause)
        {
            rect.gameObject.SetActive(true);
        }
        AudioTrackConfig.Instance.UnmuteVFX();
    }

    public void EnableShipUpgradePage()
    {
        if (!GameStatus.IsPaused) return;

        StartCoroutine(DisableEnableUpgradeDelay(shipUpgradePage));
        isOnPage = true;
    }
    public void EnableLaserUpgradePage()
    {
        if (!GameStatus.IsPaused) return;

        StartCoroutine(DisableEnableUpgradeDelay(laserUpgradePage));
        isOnPage = true;
    }
    public void EnableShieldUpgradePage()
    {
        if (!GameStatus.IsPaused) return;

        StartCoroutine(DisableEnableUpgradeDelay(shieldUpgradePage));
        isOnPage = true;
    }
    public void EnableIonStreamUpgradePage()
    {
        if (!GameStatus.IsPaused) return;

        StartCoroutine(DisableEnableUpgradeDelay(ionStreamUpgradePage));
        isOnPage = true;
    }
    public void EnableDronesUpgradePage()
    {
        if (!GameStatus.IsPaused) return;

        StartCoroutine(DisableEnableUpgradeDelay(dronesUpgradePage));
        isOnPage = true;
    }
    public void EnableAudioPage()
    {
        if (!GameStatus.IsPaused) return;

        StartCoroutine(DisableEnableUpgradeDelay(audioPage));
        isOnPage = true;
    }
    public void EnableTouchPage()
    {
        if (!GameStatus.IsPaused) return;

        StartCoroutine(DisableEnableUpgradeDelay(touchPage));
        isOnPage = true;
    }
    public void DisableAllCanvas()
    {
        shipUpgradePage.gameObject.SetActive(false);
        laserUpgradePage.gameObject.SetActive(false);
        shieldUpgradePage.gameObject.SetActive(false);
        ionStreamUpgradePage.gameObject.SetActive(false);
        dronesUpgradePage.gameObject.SetActive(false);
        audioPage.gameObject.SetActive(false);
        touchPage.gameObject.SetActive(false);
        pauseCanvas.gameObject.SetActive(false);         
    }

    IEnumerator DisableEnableUpgradeDelay(RectTransform canvasToEnable)
    {
        yield return null;

        DisableAllCanvas();

        canvasToEnable.gameObject.SetActive(true);
        isOnPage = true;
    }

    IEnumerator DisableEnablePauseDelay()
    {
        yield return null;

        DisableAllCanvas();

        pauseCanvas.gameObject.SetActive(true);
        isOnPage = false;
    }

    public void ReturnToPauseCanvas()
    {
        StartCoroutine(DisableEnablePauseDelay());
    }

    public static void EnableGameoverCanvas()
    {
        s_gameoverCanvas.gameObject.SetActive(true);
    }
    public void RestartScene()
    {
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }    
}