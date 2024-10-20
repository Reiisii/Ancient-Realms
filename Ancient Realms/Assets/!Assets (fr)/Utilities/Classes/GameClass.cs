using System;
using System.Collections.Generic;
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
    public List<QuestSO> activePlayerQuest;
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
    public string[] targetCharacters;
    public string missionID;
    public string questID;
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
[Serializable]
public class LocationData {
    public string locationName;
    public Vector3 location;
}

[Serializable]
public class Notification
{
    public string title;
    public string description;
    public Sprite image;
    public NotifType notifType;
}

public enum CultureEnum {
    Roman,
    Gallic,
    Egyptian,
    Greek,
    Germanic,
    HellenisticEgyptian
}
public enum NotifType {
    QuestStart,
    QuestComplete,
    Achievement
}
public enum ChapterEnum {
    Prologue,
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    SubQuest,
    Achievement
}
public enum RarityEnum {
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}
public enum EquipmentEnum {
    Armor,
    Shield,
    Weapon,
    Consumable,
    Item,
    QuestItem
}
public enum ArmorType{
    NotArmor,
    Helmet,
    Chest,
    Waist,
    Foot
}
public enum WeaponType{
    NotWeapon,
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
public enum MType {
    Error,
    Warning,
    Success,
    Info
}
public enum WorkStation {
    Furnace,
    Hammering,
    Grindstone,
    Assembly,
    Metal,
    Submit
}
public enum InventoryTab {
    Equipments,
    Weapons,
    QuestItem,
    NFTs,
    Items
}
public enum BuffType {
    Health,
    Armor,
    Stamina,
    Speed
}
public enum SoundType{
    SLASH,
    PILUM,
    BASH,
    WALK,
    RUN,
    ENTER,
    STAIRS,
    HELMET,
    CHEST,
    WAIST,
    FOOT,
    FIELDS,
    FOREST,
    CAVE,
    TOWN,
    TRAINING,
    WATER,
    DAY,
    NIGHT,
    CLOSE,
    SHEATHE,
    UNSHEATHE
}
public enum MusicType{
    Combat,
    MainMenu,
    Town,
    Loading
}
[Serializable]
public struct SoundList
{
    public AudioClip[] Sounds {get => sounds;}
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] sounds;
}
[Serializable]
public class StatBuff{
    public BuffType buffType;
    public float value;
}
[Serializable]
public class QuestList
{
    public List<Quest> quests;
}