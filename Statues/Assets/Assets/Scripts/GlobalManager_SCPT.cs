using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GlobalManager_SCPT : MonoBehaviour
{
    [Tooltip("Sansa ca un agent sa fie protector")]
    [SerializeField] public float chanceProtector = 1.20f;
    [Tooltip("Sansa ca un agent sa fie sabotor")]
    [SerializeField] public float chanceSaboteur = 0.20f;
    [Tooltip("Limita de panica/nDaca scate calmul sub, agentul se poate panica")]
    [SerializeField] public int panic = 50;
    [Tooltip("Calmul minim initial al agentului")]
    [SerializeField] public int calmMin = 80;
    [Tooltip("Cat calm se pierde pe secunda in red light")]
    [SerializeField] public int calmLossPerTick = 1;
    [Tooltip("Cat calm se pierde in jur la moarte")] 
    [SerializeField] public int calmLossDeath = 10;
    [Tooltip("Variatie viteza")] 
    [SerializeField] public float speedVariation = 1;
    [SerializeField] private AudioSource redLightSound;
    [SerializeField] private AudioSource runningSound;

    private bool lightSwitch = true; //false = red light, true = green light;
    // Start is called before the first frame update

    public UnityEvent greenLight = new UnityEvent();
    public UnityEvent redLight = new UnityEvent();
    public List<int> ids_used = new List<int>();

    public float calmGlobal;
    public int activeAgentsNumber;

    [SerializeField] public UI_Manager managerUI;

    void Start()
    {
        calmGlobal = 0;
        activeAgentsNumber = 0;

        runningSound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            LightSwitch();
        }
        managerUI.UpdateCalmBar(calmGlobal);
    }

    void LightSwitch()
    {
        lightSwitch = !lightSwitch;
        if (lightSwitch)
        {
            greenLight.Invoke();

            runningSound.Play();

            /* Play the doll red light song */
            redLightSound.Stop();
        }
        else
        {
            /* Stop the doll red light song */
            redLight.Invoke();

            redLightSound.Play();

            runningSound.Stop();
        }
    }
}
