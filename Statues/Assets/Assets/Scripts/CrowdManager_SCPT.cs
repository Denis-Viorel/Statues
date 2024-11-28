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
            Debug.Log("Colliders: " + collidersInContact.Count);
            foreach( Collider collision in collidersInContact ){
                BehaviourManager_SCPT behaviourManager = collision.GetComponentInParent<BehaviourManager_SCPT>();  
                behaviourManager.ModifyCalm( calm, typeReceiving);
                }
        }

    }
