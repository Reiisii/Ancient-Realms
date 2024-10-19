using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCBlockWalk : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Ally ally = animator.gameObject.GetComponent<Ally>();
        ally.isBlocking = true;
        ally.canMove = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Ally ally = animator.gameObject.GetComponent<Ally>();
        if(ally.isAttacking){
            ally.isBlocking = false;
            animator.Play("Combat Shield Bash");
            ally.canMove = false;
        }
        if(ally.stamina < 1){
            ally.isBlocking = false;
            animator.SetBool("isBlocking", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Ally ally = animator.gameObject.GetComponent<Ally>();
        ally.isAttacking = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
