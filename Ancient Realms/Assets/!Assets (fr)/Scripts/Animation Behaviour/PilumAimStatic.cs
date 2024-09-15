using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilumAimStatic : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerController.GetInstance().forceGO.SetActive(true);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerController.GetInstance().isHolding)
        {
            PlayerController.GetInstance().forceSlider.value = PlayerController.GetInstance().holdTime;
            PlayerController.GetInstance().holdTime += Time.deltaTime;
            PlayerController.GetInstance().holdTime = Mathf.Min(PlayerController.GetInstance().holdTime, PlayerStats.GetInstance().maxHoldTime); // Cap the hold time to the max hold time
            PlayerController.GetInstance().PanCameraBasedOnPlayerDirection();
        }
        if(!PlayerController.GetInstance().isHolding){
            animator.Play("Pilum Throw");
            PlayerController.GetInstance().forceGO.SetActive(false);
            PlayerController.GetInstance().forceSlider.value = 0f;
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
