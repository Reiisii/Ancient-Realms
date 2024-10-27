using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatIdleBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerController.GetInstance().canWalk = true;
        // PlayerController.GetInstance().isEquipping = false;
        PlayerController.GetInstance().isThrowing = false;
        PlayerController.GetInstance().isBlocking = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!PlayerStats.GetInstance().isCombatMode && !animator.GetBool("isCombatMode") && PlayerController.GetInstance().isEquipping){
            PlayerController.GetInstance().canWalk = false;
            PlayerController.GetInstance().IsRunning = false;
            PlayerController.GetInstance().IsMoving = false;
            animator.Play("Unequip");
        }
        if(PlayerController.GetInstance().isAttacking && !PlayerController.GetInstance().isHolding){
            animator.Play("Roman Normal Attack 1");
            PlayerController.GetInstance().canWalk = false;
        }
        if(!PlayerController.GetInstance().isAttacking && PlayerController.GetInstance().isHolding){
            animator.Play("Pilum Aim");
            PlayerController.GetInstance().canWalk = false;
            PlayerController.GetInstance().isHolding = true;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state

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
