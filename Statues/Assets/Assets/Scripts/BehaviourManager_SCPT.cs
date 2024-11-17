using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public enum AgentType{
        Normie,
        Protector,
        Saboteur
    }

public class BehaviourManager_SCPT : MonoBehaviour
{
    AgentType type = new AgentType();
    [SerializeField]CrowdManager_SCPT crowdManager;

    private int calm;
    // Start is called before the first frame update
    void Start()
    {
        calm = Random.Range(80, 100);
        type = (AgentType)Random.Range(0, System.Enum.GetValues(typeof(AgentType)).Length);
        CrowdManager_SCPT crowdManager = GetComponentInChildren<CrowdManager_SCPT>();    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Type: " + type);
        crowdManager.AgentCrowdEffect( calm, type);
    }

    public int GetCalm(){
        return calm;
    }

    public void ModifyCalm( int value, AgentType typeReceiving ){
        Debug.Log("Valori initiale: " + "calm primit: " + value);
        Debug.Log("Valori initiale: " + "tip agent primit: " + typeReceiving);
        Debug.Log("Valori initiale: " + "calm: " + calm);
        switch ( type ){
            case AgentType.Normie:{
                if( calm < value )
                    calm ++;
                else if( calm > value && typeReceiving != AgentType.Protector )
                        calm--;
                Debug.Log("Normie:" + calm );
                 break;
            }
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
        }
        Debug.Log("Dupa switch: " + calm);
    }
}
