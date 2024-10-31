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
    public GameObject gameObject;
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
    public List<int> questItem;
    public List<int> requiredItems;
    public string inkyNoRequirement;
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
    public int tier;
    public int level;
    public int amount;
}
[Serializable]
public class NFTResponse
{
    public string url;
    public bool response;
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
    Achievement,
    Character,
    Equipment,
    Event
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
[Serializable]
public class MissionGoal
{
    public int taskID;
    public string taskDescription;
    public MissionGoalType missionType;
    [Header("Mission Settings")]
    public int requiredAmount;
    public int currentAmount;
    public Time holdTime;
    public string itemID;
    public string[] targetIDs;

    public void IncrementProgress(int amount)
    {
        currentAmount += amount;
        if (currentAmount > requiredAmount)
        {
            currentAmount = requiredAmount;
        }
    }

}
public enum MissionGoalType
{
    Defend,
    Capture,
    Clear,
    Pickup
}
public enum MissionRewardEnum
{
    Gold,
    Item,
    Artifacts,
    Xp,
    Artifact,
    Event,
}
public enum OrderType {
    Gladius,
    Pila,
    Pugio
}
public enum PieceType {
    G_BLADE,
    G_POMMEL,
    G_RAINGUARD,
    G_GRIP,
    P_TIP,
    P_SHAFT,
    P_Pommel,
    P_TIPSHAFT,
    PG_POMMEL,
    PG_RAINGUARD,
    PG_BLADE,
    PG_GRIP
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
    UNSHEATHE,
    GOAL,
    GREEN,
    YELLOW,
    RED,
    DELIVERED,
    Hammering,
    ShieldUnequip,
    SwordEquip,
    PilumEquip,
    NFTEquip,
    NFTUnequip
}
public enum MusicType{
    Combat,
    MainMenu,
    Town,
    Loading,
    Smithing
}
public enum EncycType{
    Character,
    Equipment,
    Event
}
public enum StatisticsType{
    MoveDistance,
    Kill,
    DenariiTotal,
    SmithingTotal,
    MintingTotal,
    DeathTotal
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