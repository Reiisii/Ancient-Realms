using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JournalManager : MonoBehaviour
{
    [Header("Quest List")]
    [SerializeField] RectTransform questListPanel;
    [SerializeField] JournalListTitle journalListTitle;
    [SerializeField] JournalQuestGoal journalQuestGoal;
    [Header("Active Quest")]
    [SerializeField] TextMeshProUGUI questTitleText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] Button pinButton;
    [SerializeField] TextMeshProUGUI pinTypeText;
    [SerializeField] RectTransform objectiveListPanel;
    public QuestSO displayedQuest;
    public List<QuestSO> mainQuest;
    [SerializeField] List<QuestSO> mainQuestList = new List<QuestSO>();
    [SerializeField] List<QuestSO> subQuestList = new List<QuestSO>();
    [SerializeField] List<QuestSO> completedQuestList = new List<QuestSO>();
    [SerializeField] Button mainButton;
    [SerializeField] Button subButton;
    [SerializeField] Button completeButton;
    private static JournalManager Instance;
    public QuestType currentQuestType = QuestType.Main;
    public enum QuestType
    {
        Main,
        Sub,
        Completed
    }
    private void Awake()
    {
        if(Instance != null){
            Debug.LogWarning("Found more than one Journal Manager in the scene");
        }
        Instance = this;
        currentQuestType = QuestType.Main;
    }
    public static JournalManager GetInstance(){
        return Instance;
    }
    public void ShowQuestDetails(QuestSO quest){
        ClearContent(objectiveListPanel);
        displayedQuest = quest;
        questTitleText.SetText(quest.questTitle);
        descriptionText.SetText(quest.questDescription);
        foreach(Goal goal in quest.goals){
            if(goal.currentAmount > goal.requiredAmount || goal.goalID == quest.currentGoal){
                JournalQuestGoal questGoalPrefab = Instantiate(journalQuestGoal, Vector3.zero, Quaternion.identity);
                questGoalPrefab.transform.SetParent(objectiveListPanel);
                questGoalPrefab.transform.localScale = Vector3.one;
                questGoalPrefab.setQuestSO(quest, goal);
            }
        }
        if(quest.isCompleted){
            pinButton.interactable = false;
        }else{
            pinButton.interactable = true;
        }
        if(quest.isPinned){
            pinTypeText.SetText("Unpin Quest");
        }else{
            pinTypeText.SetText("Pin Quest");
        }
    }
    private void Start()
    {
        InitializeJournal();
    }
    private void OnEnable(){
        // if(mainQuestList.Count > 0){
        //     pinButton.interactable = true;
        // }else{
        //     pinButton.interactable = false;
        // }
        InitializeJournal();
    }
    private void InitializeJournal()
    {
        // Ensure the quest type is set to Main
        
        // Update the lists of quests
        UpdateQuestLists();

        ClearContent(questListPanel);
        UpdateQuestBoard();
        DisableButton(currentQuestType);
        switch(currentQuestType){
            case QuestType.Main:
                if(mainQuestList.Count > 0){
                    pinButton.interactable = true;
                    displayedQuest = mainQuestList[0];  // Set the first main quest as the displayed quest
                    ShowQuestDetails(displayedQuest);  // Display the details of the selected quest
                    foreach (Transform child in questListPanel)
                    {
                        if(child.GetComponent<JournalListTitle>().questData.questID == displayedQuest.questID){
                            child.GetComponent<JournalListTitle>().isSelected = true;
                        }else{
                            return;
                        }
                    }
                }else{
                    pinButton.interactable = false;
                }
            break;
            case QuestType.Sub:
                if(subQuestList.Count > 0){
                    pinButton.interactable = true;
                    ShowQuestDetails(subQuestList[0]);  // Display the details of the selected quest
                    foreach (Transform child in questListPanel)
                    {
                        if(child.GetComponent<JournalListTitle>().questData.questID == displayedQuest.questID){
                            child.GetComponent<JournalListTitle>().isSelected = true;
                        }else{
                            return;
                        }
                    }
                }else{
                    pinButton.interactable = false;
                }
            break;
            case QuestType.Completed:
                if(subQuestList.Count > 0){
                    pinButton.interactable = false;
                    ShowQuestDetails(completedQuestList[0]);  // Display the details of the selected quest
                    foreach (Transform child in questListPanel)
                    {
                        if(child.GetComponent<JournalListTitle>().questData.questID == displayedQuest.questID){
                            child.GetComponent<JournalListTitle>().isSelected = true;
                        }else{
                            return;
                        }
                    }
                }else{
                    pinButton.interactable = false;
                }
            break;
        }
    }
    private void UpdateQuestLists()
    {
        var activeQuests = PlayerStats.GetInstance().activeQuests;
        var completedQuests = PlayerStats.GetInstance().completedQuests;
        mainQuestList = activeQuests.Where(q => q.isMain && q.isActive && !q.isCompleted).ToList();
        subQuestList = activeQuests.Where(q => !q.isMain && q.isActive && !q.isCompleted).ToList();
        completedQuestList = completedQuests.ToList();
    }

    private void UpdateQuestBoard()
    {
        List<QuestSO> questsToDisplay = currentQuestType switch
        {
            QuestType.Main => mainQuestList,
            QuestType.Sub => subQuestList,
            QuestType.Completed => completedQuestList,
            _ => new List<QuestSO>()
        };

        foreach (var quest in questsToDisplay)
        {
            AddQuestToBoard(quest);
        }
        // Optionally show the details of the first quest
        if (questsToDisplay.Count > 0)
        {
            ShowQuestDetails(questsToDisplay[0]);
            foreach (Transform child in questListPanel)
            {
                if(child.GetComponent<JournalListTitle>().questData.questID == questsToDisplay[0].questID){
                    child.GetComponent<JournalListTitle>().isSelected = true;
                    break;
                }else{
                    child.GetComponent<JournalListTitle>().isSelected = false;
                }
            }
            if(currentQuestType != QuestType.Completed){
                pinButton.interactable = true;
            }
            
        }else{
            displayedQuest = null;
            pinButton.interactable = false;
        }
    }

    private void AddQuestToBoard(QuestSO quest)
    {
        JournalListTitle titlePrefab = Instantiate(journalListTitle, Vector3.zero, Quaternion.identity);
        titlePrefab.transform.SetParent(questListPanel);
        titlePrefab.transform.localScale = Vector3.one;
        titlePrefab.SetData(quest);
    }

    public void ChangeType(string val)
    {
        currentQuestType = val switch
        {
            "main" => QuestType.Main,
            "sub" => QuestType.Sub,
            "completed" => QuestType.Completed,
            _ => QuestType.Main
        };
        DisableButton(currentQuestType);
        ClearQuestDetails();
        ClearContent(questListPanel);
        pinButton.interactable = false;
        UpdateQuestLists(); // Update the lists based on current state
        UpdateQuestBoard(); // Update the quest board with the new list
    }
    public void PinUnpin()
    {
        displayedQuest.isPinned = !displayedQuest.isPinned;
        PlayerStats.GetInstance().SaveQuestToServer();
        pinTypeText.SetText(displayedQuest.isPinned ? "Unpin Quest" : "Pin Quest");
        // Update the quest board with the new list
    }
    private void ClearQuestDetails()
    {
        displayedQuest = null;
        questTitleText.SetText("");
        descriptionText.SetText("");
        ClearContent(objectiveListPanel);
        // Clear the objectives list
    }
    public void DeselectAllQuests()
    {
        foreach (Transform child in questListPanel)
        {
            JournalListTitle questTitle = child.GetComponent<JournalListTitle>();
            if (questTitle != null)
            {
                questTitle.Deselect();  
            }
        }
    }
    public void DisableButton(QuestType type){
        switch(type){
            case QuestType.Main:
                mainButton.interactable = false;
                subButton.interactable = true;
                completeButton.interactable = true;
            break;
            case QuestType.Sub:
                mainButton.interactable = true;
                subButton.interactable = false;
                completeButton.interactable = true;
            break;
            case QuestType.Completed:
                mainButton.interactable = true;
                subButton.interactable = true;
                completeButton.interactable = false;
            break;
        }
    }
    public void ClearContent(RectTransform cPanel)
    {
        foreach (Transform child in cPanel)
        {
            Destroy(child.gameObject);
        }
    }
}
