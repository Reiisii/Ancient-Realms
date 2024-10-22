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

        if(Utilities.npcHasQuest(npcData)){
            // Default response
            currentStory = new Story(npc.npcDialogue.text);
            currentStory.ChoosePathString(npc.dialogueKnot);
            // Check if NPC has talk quest in their active quests THIS WILL THEORETICALLY WILL PLAY EXHAUST

            // Check if NPC has giveable quest and is can be given
            foreach(string quest in npc.giveableQuest){
                QuestSO giveableQuests = AccountManager.Instance.quests.Find(q => q.questID == quest);
                QuestSO playerHasActiveQuest = PlayerStats.GetInstance().activeQuests.FirstOrDefault(q => q.questID == giveableQuests.questID);
                QuestSO playerHasCompletedQuest = PlayerStats.GetInstance().completedQuests.FirstOrDefault(q => q.questID == giveableQuests.questID);
                // if quest is not on completed and not on active quest list STOP loop and give quest
                if(giveableQuests != null && Utilities.CheckRequirements(giveableQuests) && playerHasActiveQuest == null && playerHasCompletedQuest == null){
                    currentStory = new Story(giveableQuests.dialogue.text);
                    currentStory.ChoosePathString("start");
                    nameText.SetText(npc.name);
                    npcImage.sprite = npc.portrait;
                    dialogueIsPlaying = true;
                    dialoguePanel.SetActive(true);
                    ContinueStory();
                    return;
                }
            }
            var relevantQuests = PlayerStats.GetInstance().activeQuests
            .Where(q => q.goals.Any(g => g.goalType == GoalTypeEnum.Talk && 
                                            q.characters[g.characterIndex] == npcData.id))
            .ToList();
            foreach(QuestSO quest in PlayerStats.GetInstance().activeQuests.ToList()){
                if(quest.characters.Contains(npcData.id) && quest.characters[quest.goals[quest.currentGoal].characterIndex] == npcData.id){
                    currentStory = new Story(quest.dialogue.text);
                    currentStory.ChoosePathString(quest.goals[quest.currentGoal].inkyRedirect);
                    nameText.SetText(npc.name);
                    npcImage.sprite = npc.portrait;
                    dialogueIsPlaying = true;
                    dialoguePanel.SetActive(true);
                    ContinueStory();
                    return;
                }
            }
            nameText.SetText(npc.name);
            npcImage.sprite = npc.portrait;
            dialogueIsPlaying = true;
            dialoguePanel.SetActive(true);
            ContinueStory();
        }else{
            List<QuestSO> activeQuest = PlayerStats.GetInstance().activeQuests.ToList();

            foreach(QuestSO quest in activeQuest){
                if(quest.characters.Contains(npcData.id) && quest.characters[quest.goals[quest.currentGoal].characterIndex] == npcData.id){
                    currentStory = new Story(quest.dialogue.text);
                    currentStory.ChoosePathString(quest.goals[quest.currentGoal].inkyRedirect);
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
        PlayerController.GetInstance().playerActionMap.Enable();
        PlayerController.GetInstance().dialogueActionMap.Disable();
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        var relevantQuests = PlayerStats.GetInstance().activeQuests
        .Where(q => q.goals.Any(g => g.goalType == GoalTypeEnum.Talk && 
                                        q.characters[g.characterIndex] == npcData.id))
        .ToList();

        if (Utilities.npcHasQuest(npcData))
        {
            foreach(string quest in npcData.giveableQuest){
                QuestSO giveableQuests = AccountManager.Instance.quests.Find(q => q.questID == quest);
                QuestSO playerHasActiveQuest = PlayerStats.GetInstance().activeQuests.FirstOrDefault(q => q.questID == giveableQuests.questID);
                QuestSO playerHasCompletedQuest = PlayerStats.GetInstance().completedQuests.FirstOrDefault(q => q.questID == giveableQuests.questID);
                
                
                if(giveableQuests != null && Utilities.CheckRequirements(giveableQuests) && playerHasActiveQuest == null && playerHasCompletedQuest == null){
                    QuestManager.GetInstance().StartQuest(giveableQuests.questID);
                    return;
                }

            }
        }
        if(relevantQuests.Count > 0){
            if (relevantQuests[0].isActive)
            {
                Goal currentGoal = relevantQuests[0].goals[relevantQuests[0].currentGoal];
                if (currentGoal.goalType == GoalTypeEnum.Talk && relevantQuests[0].characters[currentGoal.characterIndex] == npcData.id)
                {
                    QuestManager.GetInstance().UpdateTalkGoal(relevantQuests[0]);
                }
            }
        }
    }
    private void ContinueStory(){
        if(currentStory.canContinue){
            string nextLine = currentStory.Continue();
            nextLine = ReplacePlayerName(nextLine);
            dialogueText.text = nextLine;
        }else{
            ExitDialogueMode();
        }
    }

    private string ReplacePlayerName(string text)
    {
        // Retrieve the player name from PlayerStats
        string playerName = PlayerStats.GetInstance().localPlayerData.gameData.playerName;

        // Replace the placeholder in the text
        return text.Replace("[player]", playerName);
    }

}
