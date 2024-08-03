using System.Collections;
using System.Collections.Generic;
using Ink.Parsed;
using Microsoft.Unity.VisualStudio.Editor;
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
    [SerializeField] private SpriteRenderer icon;
    [SerializeField] private Sprite bubbleMessage;
    [SerializeField] private Sprite scroll;
    [SerializeField] private Sprite questIcon;
    [Header("Ink JSON")]
    [SerializeField] private TextAsset dialogue;
    [SerializeField] private PlayerController playerController;
    private bool playerInRange;
    private Story currentStory;
    private bool dialogueIsPlaying;
    public NPCData npcData;
    private void Awake(){
        VisualCue.SetActive(false);
        playerInRange = false;
        npcData = new NPCData
                {
                    name = npcName,
                    portrait = npcIcon,
                    dialogueKnot = currentKnot,
                    npcDialogue = dialogue,
                    giveableQuest = quests
                };
    }
    private void Update(){
        if(playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying){
            setVisualCue();
            VisualCue.SetActive(true);
            if(playerController.GetInteractPressed()){
                
                
                DialogueManager.GetInstance().EnterDialogueMode(npcData);
            }
        }else{
            VisualCue.SetActive(false);
        }

    }
    public void setVisualCue(){
            if(hasMainQuest()){
                    icon.sprite = questIcon;
            }else if(completion()){
                    icon.sprite = scroll;
            }else{
                    icon.sprite = bubbleMessage;
            }
    }
    public bool hasMainQuest(){
        QuestSO quest = QuestManager.GetInstance().quests.Find(quest => quest.questID == npcData.giveableQuest[0]);
        return !quest.isActive && !quest.isCompleted;
    }
    public bool completion(){
        QuestSO quest = QuestManager.GetInstance().quests.Find(quest => quest.questID == npcData.giveableQuest[0]);
        return !quest.isCompleted && quest.isActive && (quest.currentGoal + 1) == quest.goals.Count;
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
