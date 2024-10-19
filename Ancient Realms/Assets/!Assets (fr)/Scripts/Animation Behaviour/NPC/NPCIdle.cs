using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIdle : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {   
        Ally ally = animator.gameObject.GetComponent<Ally>();
        if(ally != null){
            ally.isEquipping = false;
            ally.canMove = true;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Ally ally = animator.gameObject.GetComponent<Ally>();
        if(ally != null){
            if(ally.isCombatMode && animator.GetBool("isCombatMode") && ally.isEquipping){
                ally.canMove = false;
                ally.isRunning = false;
                ally.IsMoving = false;
                animator.Play("Equip");
            }
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