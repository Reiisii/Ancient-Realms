using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIdle : StateMachineBehaviour
{
    private string[] talkAnimations = { "Talk_1", "Talk_2", "Talk_3", "Talk_4" };
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {   
        // Ally ally = animator.gameObject.GetComponent<Ally>();
        // Enemy enemy = animator.gameObject.GetComponent<Enemy>();
        // if(ally != null){
        //     ally.isEquipping = false;
        //     ally.canMove = true;
        // }else if(enemy != null){
        //     enemy.isEquipping = false;
        //     enemy.canMove = true;
        // }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Ally ally = animator.gameObject.GetComponent<Ally>();
        Enemy enemy = animator.gameObject.GetComponent<Enemy>();
        if(DialogueManager.GetInstance().dialogueIsPlaying && animator.GetBool("isDialogue")){
            int randomIndex = Random.Range(0, talkAnimations.Length);  // Choose a random index from 0 to 3
            string randomAnimation = talkAnimations[randomIndex];  // Get the corresponding animation name
            animator.SetBool("isDialogue", true);
            animator.Play(randomAnimation);
        }else{
            animator.SetBool("isDialogue", false);
        }
        if(ally != null){
            if(ally.isCombatMode && animator.GetBool("isCombatMode") && ally.isEquipping){
                ally.canMove = false;
                ally.isRunning = false;
                ally.IsMoving = false;
                animator.Play("Equip AI");
            }
        }else if(enemy != null){
            if(enemy.isCombatMode && animator.GetBool("isCombatMode") && enemy.isEquipping){
                enemy.canMove = false;
                enemy.isRunning = false;
                enemy.IsMoving = false;
                animator.Play("Equip AI");
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
