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
    [Space]
    [SerializeField] string unityText = "per hit";
    [SerializeField] string unityTextPT = "por acerto";

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
        tooltip.descriptionTextMesh.text = descriptionText;
        tooltip.valueTextMesh.text = GetValue();
        tooltip.unityTextMesh.text = unityText;

        Debug.Log("UPDATE TOOLTIP");
    }

    abstract public string GetValue();
}
