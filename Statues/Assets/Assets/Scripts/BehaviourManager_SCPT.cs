using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

    private int calm;
    private int calmMin;
    private int panic;
    private float chanceProtector = 0.10f;
    private float chanceSaboteur = 0.10f;
    private float typeRoll;
    private float time;
    private bool isPanicked = false;
    // Start is called before the first frame update
    void Start()
    {
        calm = Random.Range(calmMin, 100);
        // calm = 1;
        SetTypeAgent();
        CrowdManager_SCPT crowdManager = GetComponentInChildren<CrowdManager_SCPT>();   
        time = Time.time; 
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Type: " + type);
        
    }

    public int GetCalm(){
        return calm;
    }

    public void ModifyCalm( int value, AgentType typeReceiving ){
        Debug.Log("Valori initiale: " + "calm primit: " + value);
        Debug.Log("Valori initiale: " + "tip agent primit: " + typeReceiving);
        Debug.Log("Valori initiale: " + "calm: " + calm);
        switch ( type ){
            case AgentType.Protector:{
                 if( calm < value && typeReceiving != AgentType.Saboteur )
                    calm ++;
                 Debug.Log("Protector:" + calm );
                 break;
            }
            case AgentType.Saboteur:{
                if( calm > value && typeReceiving != AgentType.Protector )
                        calm--;
                Debug.Log("Saboteur:" + calm );
                break;
            }
            case AgentType.Normie:{
                if( calm < value )
                    calm ++;
                else if( calm > value && typeReceiving != AgentType.Protector )
                        calm--;
                Debug.Log("Normie:" + calm );
                 break;
            }
        }
        Debug.Log("Dupa switch: " + calm);
    }

    void SetTypeAgent(){

        if( globalManager != null ){
            chanceProtector = globalManager.chanceProtector;
            chanceSaboteur = globalManager.chanceSaboteur;
            panic = globalManager.panic;
            calmMin = globalManager.calmMin;
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
            if(Random.Range(0, panic) > calm){ //daca calm e sub panic, sansa de panicare direct proportionala cu cat de mult e calm sub panic
            isPanicked = true;
            Death();
            Debug.Log("e panicat: " + isPanicked);
            }
         }
    }

    void Death(){
        crowdManager.enabled = false;
        this.enabled = false;
    }

    void FixedUpdate(){
        if( time <= Time.time ){
            time = Time.time + 1f;
            Debug.Log("time: " + time);
            crowdManager.AgentCrowdEffect( calm, type );
            PanicCheck();
        }
    }
}
