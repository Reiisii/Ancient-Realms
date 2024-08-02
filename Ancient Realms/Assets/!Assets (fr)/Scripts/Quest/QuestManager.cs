using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public TextAsset jsonFile;
    public List<QuestSO> quests;
    private Dictionary<string, QuestSO> activeQuests = new Dictionary<string, QuestSO>();
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
                // Goal completed logic
                CheckQuestCompletion(quest); // Check if quest is complete after completing a goal
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
            quest.isCompleted = true;
            quest.isActive = false;
            Debug.Log("Quest Completed: " + quest.questID);
            // Add any additional logic for when a quest is completed, e.g., rewards, notifications
        }
    }
    private void DisplayDialogue(TextAsset dialogueJSON)
    {
        // Implement dialogue display logic
    }
    public void UpdateWalkGoals(float deltaX)
    {
        foreach (var quest in activeQuests.Values)
        {
            foreach (var goal in quest.goals)
            {
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
            foreach (var goal in quest.goals)
            {
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


}
