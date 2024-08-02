using System.Collections;
using System.Collections.Generic;
using Ink.Parsed;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("NPC Details")]
    [SerializeField] private string npcName;
    [SerializeField] private Sprite npcIcon;
    [SerializeField] private string currentKnot;
    [SerializeField] private List<string> quests;

    [Header("Visual Cue")]
    [SerializeField] private GameObject VisualCue;
    [Header("Ink JSON")]
    [SerializeField] private TextAsset dialogue;
    [SerializeField] private PlayerController playerController;
    private bool playerInRange;
    private Story currentStory;
    private bool dialogueIsPlaying;
    private void Awake(){
        VisualCue.SetActive(false);
        playerInRange = false;
    }
    private void Update(){
        if(playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying){
            VisualCue.SetActive(true);
            if(playerController.GetInteractPressed()){
                NPCData npcData = new NPCData
                {
                    name = npcName,
                    portrait = npcIcon,
                    dialogueKnot = currentKnot,
                    npcDialogue = dialogue,
                    giveableQuest = quests
                };
                DialogueManager.GetInstance().EnterDialogueMode(npcData);
            }
        }else{
            VisualCue.SetActive(false);
        }

    }
    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            playerInRange = false;
        }
    }
}
