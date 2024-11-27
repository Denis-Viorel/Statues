using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdManager_SCPT : MonoBehaviour
{
    private List<Collider> collidersInContact = new List<Collider>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(collidersInContact.Count);
    }

    public void StartCrowdCoroutine()
    {
        StartCoroutine(CrowdCheckRoutine(0.5f));
    }

    private IEnumerator CrowdCheckRoutine(float time)
    {
        yield return new WaitForSeconds(time);
    }
    
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Hit!");
        Collider otherCollider = collision.GetComponent<Collider>();
        if (!collidersInContact.Contains(otherCollider))
        {
            collidersInContact.Add(otherCollider);
        }
        
    }

    public void AgentCrowdEffect( int calm, AgentType typeReceiving ){
        foreach( Collider collider in collidersInContact ){
            BehaviourManager_SCPT behaviourManager = collider.GetComponentInParent<BehaviourManager_SCPT>();  
            behaviourManager.ModifyCalm( calm, typeReceiving);      
            }
    }

}
