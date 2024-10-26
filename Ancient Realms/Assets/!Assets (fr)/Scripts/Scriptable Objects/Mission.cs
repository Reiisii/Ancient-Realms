using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Create Mission", menuName = "SO/Mission")]
public class MissionSO : ScriptableObject
{

    public string missionID;
    public string missionTitle;
    public string missionDescription;
    public ChapterEnum chapter;
    [Header("Mission State")]
    public bool isCompleted = false;
    public int currentGoal;
    public List<MissionGoal> goals;
    public List<Reward> rewards;
    
    public MissionSO CreateCopy()
    {
        // Create a new instance of QuestSO
        MissionSO newMission = ScriptableObject.CreateInstance<MissionSO>();

        // Copy all fields from this QuestSO to the new instance
        newMission.missionID = this.missionID;
        newMission.missionTitle = this.missionTitle;
        newMission.missionDescription = this.missionDescription;
        newMission.chapter = this.chapter;
        newMission.currentGoal = this.currentGoal;
        newMission.goals = new List<MissionGoal>();
        foreach (MissionGoal task in this.goals)
        {
            MissionGoal newGoal = new MissionGoal
            {
                taskID = task.taskID,
                taskDescription = task.taskDescription,
                missionType = task.missionType,
                requiredAmount = task.requiredAmount,
                currentAmount = task.currentAmount,
                holdTime = task.holdTime,
                itemID = task.itemID,
                targetIDs = (string[])task.targetIDs.Clone(),
            };
            newMission.goals.Add(newGoal);
        }
        newMission.rewards = new List<Reward>(this.rewards);

        return newMission;
    }
}
