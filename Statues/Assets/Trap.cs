using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private BehaviourManager_SCPT behaviourManager;
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
            behaviourManager = other.transform.GetComponentInParent<BehaviourManager_SCPT>();
            behaviourManager.Death();
        }
    }
}
