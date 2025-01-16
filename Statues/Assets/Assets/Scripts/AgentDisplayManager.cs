using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgentDisplayManager : MonoBehaviour
{
    public Slider calmSlider;
    public float maxCalm = 100f;
    public float calm;
    [SerializeField]Image imageIdent;
    [SerializeField]public Sprite protectorImage;
    [SerializeField]public Sprite sabotouerImage;

    void Update()
    {
        /* Rotate with the camera */
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }

    public void UpdateCalmBar(float currentCalm)
    {
        calmSlider.value = currentCalm;
    }

    public void SetAgentTypePhoto(AgentType agentClass)
    {
        switch(agentClass) 
        {
            case AgentType.Protector:
                imageIdent.enabled = true;
                imageIdent.sprite = protectorImage;
                break;
            case AgentType.Saboteur:
                imageIdent.enabled = true;
                imageIdent.sprite = sabotouerImage;
                break;
            case AgentType.Normie: 
                imageIdent.enabled = false;
                break;
        }
    }
}
