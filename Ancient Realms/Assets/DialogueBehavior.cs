using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBehavior : StateMachineBehaviour
{
    private string[] talkAnimations = { "Talk_1", "Talk_2", "Talk_3", "Talk_4" };
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(DialogueManager.GetInstance().dialogueIsPlaying){
            int randomIndex = Random.Range(0, talkAnimations.Length);  // Choose a random index from 0 to 3
            string randomAnimation = talkAnimations[randomIndex];  // Get the corresponding animation name
            animator.Play(randomAnimation);
        }else{
            animator.SetBool("isDialogue", false);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!DialogueManager.GetInstance().dialogueIsPlaying){
            animator.SetBool("isDialogue", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    // override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {

    // }

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
