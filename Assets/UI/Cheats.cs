using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cheats : MonoBehaviour
{
    PlayerHP playerHP;
    EnemySpawner spawner;
    RareSpawnScript rareSpawner;

    [SerializeField] Button invencibilityButton;
    TextMeshProUGUI invencibilityText;

    [SerializeField] Button spawnerButton;
    TextMeshProUGUI spawnerText;
    [SerializeField] Button selfDestructButton;
    [SerializeField] Button fillHPButton;
    [SerializeField] Button addResourcesButton;
    [SerializeField] Button monitorButton;
    TextMeshProUGUI monitorText;

    Color defaultTextColor;

    private void Awake()
    {
        invencibilityText = invencibilityButton.GetComponentInChildren<TextMeshProUGUI>();
        defaultTextColor = invencibilityText.color;

        spawnerText = spawnerButton.GetComponentInChildren<TextMeshProUGUI>();  
        monitorText = monitorButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    void Start()
    {
        playerHP = FindObjectOfType<PlayerHP>();
        spawner = FindObjectOfType<EnemySpawner>();
        rareSpawner = FindObjectOfType<RareSpawnScript>();

        invencibilityButton.onClick.AddListener(SetInvencibility);
        spawnerButton.onClick.AddListener(SetSpawner);
        selfDestructButton.onClick.AddListener(SelfDestruct);
        fillHPButton.onClick.AddListener(FillHP);
        addResourcesButton.onClick.AddListener(AddResources);
        monitorButton.onClick.AddListener(ToogleMonitor);

        UpdateTextColor(invencibilityText, playerHP.isInvencible);
        UpdateTextColor(spawnerText, !spawner.enabled);
    }

    public void FillHP()
    {
        PlayerHP.Instance.ChangePlayerHP(200);
    }

    public void SelfDestruct()
    {
        PauseAndUIManager.Instance.DisableAllCanvas();
        Time.timeScale = 1;
        GameStatus.IsPaused = false;
        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.UnmuteTrack, MMSoundManager.MMSoundManagerTracks.Sfx);

        playerHP.isInvencible = false;
        PlayerHP.Instance.ChangePlayerHP(-PlayerHP.Instance.CurrentHP, ignoreInvencibility:true);
    }

    public void SetInvencibility()
    {
        playerHP.isInvencible = !playerHP.isInvencible;

        UpdateTextColor(invencibilityText, playerHP.isInvencible);
    }

    public void SetSpawner()
    {
        spawner.enabled = !spawner.enabled;
        rareSpawner.enabled = spawner.enabled;

        UpdateTextColor(spawnerText, !spawner.enabled);
    }

    public void ToogleMonitor()
    {
        EnableDisableMonitor.isMonitor = !EnableDisableMonitor.isMonitor;
        UpdateTextColor(monitorText, EnableDisableMonitor.isMonitor);
    }

    public void AddResources()
    {
        PlayerCollectiblesCount.MetalAmount += 500;
        PlayerCollectiblesCount.RareMetalAmount += 500;
        PlayerCollectiblesCount.EnergyCristalAmount += 500;
        PlayerCollectiblesCount.CondensedEnergyCristalAmount += 500;

        PlayerCollectiblesCount.ChangedCollectbleAmount();
    }

    void UpdateTextColor(TextMeshProUGUI text, bool enabled)
    {
        text.color = enabled? Color.green : defaultTextColor;
    }
  
}