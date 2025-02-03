using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private BehaviourManager_SCPT behaviourManager;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider col;
    public enum trapType { wolf, spikes };
    [SerializeField]  private trapType selectedTrap;
    // Start is called before the first frame update
    void Start()
    {
        if(audioSource == null)
            audioSource = GetComponentInChildren<AudioSource>();
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (col == null)
        {
            col = GetComponent<BoxCollider>();
        }
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
            audioSource.Play();
            animator.SetTrigger("Entered");
            Debug.Log("Trap entered" + animator.gameObject.name);

            if( selectedTrap == trapType.wolf ){
                col.enabled = false;
            }
        }
    }
}
