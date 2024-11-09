using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class TooltipLink : MonoBehaviour
{
    [SerializeField] TooltipRefHolder tooltip;
    [Space]
    [SerializeField, TextArea(2, 3)] string descriptionText = "Current\nDamage";
    [SerializeField, TextArea(2, 3)] string descriptionTextPT = "Dano\nAtual";
    [SerializeField, TextArea(2, 3)] string descriptionTexteESP = "Daño\nActual";
    [Space]
    [SerializeField] string unityText = "per hit";
    [SerializeField] string unityTextPT = "por acerto";
    [SerializeField] string unityTextESP = "por acierto";

    Button thisButton;
    ButtonScript buttonScript;
    GameObject lastSelection;

    private void Awake()
    {
        thisButton = GetComponentInChildren<Button>();
        buttonScript = GetComponentInChildren<ButtonScript>();
    }
    private void OnEnable()
    {
        buttonScript.OnClick.AddListener(UpdateTooltip);
    }
    private void OnDisable()
    {
        buttonScript.OnClick.RemoveListener(UpdateTooltip);
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == thisButton.gameObject && lastSelection != thisButton.gameObject)
        {
            UpdateTooltip();
        }

        lastSelection = EventSystem.current.currentSelectedGameObject;
    }

    public void UpdateTooltip()
    {
        tooltip.gameObject.SetActive(true);
        tooltip.valueTextMesh.text = GetValue();

        if(GameManager.CurrentLanguage == Language.English)
        {
            tooltip.descriptionTextMesh.text = descriptionText;
            tooltip.unityTextMesh.text = unityText;
        }
        else if (GameManager.CurrentLanguage == Language.Português)
        {
            tooltip.descriptionTextMesh.text = descriptionTextPT;
            tooltip.unityTextMesh.text = unityTextPT;
        }
        else if (GameManager.CurrentLanguage == Language.Español)
        {
            tooltip.descriptionTextMesh.text = descriptionTexteESP;
            tooltip.unityTextMesh.text = unityTextESP;
        }
    }

    abstract public string GetValue();
}
