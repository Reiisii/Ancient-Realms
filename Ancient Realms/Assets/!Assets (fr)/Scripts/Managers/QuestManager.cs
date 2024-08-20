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
    public List<QuestSO> quests;
    private static QuestManager Instance;
    private PlayerStats playerStats;
    private Dictionary<string, QuestPrefab> activeQuestPrefabs = new Dictionary<string, QuestPrefab>();
    public InputActionAsset inputActions;
    private InputActionMap playerActionMap;
    private InputActionMap uiActionMap;

    private void Awake(){
        if(Instance != null){
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        Instance = this;
        playerActionMap = inputActions.FindActionMap("Player");
        uiActionMap = inputActions.FindActionMap("UI");
    }
    void Start(){
        playerStats = PlayerStats.GetInstance();
        quests = Resources.LoadAll<QuestSO>("QuestSO").ToList();
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
                QuestData questData = new QuestData(){
                    questID = questID,
                    isActive = true,
                    completed = false,
                    currentGoal = 0,
                    goals = new List<GoalData>()
                };
                if(quest.isChained) {
                    quest.currentKnot = "start";
                    questData.currentKnot = "start"; 
                }else{
                    quest.currentKnot = "exhaust";
                    questData.currentKnot = "exhaust";
                }
                foreach (Goal goal in quest.goals)
                {
                    GoalData goalData = new GoalData(){
                        goalID = goal.goalID,
                        currentAmount = 0,
                        requiredAmount = goal.requiredAmount
                    };
                    questData.goals.Add(goalData);
                }

                playerStats.AddQuest(questData, quest);
                Debug.Log("Started Quest:" + questID);

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
            if (!activeQuestPrefabs.ContainsKey(quest.questID))
            {
                AddQuestToBoard(quest);
            }
            else
            {
                activeQuestPrefabs[quest.questID].UpdateQuestDisplay();
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
    }
    public void OpenJournal(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if(journalPanel.activeSelf == true){
                Time.timeScale = 1f;
                playerActionMap.Enable();
                uiActionMap.Disable();
                journalPanel.SetActive(false);
            }else{  
                Time.timeScale = 0f;
                playerActionMap.Disable();
                uiActionMap.Enable();
                journalPanel.SetActive(true);
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
            if (quest.currentGoal < quest.goals.Count)
            {
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
        if(!quest.isActive) return;
        Goal goal = quest.goals.Find(g => g.goalID == goalID);
        if (goal != null)
        {
            goal.currentAmount++;
            playerStats.SaveQuestToServer();
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
            playerStats.SaveQuestToServer();
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
                    if(!quest.isRewarded) playerStats.AddArtifact(reward.value);
                    break;
                case RewardsEnum.Quest:
                    Instance.StartQuest(reward.value);
                    break;
            }
            quest.isRewarded = true;
            playerStats.SaveQuestToServer();
        }
    }
}
