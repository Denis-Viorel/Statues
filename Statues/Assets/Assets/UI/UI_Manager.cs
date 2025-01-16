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

    private void Start()
    {
        panel.SetActive(false);
    }

    public void SetPanelActive(int agentNumber, string agentName, string agentJob, string agentSpeed, AgentType agentClass)
    {
        panel.SetActive(true);

        infoText.text = agentNumber + "\n" + agentName + "\n" + agentJob + "\n" + agentSpeed + "\n" + agentClass;
    }

    public void SetPanelInactive()
    {
        panel.SetActive(false);
    }
}
