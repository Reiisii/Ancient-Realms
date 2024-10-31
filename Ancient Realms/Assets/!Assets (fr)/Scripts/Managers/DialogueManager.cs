using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;
using System.Linq;
using ESDatabase.Classes;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI npcNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image npcImage;
    [SerializeField] private Sprite playerSprite;
    [SerializeField] private GameObject playerPortrait;
    [SerializeField] private GameObject npcPortrait;
    public float typingSpeed = 0.03f;
    public bool isTyping = false;
    private Coroutine typingCoroutine;
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
        if (PlayerController.GetInstance().GetInteractPressed())
        {
            if (isTyping) // If currently typing
            {
                // Complete the current line instantly
                CompleteCurrentLine();
            }
            else
            {
                ContinueStory(); // Otherwise, continue to the next line
            }
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
                    if(quest.goals[quest.currentGoal].goalType.Equals(GoalTypeEnum.Deliver) && CheckItemRequirements(quest.goals[quest.currentGoal])){
                        currentStory = new Story(quest.dialogue.text);
                        currentStory.ChoosePathString(quest.goals[quest.currentGoal].inkyNoRequirement);
                    }else{
                    currentStory = new Story(quest.dialogue.text);
                    currentStory.ChoosePathString(quest.goals[quest.currentGoal].inkyRedirect);
                    }
                    dialogueIsPlaying = true;
                    dialoguePanel.SetActive(true);
                    ContinueStory();
                    return;
                }
            }
            dialogueIsPlaying = true;
            dialoguePanel.SetActive(true);
            ContinueStory();
        }else{
            List<QuestSO> activeQuest = PlayerStats.GetInstance().activeQuests.ToList();

            foreach(QuestSO quest in activeQuest){
                if(quest.characters.Contains(npcData.id) && quest.characters[quest.goals[quest.currentGoal].characterIndex] == npcData.id){
                    if(quest.goals[quest.currentGoal].goalType.Equals(GoalTypeEnum.Deliver) && !CheckItemRequirements(quest.goals[quest.currentGoal])){
                        currentStory = new Story(quest.dialogue.text);
                        currentStory.ChoosePathString(quest.goals[quest.currentGoal].inkyNoRequirement);
                    }else{
                        currentStory = new Story(quest.dialogue.text);
                        currentStory.ChoosePathString(quest.goals[quest.currentGoal].inkyRedirect);
                    }
                    dialogueIsPlaying = true;
                    dialoguePanel.SetActive(true);
                    ContinueStory();
                    return;
                }
            }
            currentStory = new Story(npc.npcDialogue.text);
            currentStory.ChoosePathString(npc.dialogueKnot);
            dialogueIsPlaying = true;
            dialoguePanel.SetActive(true);
            ContinueStory();
        }
        
    }
    public void ExitDialogueMode(){
        dialogueIsPlaying = false;
        PlayerController.GetInstance().playerActionMap.Enable();
        PlayerController.GetInstance().dialogueActionMap.Disable();
        npcData.gameObject.GetComponent<Animator>().SetBool("isDialogue", false);
        PlayerController.GetInstance().animator.SetBool("isDialogue", false);
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        var relevantQuests = PlayerStats.GetInstance().activeQuests
        .Where(q => q.goals.Any(g => (g.goalType == GoalTypeEnum.Talk || g.goalType == GoalTypeEnum.Deliver) && 
                                        q.characters[g.characterIndex].Equals(npcData.id)))
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
                    if(currentGoal.questItem.Count > 0){
                        foreach(int item in currentGoal.questItem){
                            PlayerStats.GetInstance().AddItem(item, 0, 0, 1);
                        }
                    }
                    QuestManager.GetInstance().UpdateTalkGoal(relevantQuests[0]);
                }else if(currentGoal.goalType == GoalTypeEnum.Deliver && CheckItemRequirements(currentGoal) && relevantQuests[0].characters[currentGoal.characterIndex] == npcData.id){
                    QuestManager.GetInstance().UpdateDeliverGoal(relevantQuests[0]);
                    foreach(int itm in currentGoal.requiredItems){
                        for(int i = 0; i < PlayerStats.GetInstance().localPlayerData.gameData.inventory.items.Count; i++){
                            if(PlayerStats.GetInstance().localPlayerData.gameData.inventory.items[i].equipmentId.Equals(itm)){
                                PlayerStats.GetInstance().localPlayerData.gameData.inventory.items.RemoveAt(i);
                            }
                        }
                    }
                    
                }
            }
        }
    }
    private void ContinueStory(){
        if(currentStory.canContinue){
            string nextLine = currentStory.Continue();
            nextLine = ReplacePlayerName(nextLine);
            StartTypingEffect(nextLine);
            HandleTags(currentStory.currentTags);
        }else{
            ExitDialogueMode();
        }
    }
    public bool CheckItemRequirements(Goal goal)
    {
        List<EquipmentSO> itemList = PlayerStats.GetInstance().inventory;

        // Check if all items in goal.requiredItems are in the player's inventory
        foreach (int val in goal.requiredItems)
        {
            EquipmentSO equipmentSO = itemList.FirstOrDefault(q => q.equipmentId.Equals(val));
            
            // If any required item is missing, return false immediately
            if (equipmentSO == null)
            {
                return false;
            }
        }

        // All items are present
        return true;
    }

    private void HandleTags(List<string> currentTags){
        foreach(string tag in currentTags){
            string[] splitTag = tag.Split(':');

            if(splitTag.Length != 2){
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }
            string tagValue = splitTag[1].Trim();
            if(tagValue.Equals("player")){
                playerPortrait.SetActive(true);
                npcPortrait.SetActive(false);
                string playerName = PlayerStats.GetInstance().localPlayerData.gameData.playerName;
                nameText.SetText(playerName);
            }else{
                playerPortrait.SetActive(false);
                npcPortrait.SetActive(true);
                npcNameText.SetText(npcData.name);
                npcImage.sprite = npcData.portrait;
            }
        }
    }
    private void StartTypingEffect(string text)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine); // Stop any previous typing effect
        }
        typingCoroutine = StartCoroutine(TypeText(text)); // Start new typing effect
    }

    private IEnumerator TypeText(string text)
    {
        dialogueText.text = ""; // Clear previous text
        isTyping = true; // Set typing state

        foreach (char character in text)
        {
            dialogueText.text += character; // Add one character at a time
            yield return new WaitForSeconds(typingSpeed); // Wait for the typing speed duration
        }

        isTyping = false; // Finished typing
    }

    private void CompleteCurrentLine()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine); // Stop typing effect
        }
        string fullText = ReplacePlayerName(currentStory.currentText);
        dialogueText.text = fullText; // Set text to the full current line
        isTyping = false; // No longer typing
    }

    private string ReplacePlayerName(string text)
    {
        // Retrieve the player name from PlayerStats
        string playerName = PlayerStats.GetInstance().localPlayerData.gameData.playerName;

        // Replace the placeholder in the text
        return text.Replace("[player]", playerName);
    }

}
