using System.Collections.Generic;
using ESDatabase.Classes;
public class Rewards{
    public RewardsEnum rewardType;
    public int amount;
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