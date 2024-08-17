using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ink.Parsed;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("NPC Details")]
    [SerializeField] private string npcID;
    [SerializeField] private string npcName;
    [SerializeField] private Sprite npcIcon;
    [SerializeField] private string currentKnot;
    [SerializeField] private List<string> quests;

    [Header("Visual Cue")]
    [SerializeField] private GameObject VisualCue;
    [SerializeField] private GameObject VisualCueKey;
    [SerializeField] private SpriteRenderer icon;
    [SerializeField] private Sprite bubbleMessage;
    [SerializeField] private Sprite scroll;
    [SerializeField] private Sprite questIcon;
    [Header("Ink JSON")]
    [SerializeField] private TextAsset dialogue;
    [SerializeField] private SpriteRenderer npcSpriteRenderer;
    private bool playerInRange;
    private Story currentStory;
    private bool dialogueIsPlaying;
    public NPCData npcData;
    private bool initialFlipX;
    private void Awake(){
        VisualCue.SetActive(false);
        VisualCueKey.SetActive(false);
        playerInRange = false;
        
    }
    private void Start(){
        npcData = new NPCData
                {
                    id = npcID,
                    name = npcName,
                    portrait = npcIcon,
                    dialogueKnot = currentKnot,
                    npcDialogue = dialogue,
                    giveableQuest = quests
                };
        initialFlipX = npcSpriteRenderer.flipX;;
    }
    private void Update(){
        if(playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying){
            VisualCue.SetActive(true);
            VisualCueKey.SetActive(true);
            setVisualCue();
            if(PlayerController.GetInstance().GetInteractPressed()){
                DialogueManager.GetInstance().EnterDialogueMode(npcData);
                FlipPlayerSprite();
            }
        }else{
            VisualCue.SetActive(false);
            VisualCueKey.SetActive(false);
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
        if(npcData.giveableQuest.Count > 0){
            QuestSO quest = QuestManager.GetInstance().quests.Find(quest => quest.questID == npcData.giveableQuest[0]);
            return !quest.isActive && !quest.isCompleted;
        }else{
            return false;
        }
        
    }
    public bool completion(){
        List<QuestSO> activeQuest = PlayerStats.GetInstance().activeQuests.ToList();
        bool isInActiveQuest = activeQuest.Any(quest => quest.characters.Contains(npcData.id));
        if(npcData.giveableQuest.Count > 0){    
            if(PlayerStats.GetInstance().activeQuests.Find(quest => quest.characters[quest.goals[quest.currentGoal].characterIndex] == npcData.id)){
                QuestSO quest = QuestManager.GetInstance().quests.Find(quest => quest.questID == npcData.giveableQuest[0]);
                return !quest.isCompleted && quest.isActive && (quest.currentGoal + 1) == quest.goals.Count;
            }else{
                return false;
            }
        }else if(PlayerStats.GetInstance().activeQuests.Find(quest => quest.characters[quest.goals[quest.currentGoal].characterIndex] == npcData.id)){
            QuestSO quest = activeQuest.Find(quest => quest.characters.Contains(npcData.id));
            return !quest.isCompleted && quest.isActive && (quest.currentGoal + 1) == quest.goals.Count;
        }else{
            return false;
        }
        
    }
    private void FlipPlayerSprite()
    {
        if (playerInRange)
        {
            if (PlayerStats.GetInstance().gameObject.transform.position.x < transform.position.x)
            {
                npcSpriteRenderer.flipX = true; // Player is on the left side of the NPC
            }
            else
            {
                npcSpriteRenderer.flipX = false; // Player is on the right side of the NPC
            }
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
            npcSpriteRenderer.flipX = initialFlipX;
        }
    }
}
