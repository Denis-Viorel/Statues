using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UI_Manager : MonoBehaviour
{
    [SerializeField]private GameObject panel;
    [SerializeField]private TextMeshProUGUI infoText;
    [SerializeField] UnityEngine.UI.Image imageIdent;
    [SerializeField] public Sprite protectorImage;
    [SerializeField] public Sprite sabotouerImage;
    [SerializeField] public Sprite normieImage;
    [SerializeField] private UnityEngine.UI.Slider globalCalmSlider;

    private void Start()
    {
        panel.SetActive(false);
    }

    public void SetPanelActive(int agentNumber, string agentName, string agentJob, string agentSpeed, AgentType agentClass)
    {
        panel.SetActive(true);

        infoText.text = agentNumber + "\n" + agentName + "\n" + agentJob + "\n" + agentSpeed + "\n" + agentClass;

        switch (agentClass)
        {
            case AgentType.Protector:
                imageIdent.sprite = protectorImage;
                break;
            case AgentType.Saboteur:
                imageIdent.sprite = sabotouerImage;
                break;
            case AgentType.Normie:
                imageIdent.sprite= normieImage;
                break;
        }
    }

    public void SetPanelInactive()
    {
        panel.SetActive(false);
    }

    public void UpdateCalmBar(float currentCalm)
    {
        globalCalmSlider.value = currentCalm;
    }
}
