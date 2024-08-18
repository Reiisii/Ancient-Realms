using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilumAimStatic : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    // override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {
    //     PlayerController.GetInstance().isHolding = true;
    // }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerController.GetInstance().isHolding)
        {
            PlayerController.GetInstance().holdTime += Time.deltaTime;
            PlayerController.GetInstance().holdTime = Mathf.Min(PlayerController.GetInstance().holdTime, PlayerStats.GetInstance().maxHoldTime); // Cap the hold time to the max hold time
        }
        if(!PlayerController.GetInstance().isHolding){
            animator.Play("Pilum Throw");
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