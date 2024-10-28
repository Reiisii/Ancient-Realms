using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    private static MissionManager Instance;
    public MissionSO mission;
    public bool inMission = false;
    private void Awake(){
        if(Instance != null){
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        Instance = this;
    }
    public static MissionManager GetInstance(){
        return Instance;
    }
    public void StartMission(string missionID){
        MissionSO missionSO = AccountManager.Instance.missions.FirstOrDefault(m => m.missionID.Equals(missionID));

        if(missionSO != null){
            mission = missionSO.CreateCopy();
            inMission = true;
        }else{
            inMission = false;
        }
    }
    void Update(){
        if(mission != null){
            Debug.Log(mission.goals[mission.currentGoal].currentAmount + "/" + mission.goals[mission.currentGoal].requiredAmount);
        }
    }

    public void AcceptAndContinue(){
        if(mission != null){
            mission = null;
            inMission = false;
        }
    }
    public void EndMission(){
        if(mission != null){
            inMission = false;
            mission = null;
        }
    }
    public void UpdateGoal(MissionGoalType missionGoalType)
    {
        MissionGoal missionGoal = mission.goals[mission.currentGoal];
        if (missionGoal.missionType == missionGoalType)
        {
            missionGoal.IncrementProgress(1);
            if (missionGoal.currentAmount >= missionGoal.requiredAmount)
            {  
                CompleteGoal(missionGoal.taskID); // Complete the goal if required amount is reached
            }
        }
    }

    public void CompleteGoal(int goalID)
    {
        MissionGoal goal = mission.goals.Find(g => g.taskID == goalID);
        if (goal != null)
        {
            goal.currentAmount++;
            if (goal.currentAmount >= goal.requiredAmount)
            {
                mission.currentGoal++;
                AudioManager.GetInstance().PlayAudio(SoundType.GOAL, 1f);
                CheckQuestCompletion(mission);
            }
        }
    }
    private void CheckQuestCompletion(MissionSO mission)
    {
        bool allGoalsCompleted = true;

        foreach (var goal in mission.goals)
        {
            if (goal.currentAmount < goal.requiredAmount)
            {
                allGoalsCompleted = false;
                break;
            }
        }
        if (allGoalsCompleted)
        {
            mission.isCompleted = true;
            PlayerUIManager.GetInstance().OpenMissionPanel();
            PlayerController.GetInstance().playerActionMap.Disable();
        }
    }
    public void RewardPlayer(MissionSO mission)
    {
        PlayerStats playerStats = PlayerStats.GetInstance();

        foreach (Reward reward in mission.rewards)
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
                    playerStats.AddItem(Convert.ToInt32(reward.value), reward.level, reward.tier, reward.amount);
                    break;
                case RewardsEnum.Artifact:
                    playerStats.AddArtifact(reward.value);
                    ArtifactsSO achievement = AccountManager.Instance.achievements.Where(a => a.id == Convert.ToInt32(reward.value)).FirstOrDefault();
                break;
                case RewardsEnum.Quest:
                    QuestManager.GetInstance().StartQuest(reward.value);
                break;
                case RewardsEnum.Event:
                    playerStats.AddEncyc(EncycType.Event, Convert.ToInt32(reward.value));
                break;
            }
        }
        QuestManager.GetInstance().UpdateMissionGoal(mission.missionID);
        EndMission();
    }
}
