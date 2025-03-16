using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField]private int agentsNumber;
    [SerializeField]private int finishedAgentsNumber;
    [SerializeField]GlobalManager_SCPT globalManager;
    [SerializeField]CrowdManager_SCPT crowdManager;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //TODO: remove collider, AgentFinished should handle it, move this logic to Global, delete Finish object
    /*void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            agentsNumber = globalManager.activeAgentsNumber;
            finishedAgentsNumber++;
            Debug.Log("agentsNumber" + agentsNumber + " finished " + finishedAgentsNumber);
            if (finishedAgentsNumber == agentsNumber)
            {
                StartCoroutine(Delay(5.0f));
            }
        }
    }*/

    public void AgentFinished()
    {
        agentsNumber = globalManager.activeAgentsNumber;
        finishedAgentsNumber++;
        Debug.Log("agentsNumber" + agentsNumber + " finished " + finishedAgentsNumber);

        if (finishedAgentsNumber == agentsNumber)
        {
            StartCoroutine(Delay(5.0f));
        }
    }

    IEnumerator Delay(float delayTime)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);
        globalManager.WinRound(finishedAgentsNumber);
        finishedAgentsNumber = 0;
        //Do the action after the delay time has finished.
    }
}
