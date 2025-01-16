using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public enum AgentType{
        Protector,
        Saboteur,
        Normie
    }

public class BehaviourManager_SCPT : MonoBehaviour
{
    AgentType type = new AgentType();
    [SerializeField]CrowdManager_SCPT crowdManager;
    [SerializeField]GlobalManager_SCPT globalManager;
    [SerializeField]AgentDisplayManager agentDisplayManager;
    [SerializeField]UI_Manager agentUIManager;

    private int calm;
    private int calmMin;
    private int panic;
    private int panicCheck;
    private int _calmLossPerSecond;
    private int _calmLossDeath;
    private int agentId;
    private float chanceProtector = 0.10f;
    private float chanceSaboteur = 0.10f;
    private float typeRoll;
    private float time;
    private float _speedVariation;
    private float rollSpeed;
    private bool isPanicked = false;
    private FollowerEntity _follower;
    private string agentName="x";
    private string agentJob="Nerd";
    private string agentSpeed="Fast";
    private string[] names;
    private string[] jobs;

    private bool redLightActive = true;
    // Start is called before the first frame update
    void Start()
    {
        if (globalManager == null)
        {
            globalManager = GameObject.Find("GlobalManager").GetComponent<GlobalManager_SCPT>();
        }

        if (agentUIManager == null)
        {
            agentUIManager = GameObject.Find("DetailPanelCanvas").GetComponent<UI_Manager>();
        }

        SetTypeAgent();
       
        calm = Random.Range(calmMin, 100);
        if (crowdManager == null)
        {
            crowdManager = GetComponentInChildren<CrowdManager_SCPT>();
        }   
        time = Time.time; 
        globalManager.greenLight.AddListener(GreenLight);
        globalManager.redLight.AddListener(RedLight);
        _follower = GetComponent<FollowerEntity>();
        _follower.maxSpeed += rollSpeed;
        Debug.Log($"Setup: ID - {gameObject.GetInstanceID()}, tip - {type}, calm - {calm}, follower gasit: {_follower.isActiveAndEnabled}");

        /* Set the calm bar */
        agentDisplayManager.UpdateCalmBar(calm);
    }

    void GreenLight()
    {
        Debug.Log("GreenLight");
        redLightActive = false;
        _follower.isStopped = false;
    }

    void RedLight()
    {
        Debug.Log("RedLight");
        redLightActive = true;
        _follower.isStopped = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetCalm(){
        return calm;
    }

    public void ModifyCalm( int value, AgentType typeReceiving, bool isDeathEffect ){
        Debug.Log($"Valori initiale: ID - {gameObject.GetInstanceID()}, tip - {type}, calm - {calm}, calm primit - {value}, tip agent primit - {typeReceiving}, moarte - {isDeathEffect}");
        /*Debug.Log("Valori initiale: " + "calm primit: " + value);
        Debug.Log("Valori initiale: " + "tip agent primit: " + typeReceiving);
        Debug.Log("Valori initiale: " + "calm: " + calm);*/
        switch ( type ){
            case AgentType.Protector:{
                if (isDeathEffect)
                {
                    calm -= _calmLossDeath;
                    break;
                }
                 if( calm < value && typeReceiving != AgentType.Saboteur )
                    calm ++;
                 //Debug.Log("Protector:" + calm );
                 break;
            }
            case AgentType.Saboteur:{
                if (isDeathEffect)
                {
                    calm += _calmLossDeath/2;
                    break;
                }
                if( calm > value && typeReceiving != AgentType.Protector )
                        calm--;
                //Debug.Log("Saboteur:" + calm );
                break;
            }
            case AgentType.Normie:{
                if (isDeathEffect)
                {
                    calm -= _calmLossDeath;
                    break;
                }
                if( calm < value )
                    calm ++;
                else if( calm > value && typeReceiving != AgentType.Protector )
                        calm--;
                //Debug.Log("Normie:" + calm );
                 break;
            }
        }
        Debug.Log($"Dupa switch: ID - {gameObject.GetInstanceID()}, tip - {type}, calm - {calm}, calm primit - {value}, tip agent primit - {typeReceiving}, moarte - {isDeathEffect}");
    }

    void SetTypeAgent(){

        if( globalManager != null ){
            chanceProtector = globalManager.chanceProtector;
            chanceSaboteur = globalManager.chanceSaboteur;
            panic = globalManager.panic;
            calmMin = globalManager.calmMin;
            _calmLossPerSecond = globalManager.calmLossPerTick;
            _calmLossDeath = globalManager.calmLossDeath;
            _speedVariation = globalManager.speedVariation;
            rollSpeed = Random.Range(-_speedVariation, _speedVariation);
            agentId = Random.Range(1, 456);

            while( globalManager.ids_used.Contains(agentId) ){
                agentId = Random.Range(1, 456);
            }

            globalManager.ids_used.Add(agentId);
        }


        TextAsset textFile = Resources.Load<TextAsset>("first-names");
        names = textFile.text.Split('\n');
        if (names.Length > 0)
        {
            agentName = names[Random.Range(0, names.Length)];
        }
        
        TextAsset textFileJobs = Resources.Load<TextAsset>("jobs");
        jobs = textFileJobs.text.Split('\n');
        if (jobs.Length > 0)
        {
            agentJob = jobs[Random.Range(0, jobs.Length)];
        }

        if( rollSpeed < -_speedVariation / 3  ){
            agentSpeed = "Slow";
        }
        else if( rollSpeed >= -_speedVariation / 3 && rollSpeed < _speedVariation / 3){
            agentSpeed = "Medium";
        }
        else{
            agentSpeed = "Fast";
        }

        typeRoll = Random.value; // returns numbers between {0 - 1}

        if( typeRoll < chanceProtector ){
            type = AgentType.Protector;
        }
        else if( typeRoll < chanceProtector + chanceSaboteur ){
            type = AgentType.Saboteur;
        }
        else{
            type = AgentType.Normie;
        }
    }

    void PanicCheck(){
         if( panic > calm ){
             //daca calm e sub panic, sansa de panicare direct proportionala cu cat de mult e calm sub panic
             panicCheck = Random.Range(0, panic);
             Debug.Log($"Panic check: ID - {gameObject.GetInstanceID()}, valoare - {panicCheck}, calm - {calm}");
             if(panicCheck > calm)
             {
                isPanicked = true;
                Death();
                Debug.Log($"Panicat: {gameObject.GetInstanceID()}");
             }
         }
    }

    void Death(){
        crowdManager.AgentCrowdEffect( calm, type, true);
        crowdManager.enabled = false;
        this.enabled = false;
    }

    void FixedUpdate(){
        if( redLightActive && time <= Time.time ){
            time = Time.time + 1f;
            crowdManager.AgentCrowdEffect( calm, type, false );
            PanicCheck();
            calm -= _calmLossPerSecond;
            Debug.Log($"Red Light Loss: ID - {gameObject.GetInstanceID()}, calm - {calm}");
        }

        /* Update calm bar */
        agentDisplayManager.UpdateCalmBar(calm);
    }

    /* When hovering, display the information panel */
    void OnMouseOver()
    {
        agentUIManager.SetPanelActive(agentId, agentName, agentJob, agentSpeed, type);
    }

    private void OnMouseExit()
    {
        agentUIManager.SetPanelInactive();
    }
}
