using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    private int agentsNumber;
    private int finishedAgentsNumber;
    [SerializeField]GlobalManager_SCPT globalManager;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
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
    }

    IEnumerator Delay(float delayTime)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);
        globalManager.Win(finishedAgentsNumber);
        //Do the action after the delay time has finished.
    }
}
