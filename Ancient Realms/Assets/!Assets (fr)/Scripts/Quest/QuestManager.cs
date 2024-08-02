using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] QuestPrefab questPrefab;
    [SerializeField] RectTransform questPanel;
    public List<QuestSO> quests;
    public Dictionary<string, QuestSO> activeQuests = new Dictionary<string, QuestSO>();
    private static QuestManager Instance;
    private void Awake(){
        if(Instance != null){
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        Instance = this;
    }

    public static QuestManager GetInstance(){
        return Instance;
    }
    public void StartQuest(string questID)
    {
        if (!activeQuests.ContainsKey(questID))
        {
            QuestSO quest = quests.Find(q => q.questID == questID);
            if (quest != null)
            {
                quest.isActive = true;
                activeQuests.Add(questID, quest);
                Debug.Log("Started Quest:" + questID);
                QuestPrefab characterPrefab = Instantiate(questPrefab, Vector3.zero, Quaternion.identity);
                characterPrefab.transform.SetParent(questPanel);
                characterPrefab.transform.localScale = Vector3.one;
                characterPrefab.setQuestSO(quest);
            }
        }
    }
    public void CompleteGoal(QuestSO quest, string goalID)
    {
        if(!quest.isActive) return;
        Goal goal = quest.goals.Find(g => g.goalID == goalID);
        if (goal != null)
        {
            goal.currentAmount++;
            if (goal.currentAmount >= goal.requiredAmount)
            {
                quest.currentGoal++;
                if(goal.inkyRedirect.Equals("")) return;
                else quest.currentKnot = goal.inkyRedirect;
                foreach (Transform child in questPanel)
                {
                    QuestPrefab questPrefab = child.GetComponent<QuestPrefab>();
                    if (questPrefab != null && questPrefab.questSO == quest)
                    {
                        questPrefab.UpdateQuestDisplay();
                    }
                }
                
            CheckQuestCompletion(quest);
            }
        }
    }
    private void CheckQuestCompletion(QuestSO quest)
    {
        bool allGoalsCompleted = true;

        foreach (var goal in quest.goals)
        {
            if (goal.currentAmount < goal.requiredAmount)
            {
                allGoalsCompleted = false;
                break;
            }
        }

        if (allGoalsCompleted)
        {
            // Remove or Delete Quest


            quest.isCompleted = true;
            quest.isActive = false;
            foreach (Transform child in questPanel)
            {
                QuestPrefab questPrefab = child.GetComponent<QuestPrefab>();
                if (questPrefab != null && questPrefab.questSO == quest)
                {
                    Destroy(child.gameObject);
                    break;
                }
            }
            Debug.Log("Quest Completed: " + quest.questID);
            // Add any additional logic for when a quest is completed, e.g., rewards, notifications
        }
    }
    public void UpdateWalkGoals(float deltaX)
    {
        
        foreach (var quest in activeQuests.Values)
        {
                if(quest.currentGoal < quest.goals.Capacity){
                Goal goal = quest.goals[quest.currentGoal];
                if (goal.goalType == GoalTypeEnum.WalkRight && deltaX > 0)
                {
                    goal.IncrementProgress(1);
                    if (goal.currentAmount >= goal.requiredAmount)
                    {
                        CompleteGoal(quest, goal.goalID); // Complete the goal if required amount is reached
                    }
                }
                else if (goal.goalType == GoalTypeEnum.WalkLeft && deltaX < 0)
                {
                    goal.IncrementProgress(1); 
                    if (goal.currentAmount >= goal.requiredAmount)
                    {
                        CompleteGoal(quest, goal.goalID); // Complete the goal if required amount is reached
                    }
                }
            }
        }
    }
    public void UpdateRunGoals(float deltaX)
    {
        foreach (var quest in activeQuests.Values)
        {
            if(quest.currentGoal < quest.goals.Capacity){
                Goal goal = quest.goals[quest.currentGoal];
                if (goal.goalType == GoalTypeEnum.RunRight && deltaX > 0)
                {
                    goal.IncrementProgress(1);
                    if (goal.currentAmount >= goal.requiredAmount)
                    {
                        CompleteGoal(quest, goal.goalID); // Complete the goal if required amount is reached
                    }
                }
                else if (goal.goalType == GoalTypeEnum.RunLeft && deltaX < 0)
                {
                    goal.IncrementProgress(1); 
                    if (goal.currentAmount >= goal.requiredAmount)
                    {
                        CompleteGoal(quest, goal.goalID); // Complete the goal if required amount is reached
                    }
                }
            }
        }
    }

    public void UpdateTalkGoal()
    {
        foreach (var quest in activeQuests.Values)
        {
            if(quest.currentGoal < quest.goals.Capacity){
                if(quest.isActive){
                    Goal goal = quest.goals[quest.currentGoal];
                    if (goal.goalType == GoalTypeEnum.Talk)
                    {
                        goal.IncrementProgress(1);
                        if (goal.currentAmount >= goal.requiredAmount)
                        {
                            CompleteGoal(quest, goal.goalID);
                        }
                    }
                }
            }
        }
    }
}
