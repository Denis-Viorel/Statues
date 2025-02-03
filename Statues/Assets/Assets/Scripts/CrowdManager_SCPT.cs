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
            if (!collidersInContact.Contains(otherCollider) && otherCollider.gameObject.tag.Equals("Player"))
            {
                collidersInContact.Add(otherCollider);
            }
            
        }

        public void AgentCrowdEffect( float calm, AgentType typeReceiving, bool isDeathEffect ){
            Debug.Log("Colliders: " + collidersInContact.Count);
            foreach( Collider collision in collidersInContact ){
                BehaviourManager_SCPT behaviourManager = collision.transform.GetComponentInParent<BehaviourManager_SCPT>();
                if (behaviourManager != null)
                {
                    behaviourManager.ModifyCalm( calm, typeReceiving, isDeathEffect );
                } else {
                    Debug.LogWarning($"Could not find BehaviourManager on or above {collision.gameObject.name}");
                }
                
                }
        }

    }
