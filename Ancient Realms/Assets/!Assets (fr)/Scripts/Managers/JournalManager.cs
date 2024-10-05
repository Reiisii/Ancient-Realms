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
    [SerializeField] List<QuestSO> mainQuestList = new List<QuestSO>();
    [SerializeField] List<QuestSO> subQuestList = new List<QuestSO>();
    [SerializeField] List<QuestSO> completedQuestList = new List<QuestSO>();
    [SerializeField] Button mainButton;
    [SerializeField] Button subButton;
    [SerializeField] Button completeButton;
    private static JournalManager Instance;
    public QuestType currentQuestType = QuestType.Main;
    private int questTypeCycle = 0;
    private int questListCycle = 0;
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
    private void OnEnable()
    {
        InitializeJournal();
    }
    private void OnDisable()
    {
        ClearQuestDetails();
        ClearContent(questListPanel);
    }
    private void InitializeJournal()
    {
        ChangeType(currentQuestType);
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
            if(displayedQuest != null){
                ShowQuestDetails(displayedQuest);
                int i = 0;
                foreach (Transform child in questListPanel)
                {
                    if(child.GetComponent<JournalListTitle>().questData.questID == displayedQuest.questID){
                        child.GetComponent<JournalListTitle>().isSelected = true;
                        questListCycle = GetSelectedIndex(displayedQuest.questID);
                        break;
                    }
                    i++;
                }
                
                if(!displayedQuest.isCompleted && displayedQuest.isActive){
                    pinButton.interactable = true;
                }
            }else{
                ShowQuestDetails(questsToDisplay[0]);
                questListCycle = 0;
                foreach (Transform child in questListPanel)
                {
                    if(child.GetComponent<JournalListTitle>().questData.questID == displayedQuest.questID){
                        child.GetComponent<JournalListTitle>().isSelected = true;
                        break;
                    }
                }
                if(!questsToDisplay[0].isCompleted && questsToDisplay[0].isActive){
                    pinButton.interactable = true;
                }
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
    public void CycleQuestRight()
    {
        if(questTypeCycle > 1) return;
        questTypeCycle++;
        switch(questTypeCycle){
            case 0:
                ChangeType("main");
            break;
            case 1:
                ChangeType("sub");
            break;
            case 2:
                ChangeType("completed");
            break;
        }
    }
    public void CycleQuestLeft()
    {
        if(questTypeCycle < 1) return;
        questTypeCycle--;
        switch(questTypeCycle){
            case 0:
                ChangeType("main");
            break;
            case 1:
                ChangeType("sub");
            break;
            case 2:
                ChangeType("completed");
            break;
        }
    }
    public void CycleListUp()
    {
        List<QuestSO> questsToDisplay = currentQuestType switch
        {
            QuestType.Main => mainQuestList,
            QuestType.Sub => subQuestList,
            QuestType.Completed => completedQuestList,
            _ => new List<QuestSO>()
        };
        if(!IsSelected() && questsToDisplay.Count > 0){
            if(questListCycle < questsToDisplay.Count){
                ShowQuestDetails(questsToDisplay[0]);
                foreach (Transform child in questListPanel)
                {
                    if(child.GetComponent<JournalListTitle>().questData.questID == displayedQuest.questID){
                        child.GetComponent<JournalListTitle>().isSelected = true;
                        break;
                    }
                }
            }
        }else if(questsToDisplay.Count > 0 && IsSelected()){
            if(questListCycle >= 1){
                questListCycle--;
                DeselectAllQuests();
                ShowQuestDetails(questsToDisplay[questListCycle]);
                foreach (Transform child in questListPanel)
                {
                    if(child.GetComponent<JournalListTitle>().questData.questID == displayedQuest.questID){
                        child.GetComponent<JournalListTitle>().isSelected = true;
                        break;
                    }
                }
            }
        }
    }
    public void CycleListDown()
    {
        List<QuestSO> questsToDisplay = currentQuestType switch
        {
            QuestType.Main => mainQuestList,
            QuestType.Sub => subQuestList,
            QuestType.Completed => completedQuestList,
            _ => new List<QuestSO>()
        };
        if(!IsSelected() && questsToDisplay.Count > 0){
            if(questListCycle < questsToDisplay.Count){
                ShowQuestDetails(questsToDisplay[0]);
                foreach (Transform child in questListPanel)
                {

                    if(child.GetComponent<JournalListTitle>().questData.questID == displayedQuest.questID){
                        child.GetComponent<JournalListTitle>().isSelected = true;
                        break;
                    }
                }
            }
        }else if(questsToDisplay.Count > 0 && IsSelected()){
            if(questListCycle < (questsToDisplay.Count - 1)){
                questListCycle++;
                DeselectAllQuests();
                ShowQuestDetails(questsToDisplay[questListCycle]);
                foreach (Transform child in questListPanel)
                {

                    if(child.GetComponent<JournalListTitle>().questData.questID == displayedQuest.questID){
                        child.GetComponent<JournalListTitle>().isSelected = true;
                        break;
                    }
                }
            }
        }
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
        switch(val){
            case "main":
                questTypeCycle = 0;
            break;
            case "sub":
                questTypeCycle = 1;
            break;
            case "completed":
                questTypeCycle = 2;
            break;
        };
        DisableButton(currentQuestType);
        ClearQuestDetails();
        ClearContent(questListPanel);
        pinButton.interactable = false;
        UpdateQuestLists(); // Update the lists based on current state
        UpdateQuestBoard(); // Update the quest board with the new list
    }
    public void ChangeType(QuestType questType)
    {
        currentQuestType = questType switch
        {
            QuestType.Main => QuestType.Main,
            QuestType.Sub => QuestType.Sub,
            QuestType.Completed => QuestType.Completed,
            _ => QuestType.Main
        };
        switch(questType){
            case QuestType.Main:
                questTypeCycle = 0;
            break;
            case QuestType.Sub:
                questTypeCycle = 1;
            break;
            case QuestType.Completed:
                questTypeCycle = 2;
            break;
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
    public int GetSelectedIndex(string questID)
    {
        int i = 0;
        List<QuestSO> questsToDisplay = currentQuestType switch
        {
            QuestType.Main => mainQuestList,
            QuestType.Sub => subQuestList,
            QuestType.Completed => completedQuestList,
            _ => new List<QuestSO>()
        };
        foreach (QuestSO child in questsToDisplay)
        {
            if (child.questID.Equals(questID))
            {
                return i;  
            }
            i++;
        }
        return 0;
    }
    public bool IsSelected()
    {
        foreach (Transform child in questListPanel)
        {
            if (child.GetComponent<JournalListTitle>().isSelected)
            {
                return true;  
            }
        }
        return false;
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
