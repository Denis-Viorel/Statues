using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalManager_SCPT : MonoBehaviour
{
    [Tooltip("Sansa ca un agent sa fie protector")]
    [SerializeField] public float chanceProtector = 0.20f;
    [Tooltip("Sansa ca un agent sa fie sabotor")]
    [SerializeField] public float chanceSaboteur = 0.20f;
    [Tooltip("Limita de panica/nDaca scate calmul sub, agentul se poate panica")]
    [SerializeField] public int panic = 50;
    [Tooltip("Calmul minim initial al agentului")]
    [SerializeField] public int calmMin = 80;
    [Tooltip("Cat calm se pierde pe secunda in red light")]
    [SerializeField] public int calmLossPerTick = 1;

    private bool lightSwitch = false; //false = red light, true = green light;
    // Start is called before the first frame update

    public UnityEvent greenLight = new UnityEvent();
    public UnityEvent redLight = new UnityEvent();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LightSwitch();
        }
    }

    void LightSwitch()
    {
        lightSwitch = !lightSwitch;
        if (lightSwitch)
        {
            greenLight.Invoke();
        }
        else
        {
            redLight.Invoke();
        }
    }
}
