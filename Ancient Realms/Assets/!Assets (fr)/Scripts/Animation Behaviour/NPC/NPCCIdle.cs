using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCIdle : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Ally ally = animator.gameObject.GetComponent<Ally>();
        Enemy enemy = animator.gameObject.GetComponent<Enemy>();
        if(ally != null){
            ally.canMove = true;
            ally.isAttacking = false;
        }else{
            enemy.canMove = true;
            enemy.isAttacking = false;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Ally ally = animator.gameObject.GetComponent<Ally>();
        Enemy enemy = animator.gameObject.GetComponent<Enemy>();
        if(ally != null){
            if(!ally.isCombatMode && !animator.GetBool("isCombatMode") && ally.isEquipping){
                ally.canMove = false;
                ally.isRunning = false;
                ally.IsMoving = false;
                animator.Play("Unequip");
            }
            if(ally.isAttacking && !ally.isHolding){
                if(ally.equippedSOs[4].weaponType == WeaponType.Sword){
                    animator.Play("R Attack AI");
                }else if(ally.equippedSOs[4].weaponType == WeaponType.Spear){
                    animator.Play("R Attack AI");
                }
                ally.canMove = false;
            }
            if(!ally.isAttacking && ally.isHolding && ally.isJavelin){
                animator.Play("Pilum Aim");
            }
        }else{
            if(!enemy.isCombatMode && !animator.GetBool("isCombatMode") && enemy.isEquipping){
                enemy.canMove = false;
                enemy.isRunning = false;
                enemy.IsMoving = false;
                animator.Play("Unequip");
            }
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
