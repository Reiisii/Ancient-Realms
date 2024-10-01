using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ESDatabase.Classes;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuestManager : MonoBehaviour
{
    [SerializeField] QuestPrefab qPrefab;
    [SerializeField] public RectTransform questPanel;
    [SerializeField] public GameObject journalPanel;
    [SerializeField] public QuestPointer questPointer;
    public List<QuestSO> quests;
    public List<ArtifactsSO> achievements;
    private static QuestManager Instance;
    private PlayerStats playerStats;
    private Dictionary<string, QuestPrefab> activeQuestPrefabs = new Dictionary<string, QuestPrefab>();

    private void Awake(){
        if(Instance != null){
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        Instance = this;
        quests = Resources.LoadAll<QuestSO>("QuestSO").ToList();
        achievements = Resources.LoadAll<ArtifactsSO>("ArtifactSO").ToList();
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
                QuestData questData = new QuestData(){
                    questID = questID,
                    isActive = true,
                    isPinned = true,
                    completed = false,
                    currentGoal = 0,
                    goals = new List<GoalData>()
                };

                foreach (Goal goal in quest.goals)
                {
                    GoalData goalData = new GoalData(){
                        goalID = goal.goalID,
                        currentAmount = 0,
                        requiredAmount = goal.requiredAmount
                    };
                    questData.goals.Add(goalData);
                }
                QuestSO copiedQuest = quest.CreateCopy();
                copiedQuest.isActive = true;
                copiedQuest.isPinned = true;
                if(quest.isChained) {
                    questData.currentKnot = "start"; 
                    copiedQuest.currentKnot = "start";
                }else{
                    questData.currentKnot = "exhaust";
                    copiedQuest.currentKnot = "exhaust";
                }
                Notification notification = new Notification(){
                            title = quest.questTitle,
                            notifType = NotifType.QuestStart
                        };
                PlayerUIManager.GetInstance().notification.AddQueue(notification);
                playerStats.AddQuest(questData, copiedQuest);

            }
        }
    }
    public void Update(){
        UpdateQuestBoard();
    }
    private void UpdateQuestBoard()
    {
        // Remove completed quests from the board
        var completedQuests = activeQuestPrefabs.Keys.Except(playerStats.activeQuests.Select(q => q.questID)).ToList();
        foreach (var questID in completedQuests)
        {
            if (activeQuestPrefabs.TryGetValue(questID, out QuestPrefab questPrefab))
            {
                Destroy(questPrefab.gameObject);
                activeQuestPrefabs.Remove(questID);
            }
        }

        // Update or add active quests
        foreach (var quest in playerStats.activeQuests)
        {
            if (quest.isPinned)
            {
                if (!activeQuestPrefabs.ContainsKey(quest.questID))
                {
                    AddQuestToBoard(quest);
                }
                else
                {
                    activeQuestPrefabs[quest.questID].UpdateQuestDisplay();
                }
            }
            else
            {
                // If the quest is not pinned but is in the board, remove it
                if (activeQuestPrefabs.ContainsKey(quest.questID))
                {
                    Destroy(activeQuestPrefabs[quest.questID].gameObject);
                    activeQuestPrefabs.Remove(quest.questID);
                }
            }
        }

    }
    private void AddQuestToBoard(QuestSO quest)
    {
        QuestPrefab questPrefab = Instantiate(qPrefab, Vector3.zero, Quaternion.identity);
        questPrefab.transform.SetParent(questPanel);
        questPrefab.transform.localScale = Vector3.one;
        questPrefab.setQuestSO(quest);
        activeQuestPrefabs.Add(quest.questID, questPrefab);
        GameObject pointerList = PlayerUIManager.GetInstance().questPointer;
        QuestPointer pointerPrefab = Instantiate(questPointer, Vector3.zero, Quaternion.identity);
        pointerPrefab.transform.SetParent(pointerList.transform);
        pointerPrefab.transform.localScale = Vector3.one;
        pointerPrefab.SetData(quest);
    }
    public void OpenJournal()
    {
        if(journalPanel.activeSelf == true){
            Time.timeScale = 1f;
            PlayerController.GetInstance().playerActionMap.Enable();
            PlayerController.GetInstance().questActionMap.Disable();
            journalPanel.SetActive(false);
        }else{  
            Time.timeScale = 0f;
            PlayerController.GetInstance().playerActionMap.Disable();
            PlayerController.GetInstance().questActionMap.Enable();
            journalPanel.SetActive(true);
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
                    playerStats.SaveQuestToServer();
                    if (goal.currentAmount >= goal.requiredAmount)
                    {
                        // COMPLETE TRIGGER CODE
                        CompleteGoal(quest, goal.goalID); // Complete the goal if required amount is reached
                    }
                }
                else if (goal.goalType == GoalTypeEnum.WalkLeft && deltaX < 0)
                {
                    goal.IncrementProgress(1); 
                    playerStats.SaveQuestToServer();
                    if (goal.currentAmount >= goal.requiredAmount)
                    {
                        // COMPLETE TRIGGER CODE
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
                    playerStats.SaveQuestToServer();
                    if (goal.currentAmount >= goal.requiredAmount)
                    {
                        // COMPLETE TRIGGER CODE
                        CompleteGoal(quest, goal.goalID); // Complete the goal if required amount is reached
                    }
                }
                else if (goal.goalType == GoalTypeEnum.RunLeft && deltaX < 0)
                {
                    goal.IncrementProgress(1); 
                    playerStats.SaveQuestToServer();
                    if (goal.currentAmount >= goal.requiredAmount)
                    {
                        // COMPLETE TRIGGER CODE 
                        CompleteGoal(quest, goal.goalID); // Complete the goal if required amount is reached
                    }
                }
            }
        }
    }
    public void UpdateHitMeleeGoal()
    {
        foreach (var quest in playerStats.activeQuests)
        {
            if(quest.currentGoal < quest.goals.Capacity){
                Goal goal = quest.goals[quest.currentGoal];
                if (goal.goalType == GoalTypeEnum.HitMelee)
                {
                    goal.IncrementProgress(1);
                    playerStats.SaveQuestToServer();
                    if (goal.currentAmount >= goal.requiredAmount)
                    {
                        // COMPLETE TRIGGER CODE
                        CompleteGoal(quest, goal.goalID); // Complete the goal if required amount is reached
                    }
                }
            }
        }
    }
    public void UpdatePromptGoals(QuestSO quest)
    {
        if(quest.currentGoal < quest.goals.Capacity){
            Goal goal = quest.goals[quest.currentGoal];
            if (goal.goalType == GoalTypeEnum.Prompt)
            {
                goal.IncrementProgress(1);
                playerStats.SaveQuestToServer();
                if (goal.currentAmount >= goal.requiredAmount)
                {
                        // COMPLETE TRIGGER CODE
                    CompleteGoal(quest, goal.goalID); // Complete the goal if required amount is reached
                }
            }
        }
    }
    public void UpdateHitJavelinGoal()
    {
        foreach (var quest in playerStats.activeQuests)
        {
            if(quest.currentGoal < quest.goals.Capacity){
                Goal goal = quest.goals[quest.currentGoal];
                if (goal.goalType == GoalTypeEnum.HitJavelin)
                {
                    goal.IncrementProgress(1);
                    playerStats.SaveQuestToServer();
                    if (goal.currentAmount >= goal.requiredAmount)
                    {
                        // COMPLETE TRIGGER CODE
                        CompleteGoal(quest, goal.goalID); // Complete the goal if required amount is reached
                    }
                }
            }
        }
    }
    public void UpdateHitAnyGoal()
    {
        foreach (var quest in playerStats.activeQuests)
        {
            if(quest.currentGoal < quest.goals.Capacity){
                Goal goal = quest.goals[quest.currentGoal];
                if (goal.goalType == GoalTypeEnum.HitAny)
                {
                    goal.IncrementProgress(1);
                    playerStats.SaveQuestToServer();
                    if (goal.currentAmount >= goal.requiredAmount)
                    {
                        // COMPLETE TRIGGER CODE
                        CompleteGoal(quest, goal.goalID); // Complete the goal if required amount is reached
                    }
                }
            }
        }
    }
    public void UpdateKillGoal()
    {
        foreach (var quest in playerStats.activeQuests)
        {
            if(quest.currentGoal < quest.goals.Capacity){
                Goal goal = quest.goals[quest.currentGoal];
                if (goal.goalType == GoalTypeEnum.Kill)
                {
                    goal.IncrementProgress(1);
                    playerStats.SaveQuestToServer();
                    if (goal.currentAmount >= goal.requiredAmount)
                    {
                        // COMPLETE TRIGGER CODE
                        CompleteGoal(quest, goal.goalID); // Complete the goal if required amount is reached
                    }
                }
            }
        }
    }
    public void UpdateDamageGoal(float damage)
    {
        foreach (var quest in playerStats.activeQuests)
        {
            if(quest.currentGoal < quest.goals.Capacity){
                Goal goal = quest.goals[quest.currentGoal];
                if (goal.goalType == GoalTypeEnum.Damage)
                {
                    goal.IncrementProgress(Convert.ToInt32(damage));
                    playerStats.SaveQuestToServer();
                    if (goal.currentAmount >= goal.requiredAmount)
                    {
                        // COMPLETE TRIGGER CODE
                        CompleteGoal(quest, goal.goalID); // Complete the goal if required amount is reached
                    }
                }
            }
        }
    }
    public void UpdateTalkGoal(QuestSO quest)
    {
        List<QuestSO> questsToRemove = new List<QuestSO>();
        if (quest.currentGoal < quest.goals.Count){
                if (quest.isActive)
                {
                    Goal goal = quest.goals[quest.currentGoal];
                    if (goal.goalType == GoalTypeEnum.Talk)
                    {
                        goal.IncrementProgress(1);
                        playerStats.SaveQuestToServer();
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

        foreach (QuestSO completedQuest in questsToRemove)
        {
            playerStats.activeQuests.Remove(completedQuest);
            playerStats.completedQuests.Add(completedQuest); // Optionally add to completed quests here
            playerStats.isDataDirty = true;
        }
    }
    public void CompleteGoal(QuestSO quest, int goalID)
    {
        Goal goal = quest.goals.Find(g => g.goalID == goalID);
        if (goal != null)
        {
            goal.currentAmount++;
            if (goal.currentAmount >= goal.requiredAmount)
            {
                quest.currentGoal++;
                playerStats.SaveQuestToServer();
                if(goal.inkyRedirect.Equals("")) return;
                else quest.currentKnot = goal.inkyRedirect;
                foreach (Transform child in questPanel)
                {
                    QuestPrefab questPrefab = child.GetComponent<QuestPrefab>();
                    if (questPrefab != null && questPrefab.questSO.questID == quest.questID)
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
            quest.isCompleted = true;
            quest.isPinned = false;
            quest.isActive = false;
            Notification notification = new Notification(){
                            title = quest.questTitle,
                            notifType = NotifType.QuestComplete
                        };
            PlayerUIManager.GetInstance().notification.AddQueue(notification);
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
                        if(!quest.isRewarded) playerStats.AddGold(goldAmount);
                    }
                    break;
                case RewardsEnum.Xp:
                    int xpAmount;
                    if (int.TryParse(reward.value, out xpAmount))
                    {
                        if(!quest.isRewarded) playerStats.AddXp(xpAmount);
                    }
                    break;
                case RewardsEnum.Item:
                    // playerStats.AddItem(reward.value);
                    break;
                case RewardsEnum.Artifact:
                    if(!quest.isRewarded){
                        playerStats.AddArtifact(reward.value);
                        ArtifactsSO achievement = achievements.Where(a => a.id == Convert.ToInt32(reward.value)).FirstOrDefault();
                        Notification notification = new Notification(){
                            title = achievement.artifactName,
                            description = achievement.description,
                            notifType = NotifType.Achievement,
                            image = achievement.image
                        };
                        PlayerUIManager.GetInstance().notification.AddQueue(notification);
                        
                    }
                    break;
                case RewardsEnum.Quest:
                    Instance.StartQuest(reward.value);
                    break;
            }
        }
        quest.isRewarded = true;
        playerStats.SaveQuestToServer();
    }
}
