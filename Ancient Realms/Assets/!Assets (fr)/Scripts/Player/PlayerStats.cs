using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using ESDatabase.Classes;
using ESDatabase.Entities;
using Solana.Unity.SDK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Player UI")]
    [SerializeField] public GameObject UI;
    [SerializeField] public GameObject QuestPanel;
    [SerializeField] public Slider hpSlider;
    [SerializeField] public TextMeshProUGUI hpText;
    [SerializeField] public Slider staminaSlider;
    [SerializeField] public TextMeshProUGUI staminaText;
    [SerializeField] public Slider xpSlider;
    [SerializeField] public TextMeshProUGUI levelText;
    [Header("Scripts")]
    public PlayerData localPlayerData;

    [Header("Player Stats")]
    public float currentHP = 100f;
    public float maxHP = 100f;
    public float maxStamina = 70f;
    public float stamina = 70f;
    public float armor = 0f;
    [Header("Movement Stats")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float staminaDepletionRate = 40f;
    public float staminaRegenRate = 10f;
    [Header("Combat Stats")]
    public float attack = 30;
    public bool isCombatMode = false;
    public bool toggleStamina = true;
    [Header("Temp")]
    public bool isDataDirty = false;
    public int level = 0;
    public int denarii = 0;
    public int maxXP = 30;
    public int currentXP = 0;
    public float attackRange = 0f;
    public float maxThrowForce = 20f; // Maximum force applied to the throw
    public float maxHoldTime = 1f;
    public List<EquipmentSO> equippedItems;
    public List<EquipmentSO> inventory;
    public List<QuestSO> activeQuests;
    public List<QuestSO> completedQuests;
    private static PlayerStats Instance;
    private List<EquipmentSO> equipmentLibrary;
    private void Awake()
    {
        equipmentLibrary = AccountManager.Instance.equipments;
        if (Instance != null)
        {
            Debug.LogWarning("Found more than one Player Stats in the scene");
        }
        localPlayerData = AccountManager.Instance.playerData;
        Instance = this;
    }
    public void Start()
    {
        InvokeRepeating("SaveDataToServer", 1f, 1f); // Save data to the server every 10 seconds
    }
    private void OnEnable()
    {
        LoadPlayerData(localPlayerData);
    }

    private void LoadPlayerData(PlayerData data)
    {
        GameData playerGameData = data.gameData;
        foreach(QuestData quest in playerGameData.quests){
            if(quest.isActive && !quest.completed){
                QuestSO qData = AccountManager.Instance.quests.Where(q => q.questID == quest.questID).FirstOrDefault();
                QuestSO copiedQuest = qData.CreateCopy();
                copiedQuest.isPinned = quest.isPinned;
                copiedQuest.isActive = quest.isActive;
                copiedQuest.isCompleted = quest.completed;
                copiedQuest.currentKnot = quest.currentKnot;
                copiedQuest.currentGoal = quest.currentGoal;
                copiedQuest.isRewarded = quest.isRewarded;
                for (int i = 0; i < copiedQuest.goals.Count; i++)
                {
                    copiedQuest.goals[i].currentAmount = quest.goals[i].currentAmount;
                }
                activeQuests.Add(copiedQuest);
            }else if(!quest.isActive && quest.completed){
                QuestSO qData = AccountManager.Instance.quests.Where(q => q.questID == quest.questID).FirstOrDefault();
                QuestSO copiedQuest = qData.CreateCopy();
                copiedQuest.isPinned = quest.isPinned;
                copiedQuest.isActive = quest.isActive;
                copiedQuest.isCompleted = quest.completed;
                copiedQuest.currentKnot = quest.currentKnot;
                copiedQuest.currentGoal = quest.currentGoal;
                copiedQuest.isRewarded = quest.isRewarded;
                for (int i = 0; i < copiedQuest.goals.Count; i++)
                {
                    copiedQuest.goals[i].currentAmount = quest.goals[i].currentAmount;
                }
                completedQuests.Add(copiedQuest);
            }
        }
        if(playerGameData.equippedData.helmSlot != null) {
            ItemData item = playerGameData.equippedData.helmSlot;
            EquipmentSO helmSO = equipmentLibrary.Where(equipment => equipment.equipmentId == item.equipmentId).FirstOrDefault();
            EquipmentSO copiedhelm = helmSO.CreateCopy(item);
            equippedItems.Add(copiedhelm);
        }else equippedItems.Add(null);
        
        if(playerGameData.equippedData.chestSlot != null) {
            ItemData item = playerGameData.equippedData.chestSlot;
            EquipmentSO helmSO = equipmentLibrary.Where(equipment => equipment.equipmentId == item.equipmentId).FirstOrDefault();
            EquipmentSO copiedhelm = helmSO.CreateCopy(item);
            equippedItems.Add(copiedhelm);
        }else equippedItems.Add(null);

        if(playerGameData.equippedData.waistSlot != null) {
            ItemData item = playerGameData.equippedData.waistSlot;
            EquipmentSO helmSO = equipmentLibrary.Where(equipment => equipment.equipmentId == item.equipmentId).FirstOrDefault();
            EquipmentSO copiedhelm = helmSO.CreateCopy(item);
            equippedItems.Add(copiedhelm);
        }else equippedItems.Add(null);

        if(playerGameData.equippedData.footSlot != null) {
            ItemData item = playerGameData.equippedData.footSlot;
            EquipmentSO helmSO = equipmentLibrary.Where(equipment => equipment.equipmentId == item.equipmentId).FirstOrDefault();
            EquipmentSO copiedhelm = helmSO.CreateCopy(item);
            equippedItems.Add(copiedhelm);
        }else equippedItems.Add(null);

        if(playerGameData.equippedData.mainSlot != null) {
            ItemData item = playerGameData.equippedData.mainSlot;
            EquipmentSO helmSO = equipmentLibrary.Where(equipment => equipment.equipmentId == item.equipmentId).FirstOrDefault();
            EquipmentSO copiedhelm = helmSO.CreateCopy(item);
            equippedItems.Add(copiedhelm);
        }else equippedItems.Add(null);

        if(playerGameData.equippedData.shieldSlot != null) {
            ItemData item = playerGameData.equippedData.shieldSlot;
            EquipmentSO helmSO = equipmentLibrary.Where(equipment => equipment.equipmentId == item.equipmentId).FirstOrDefault();
            EquipmentSO copiedhelm = helmSO.CreateCopy(item);
            equippedItems.Add(copiedhelm);
        }else equippedItems.Add(null);

        if(playerGameData.equippedData.javelinSlot != null) {
            ItemData item = playerGameData.equippedData.javelinSlot;
            EquipmentSO helmSO = equipmentLibrary.Where(equipment => equipment.equipmentId == item.equipmentId).FirstOrDefault();
            EquipmentSO copiedhelm = helmSO.CreateCopy(item);
            equippedItems.Add(copiedhelm);
        }else equippedItems.Add(null);

        if(playerGameData.equippedData.bandageSlot != null) {
            ItemData item = playerGameData.equippedData.bandageSlot;
            EquipmentSO helmSO = equipmentLibrary.Where(equipment => equipment.equipmentId == item.equipmentId).FirstOrDefault();
            EquipmentSO copiedhelm = helmSO.CreateCopy(item);
            equippedItems.Add(copiedhelm);
        }else equippedItems.Add(null);
        int j = 0;
        foreach(EquipmentSO equipment in equippedItems){
            if(equipment && equipment.equipmentType == EquipmentEnum.Armor){
                armor += equipment.baseArmor;
            }
            if(equipment && equipment.equipmentType == EquipmentEnum.Weapon && equipment.weaponType == WeaponType.Sword && j == 4){
                attackRange = equipment.attackRange;
            }
            if(equipment && equipment.equipmentType == EquipmentEnum.Weapon && equipment.weaponType == WeaponType.SpearJavelin && j == 4){
                attackRange = equipment.attackRange;
            }
            j++;
        }
        level = playerGameData.level;
        currentXP = playerGameData.currentXP;
        denarii = playerGameData.denarii;
        
        // Calculate the stats for the current level
        CalculateStatsForCurrentLevel();

        // Update UI elements
        levelText.SetText(Utilities.FormatNumber(level));
        hpSlider.maxValue = maxHP;
        hpSlider.value = currentHP;
        xpSlider.value = currentXP;
        xpSlider.maxValue = maxXP;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = staminaSlider.maxValue;
    }
    public void SaveQuestToServer()
    {
        foreach(QuestData quest in localPlayerData.gameData.quests){
            QuestSO qData = activeQuests.Where(q => q.questID == quest.questID).FirstOrDefault();
            if(qData != null){
                quest.isActive = qData.isActive;
                quest.isPinned = qData.isPinned;
                quest.isRewarded = qData.isRewarded;
                quest.completed = qData.isCompleted;
                quest.currentKnot = qData.currentKnot;
                quest.currentGoal = qData.currentGoal;
                for(int i = 0; i < quest.goals.Count; i++)
                {
                    quest.goals[i].currentAmount = qData.goals[i].currentAmount;
                }
                isDataDirty = true;
            }
        }        
    }
    private async void SaveDataToServer()
    {
        if (isDataDirty)
        {
            await AccountManager.SaveData(localPlayerData);
            isDataDirty = false; // Reset the dirty flag after saving
        }
    }
    
    private void Update()
    {
        updateValues();
    }

    public static PlayerStats GetInstance()
    {
        return Instance;
    }
    public void SetName(string name)
    {
        localPlayerData.gameData.playerName = name;
        isDataDirty = true; // Mark data as dirty
    }
    public void AddGold(int amount)
    {
        denarii += amount;
        localPlayerData.gameData.denarii += amount;
        isDataDirty = true; // Mark data as dirty
    }

    public void AddXp(int amount)
    {
        currentXP += amount;
        if(currentXP >= maxXP) LevelUp();
        AnimateXPChange(currentXP - amount, currentXP);

        localPlayerData.gameData.currentXP += amount;
        isDataDirty = true; // Mark data as dirty
    }
    public void AddQuest(QuestData qData, QuestSO questSO)
    {
        activeQuests.Add(questSO);
        localPlayerData.gameData.quests.Add(qData);
        isDataDirty = true; // Mark data as dirty
    }

    public void AddArtifact(string artifactID)
    {
        List<ArtifactsSO> artifactList = Resources.LoadAll<ArtifactsSO>("ArtifactSO").ToList();
        ArtifactsSO artifact = artifactList.Where(q => q.id == Convert.ToInt32(artifactID)).FirstOrDefault();
        ArtifactsData artifactData = new ArtifactsData(){
            artifactID = Convert.ToInt32(artifactID),
            acquiredDate = DateTime.Now,
        };
        localPlayerData.gameData.artifacts.Add(artifactData);
        isDataDirty = true; // Mark data as dirty
    }
    public void updateValues()
    {
        levelText.SetText(Utilities.FormatNumber(level));
        staminaSlider.value = stamina;
        staminaSlider.maxValue = maxStamina;
        xpSlider.value = currentXP;
        xpSlider.maxValue = maxXP;
        hpSlider.maxValue = maxHP;
        hpSlider.value = currentHP;
        int currentHPInt = Mathf.RoundToInt(currentHP);
        int maxHPInt = Mathf.RoundToInt(maxHP);
        int staminaInt = Mathf.RoundToInt(stamina);
        int maxStaminaInt = Mathf.RoundToInt(maxStamina);
        hpText.SetText("[" + currentHPInt + "/" + maxHPInt + "]");
        staminaText.SetText("[" + staminaInt + "/" + maxStaminaInt + "]");
    }
    private void AnimateXPChange(int startValue, int endValue)
    {
        DOTween.To(() => startValue, x =>
        {
            startValue = x;
            xpSlider.value = startValue;
        }, endValue, 1f).SetUpdate(true).SetEase(Ease.Linear);
    }

    private async void LevelUp()
    {
        level++;
        maxXP = CalculateXPToNextLevel(level);
        maxHP *= 1.05f; // Increase health by 5%
        currentHP = maxHP;
        maxStamina *= 1.03f; // Increase stamina by 3%
        attack *= 1.04f; // Increase attack by 4%
        staminaRegenRate *= 1.03f;
        currentXP = 0;
        
        localPlayerData.gameData.currentXP = currentXP;
        localPlayerData.gameData.maxXP = maxXP;
        localPlayerData.gameData.level = level;

        isDataDirty = true; // Mark data as dirty
        
        // Optionally save data immediately on level up
        await AccountManager.SaveData(localPlayerData);
        isDataDirty = false;
    }
    private void CalculateStatsForCurrentLevel()
    {
        // Calculate the stats based on the current level without incrementing it
        maxXP = CalculateXPToNextLevel(level);
        maxHP = 100f * Mathf.Pow(1.05f, level - 1); // Assuming initial maxHP is 100
        currentHP = maxHP;
        maxStamina = 70f * Mathf.Pow(1.03f, level - 1); // Assuming initial maxStamina is 70
        // attack = 30f * Mathf.Pow(1.04f, level - 1); // Assuming initial attack is 30
        staminaRegenRate = 10f * Mathf.Pow(1.03f, level - 1); // Assuming initial staminaRegenRate is 10
    }
    private int CalculateXPToNextLevel(int level)
    {
        return 30 + level * 15; // Example linear growth, starting at 30 XP
    }
}
