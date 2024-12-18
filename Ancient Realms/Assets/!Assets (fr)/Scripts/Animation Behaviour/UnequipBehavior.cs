using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnequipBehavior : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!PlayerStats.GetInstance().isCombatMode && !animator.GetBool("isCombatMode") && PlayerController.GetInstance().isEquipping){
            PlayerController.GetInstance().canWalk = false;
            PlayerController.GetInstance().IsRunning = false;
            PlayerController.GetInstance().IsMoving = false;
            animator.Play("Unequip");
        }
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
