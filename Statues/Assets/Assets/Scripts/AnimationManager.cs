using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField]Animator animator;
    [SerializeField]ParticleSystem blood;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        blood = GetComponentInChildren<ParticleSystem>();
        blood.Pause();
    }

    public void UpdateAnimationState(AgentStatus state)
    {
        switch(state) 
        {
            case AgentStatus.Walking:
                animator.SetBool("isWalking", true);
                break;

            case AgentStatus.Running:
                animator.SetBool("isRunning", true);
                break;

            case AgentStatus.Victory:
                animator.SetBool("isVictory", true);
                animator.SetBool("isDead", false);
                animator.SetBool("isRunning", false);
                animator.SetBool("isWalking", false);
                break;

            case AgentStatus.Dead:
                animator.SetBool("isVictory", false);
                animator.SetBool("isDead", true);
                animator.SetBool("isRunning", false);
                animator.SetBool("isWalking", false);
                startBloodSpash();
                break;

            case AgentStatus.Idle:
                animator.SetBool("isVictory", false);
                animator.SetBool("isDead", false);
                animator.SetBool("isRunning", false);
                animator.SetBool("isWalking", false);
                break;
            
            default:
                break;

        }
    }

    private void startBloodSpash()
    {

        blood.Play();

        StartCoroutine(Delay(1.0f));
    }

    IEnumerator Delay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        blood.Pause();
    }

}
