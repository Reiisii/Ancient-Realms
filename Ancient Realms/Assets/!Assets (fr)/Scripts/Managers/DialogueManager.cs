using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;
using System.Linq;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image npcImage;
    private static DialogueManager instance;
    private Story currentStory;
    private QuestSO[] questArray;
    public bool dialogueIsPlaying { get; private set;}
    private NPCData npcData;
    private void Awake(){
        if(instance != null){
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;
    }

    public static DialogueManager GetInstance(){
        return instance;
    }

    private void Start(){
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
    }
    private void Update(){
        if(!dialogueIsPlaying){
            return;
        }
        if(PlayerController.GetInstance().GetInteractPressed()){
            ContinueStory();
        }
    }
    public void EnterDialogueMode(NPCData npc){
        npcData = npc;
        if(npc.giveableQuest.Count > 0){
            QuestSO questData = QuestManager.GetInstance().quests.Find(quest => quest.questID == npc.giveableQuest[0]);
            QuestSO activeQuest = PlayerStats.GetInstance().activeQuests.Find(quest => quest.questID == npc.giveableQuest[0]);
            if(questData.isCompleted && !questData.isActive){
                // If Quest is complete AND is not Active
                currentStory = new Story(npcData.npcDialogue.text);
                currentStory.ChoosePathString(npcData.dialogueKnot);
            }else if(!activeQuest){
                // If NPC Quest IS NOT existing on active quest list
                currentStory = new Story(questData.dialogue.text);
                currentStory.ChoosePathString(questData.currentKnot);
            }else if(activeQuest && PlayerStats.GetInstance().activeQuests.Find(quest => quest.questID == npc.giveableQuest[0]).isActive && PlayerStats.GetInstance().activeQuests.Find(quest => quest.characters[quest.goals[quest.currentGoal].characterIndex] == npcData.id)){
                // IF NPC's Quest is in ActiveQuest AND the quest is Active AND is Talking to the NPC that has the talk quest goal
                currentStory = new Story(questData.dialogue.text);
                currentStory.ChoosePathString(activeQuest.currentKnot);
            }else{
                currentStory = new Story(npc.npcDialogue.text);
                currentStory.ChoosePathString(npc.dialogueKnot);
            }
            nameText.SetText(npc.name);
            npcImage.sprite = npc.portrait;
            dialogueIsPlaying = true;
            dialoguePanel.SetActive(true);
            ContinueStory();
        }else{
            List<QuestSO> activeQuest = PlayerStats.GetInstance().activeQuests.ToList();

            foreach(QuestSO quest in activeQuest){
                if(quest.characters.Contains(npcData.id)){
                    currentStory = new Story(quest.dialogue.text);
                    currentStory.ChoosePathString(quest.currentKnot);
                    nameText.SetText(npc.name);
                    npcImage.sprite = npc.portrait;
                    dialogueIsPlaying = true;
                    dialoguePanel.SetActive(true);
                    ContinueStory();
                    return;
                }
            }
            currentStory = new Story(npc.npcDialogue.text);
            currentStory.ChoosePathString(npc.dialogueKnot);
            nameText.SetText(npc.name);
            npcImage.sprite = npc.portrait;
            dialogueIsPlaying = true;
            dialoguePanel.SetActive(true);
            ContinueStory();
        }
        
    }
    public void ExitDialogueMode(){
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        var relevantQuests = PlayerStats.GetInstance().activeQuests
        .Where(q => q.goals.Any(g => g.goalType == GoalTypeEnum.Talk && 
                                        q.characters[g.characterIndex] == npcData.id))
        .ToList();

        if (Utilities.npcHasQuest(npcData))
        {
            QuestSO questToGive = QuestManager.GetInstance().quests
                .FirstOrDefault(q => q.questID == npcData.giveableQuest.FirstOrDefault());

            if (questToGive != null && !questToGive.isActive && !questToGive.isCompleted)
            {
                // Start the quest if it's not already active or completed
                QuestManager.GetInstance().StartQuest(questToGive.questID);
            }
        }
        
            // Update talk goals for each relevant quest
            foreach (var quest in relevantQuests)
            {
                if (quest.isActive)
                {
                    Goal currentGoal = quest.goals[quest.currentGoal];
                    if (currentGoal.goalType == GoalTypeEnum.Talk && 
                        quest.characters[currentGoal.characterIndex] == npcData.id)
                    {
                        
                        QuestManager.GetInstance().UpdateTalkGoal(quest);
                    }
                }
            }
    
    }
    private void ContinueStory(){
        if(currentStory.canContinue){
            dialogueText.text = currentStory.Continue();
        }else{
            ExitDialogueMode();
        }
    }
}
