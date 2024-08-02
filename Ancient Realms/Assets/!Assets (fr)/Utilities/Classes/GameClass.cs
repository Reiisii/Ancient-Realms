using System;
using System.Collections.Generic;
using ESDatabase.Classes;
using UnityEngine;
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
public class NPCData{
    public string name {get;set;}
    public Sprite portrait {get;set;}
    public string dialogueKnot {get;set;}
    public TextAsset npcDialogue;
    public List<string> giveableQuest;
}
[Serializable]
public class Goal
{
    public string goalID;
    public string goalDescription;
    public GoalTypeEnum goalType;
    public int requiredAmount;
    public int currentAmount;
    public string inkyRedirect;

    public bool isReached(){
        return (currentAmount >= requiredAmount);
    }
    public void IncrementProgress(int amount)
    {
        currentAmount += amount;
        if (currentAmount > requiredAmount)
        {
            currentAmount = requiredAmount;
        }
    }

}

[Serializable]
public class Reward
{
    public RewardsEnum rewardType;
    public int value;
}
public enum CultureEnum {
    Roman,
    Gallic,
    Egyptian,
    Greek,
    Germanic,
    HellenisticEgyptian
}
public enum ChapterEnum {
    One,
    Two,
    Three,
    Four,
    Five,
    Six
}
public enum RarityEnum {
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}
public enum EquipmentEnum {
    Helmet,
    Chest,
    Shield,
    Weapon,
    Foot,
    Others
}

public enum WeaponType{
    NotAWeapon,
    Sword,
    Spear,
    Javelin,
    SpearJavelin,
    Dagger
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
        if(goalType == GoalTypeEnum.WalkRight){
            currentAmount++;
        };
    }
}