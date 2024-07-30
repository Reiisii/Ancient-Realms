using System;
using System.Collections.Generic;
using ESDatabase.Classes;
[Serializable]
public class Quest
{
    public string questID;
    public string questTitle;
    public string questDescription;
    public int chapter;
    public bool isMain;
    public bool isActive;
    public bool completed;
    public List<Goal> goals;
    public List<Reward> rewards;
}

[Serializable]
public class Goal
{
    public string goalID;
    public string goalDescription;
    public string goalType;
    public int requiredAmount;
    public int currentAmount;
}

[Serializable]
public class Reward
{
    public string rewardType;
    public int value;
}

[Serializable]
public class QuestList
{
    public List<Quest> quests;
}
public class GoalListener{
    public GoalTypeEnum goalType;
    public int currentAmount;
    public int requiredAmount;

    public GoalListener(GoalData goal, GoalTypeEnum type){
        this.goalType = type;
        this.currentAmount = goal.currentAmount;
        this.requiredAmount = goal.requiredAmount;
    }
    public bool isReached(){
            return (currentAmount >= requiredAmount);
    }

    public void Listen(){
        if(goalType == GoalTypeEnum.Kill) currentAmount++;
        if(goalType == GoalTypeEnum.Gather) currentAmount++;
        if(goalType == GoalTypeEnum.Talk) currentAmount++;
        if(goalType == GoalTypeEnum.Move) currentAmount++;
    }
}