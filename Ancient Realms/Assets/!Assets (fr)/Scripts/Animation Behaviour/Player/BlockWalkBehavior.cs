using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockWalkBehavior : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerController.GetInstance().isBlocking = true;
        PlayerController.GetInstance().canWalk = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(PlayerController.GetInstance().isAttacking){
            PlayerController.GetInstance().isBlocking = false;
            animator.Play("Combat Shield Bash");
            PlayerController.GetInstance().canWalk = false;
        }
        if(PlayerStats.GetInstance().stamina < 1){
            PlayerController.GetInstance().isBlocking = false;
            animator.SetBool("isBlocking", false);

        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerController.GetInstance().isAttacking = false;
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