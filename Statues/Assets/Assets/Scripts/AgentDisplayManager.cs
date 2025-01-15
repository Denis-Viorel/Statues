using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgentDisplayManager : MonoBehaviour
{
    public Slider calmSlider;
    public float maxCalm = 100f;
    public float calm;

    void Update()
    {
        /* Rotate with the camera */
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }

    public void UpdateCalmBar(float currentCalm)
    {
        calmSlider.value = currentCalm;
    }
}
