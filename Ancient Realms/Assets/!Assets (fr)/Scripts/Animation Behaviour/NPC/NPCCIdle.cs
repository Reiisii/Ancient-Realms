using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCIdle : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Ally ally = animator.gameObject.GetComponent<Ally>();
        ally.canMove = true;
        ally.isAttacking = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Ally ally = animator.gameObject.GetComponent<Ally>();
        if(!ally.isCombatMode && !animator.GetBool("isCombatMode") && ally.isEquipping){
            ally.canMove = false;
            ally.isRunning = false;
            ally.IsMoving = false;
            animator.Play("Unequip");
        }
        if(animator.gameObject.GetComponent<Ally>().isAttacking && !animator.gameObject.GetComponent<Ally>().isHolding){
            animator.Play("R Attack AI");
            ally.canMove = false;
        }
        if(!animator.gameObject.GetComponent<Ally>().isAttacking && animator.gameObject.GetComponent<Ally>().isHolding && ally.isJavelin){
            animator.Play("Pilum Aim");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
