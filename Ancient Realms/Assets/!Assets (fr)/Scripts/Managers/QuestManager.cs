using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] QuestPrefab qPrefab;
    [SerializeField] RectTransform questPanel;
    public List<QuestSO> quests;
    private static QuestManager Instance;
    private PlayerStats playerStats;
    private void Awake(){
        if(Instance != null){
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        Instance = this;
    }
    void Start(){
        playerStats = PlayerStats.GetInstance();
    }
    public static QuestManager GetInstance(){
        return Instance;
    }
    public void StartQuest(string questID)
    {
        QuestSO questSO = playerStats.activeQuests.Where(q => q.questID == questID).FirstOrDefault();
        if (questSO == null)
        {
            QuestSO quest = quests.Find(q => q.questID == questID);
            if (quest != null)
            {
                quest.isActive = true;
                if(!quest.isChained)quest.currentKnot = "exhaust";
                playerStats.activeQuests.Add(quest);
                Debug.Log("Started Quest:" + questID);
                QuestPrefab questPrefab = Instantiate(qPrefab, Vector3.zero, Quaternion.identity);
                questPrefab.transform.SetParent(questPanel);
                questPrefab.transform.localScale = Vector3.one;
                questPrefab.setQuestSO(quest);
            }
        }
    }
    public void UpdateWalkGoals(float deltaX)
    {
        
        foreach (var quest in playerStats.activeQuests)
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
        foreach (var quest in playerStats.activeQuests)
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
        List<QuestSO> questsToRemove = new List<QuestSO>();

        foreach (var quest in playerStats.activeQuests.ToList()) // Create a copy for safe iteration
        {
            if (quest.currentGoal < quest.goals.Count)
            {
                if (quest.isActive)
                {
                    Goal goal = quest.goals[quest.currentGoal];
                    if (goal.goalType == GoalTypeEnum.Talk)
                    {
                        goal.IncrementProgress(1);
                        if (goal.currentAmount >= goal.requiredAmount)
                        {
                            CompleteGoal(quest, goal.goalID);
                            if (quest.isCompleted)
                            {
                                questsToRemove.Add(quest);
                            }
                        }
                    }
                }
            }
        }

        foreach (QuestSO completedQuest in questsToRemove)
        {
            playerStats.activeQuests.Remove(completedQuest);
            playerStats.completedQuests.Add(completedQuest); // Optionally add to completed quests here
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
            RewardPlayer(quest);
            Debug.Log("Quest Completed: " + quest.questID);
            
            // Add any additional logic for when a quest is completed, e.g., rewards, notifications
        }
    }
    private void RewardPlayer(QuestSO quest)
    {
        PlayerStats playerStats = PlayerStats.GetInstance();

        foreach (Reward reward in quest.rewards)
        {
            switch (reward.rewardType)
            {
                case RewardsEnum.Gold:
                    int goldAmount;
                    if (int.TryParse(reward.value, out goldAmount))
                    {
                        playerStats.AddGold(goldAmount);
                    }
                    break;
                case RewardsEnum.Xp:
                    int xpAmount;
                    if (int.TryParse(reward.value, out xpAmount))
                    {
                        playerStats.AddXp(xpAmount);
                    }
                    break;
                case RewardsEnum.Item:
                    // playerStats.AddItem(reward.value);
                    break;
                case RewardsEnum.Artifact:
                    // playerStats.AddArtifact(reward.value);
                    break;
                case RewardsEnum.Quest:
                    Instance.StartQuest(reward.value);
                    break;
            }
        }
    }
}
