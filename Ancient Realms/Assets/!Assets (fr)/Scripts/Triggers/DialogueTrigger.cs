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
            QuestSO activeQuest = PlayerStats.GetInstance().activeQuests.Find(quest => quest.questID == npcData.giveableQuest[0]);
            QuestSO completedQuest = PlayerStats.GetInstance().completedQuests.Find(quest => quest.questID == npcData.giveableQuest[0]);
            if(activeQuest == null && completedQuest == null){
                QuestSO quest = QuestManager.GetInstance().quests.Find(quest => quest.questID == npcData.giveableQuest[0]);
                return !quest.isActive && !quest.isCompleted;
            }else{
                return false;
            }
            
            
        }else{
            return false;
        }
        
    }
    public bool completion()
    {
        List<QuestSO> activeQuest = PlayerStats.GetInstance().activeQuests.ToList();

        // Ensure the NPC has quests to give
        if (npcData.giveableQuest.Count > 0)
        {
            QuestSO quest = PlayerStats.GetInstance().activeQuests
                            .Find(q => q.characters != null && q.goals != null &&
                                    q.currentGoal < q.goals.Count &&
                                    q.characters[q.goals[q.currentGoal].characterIndex] == npcData.id);

            if (quest != null)
            {
                return !quest.isCompleted && quest.isActive && (quest.currentGoal + 1) == quest.goals.Count;
            }
        }
        else
        {
            QuestSO quest = PlayerStats.GetInstance().activeQuests
                            .Find(q => q.characters != null && q.goals != null &&
                                    q.currentGoal < q.goals.Count &&
                                    q.characters[q.goals[q.currentGoal].characterIndex] == npcData.id);

            if (quest != null)
            {
                return !quest.isCompleted && quest.isActive && (quest.currentGoal + 1) == quest.goals.Count;
            }
        }

        return false;
    }
    private void FlipPlayerSprite()
    {
        if (playerInRange)
        {
            if (PlayerController.GetInstance().gameObject.transform.position.x < transform.position.x)
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
