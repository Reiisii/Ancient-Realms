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
    [SerializeField] public PlayerController playerController;
    private static DialogueManager instance;
    [SerializeField] public QuestManager questManager;
    private Story currentStory;
    private QuestSO[] questArray;
    public bool dialogueIsPlaying { get; private set;}
    NPCData npcData;
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
        if(playerController.GetInteractPressed()){
            ContinueStory();
        }
    }
    public void EnterDialogueMode(NPCData npc){
        QuestSO questData = QuestManager.GetInstance().quests.Find(quest => quest.questID == npc.giveableQuest[0]);
        npcData = npc;
        if(questData.isCompleted && !questData.isActive){
            currentStory = new Story(npcData.npcDialogue.text);
            currentStory.ChoosePathString(npcData.dialogueKnot);
        }else if(!PlayerStats.GetInstance().activeQuests.Find(quest => quest.questID == npc.giveableQuest[0])){
            currentStory = new Story(questData.dialogue.text);
            currentStory.ChoosePathString(questData.currentKnot);
        }else if(PlayerStats.GetInstance().activeQuests.Find(quest => quest.questID == npc.giveableQuest[0]) && PlayerStats.GetInstance().activeQuests.Find(quest => quest.questID == npc.giveableQuest[0]).isActive){
            QuestSO quest = PlayerStats.GetInstance().activeQuests.Find(quest => quest.questID == npc.giveableQuest[0]);
            currentStory = new Story(questData.dialogue.text);
            currentStory.ChoosePathString(quest.currentKnot);
        }
        nameText.SetText(npc.name);
        npcImage.sprite = npc.portrait;
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        ContinueStory();
    }
    public void ExitDialogueMode(){
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        QuestSO quest = QuestManager.GetInstance().quests.Find(quest => quest.questID == npcData.giveableQuest[0]);
        if(quest.isActive == false && !quest.isCompleted){
            QuestManager.GetInstance().StartQuest(quest.questID);
        }
        
        QuestManager.GetInstance().UpdateTalkGoal();
    }
    private void ContinueStory(){
        if(currentStory.canContinue){
            dialogueText.text = currentStory.Continue();
        }else{
            ExitDialogueMode();
        }
    }
}
