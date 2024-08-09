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
    public string id {get;set;}
    public string name {get;set;}
    public Sprite portrait {get;set;}
    public string dialogueKnot {get;set;}
    public TextAsset npcDialogue;
    public List<string> giveableQuest;
}
[Serializable]
public class Goal
{
    public int goalID;
    public string goalDescription;
    public GoalTypeEnum goalType;
    public int requiredAmount;
    public int currentAmount;
    public string inkyRedirect;
    public int characterIndex;

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
    public string value;
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
public enum MouseEnum {
    Default,
    Warning,
    Link,
    Accept,
    SlideHorizontal,
    SlideVertical,
    Info,
    Help,
    TextBox
}

[Serializable]
public class QuestList
{
    public List<Quest> quests;
}