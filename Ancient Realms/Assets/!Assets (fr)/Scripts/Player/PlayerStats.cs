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
    public bool isCombatMode = false;
    public bool toggleStamina = true;
    [Header("Temp")]
    public bool isDataDirty = false;
    public int level = 0;
    public int denarii = 0;
    public int maxXP = 30;
    public int currentXP = 0;
    public float damage = 0;
    public float attackRange = 0f;
    public float maxThrowForce = 20f; // Maximum force applied to the throw
    public float maxHoldTime = 1f;
    public List<EquipmentSO> equippedItems;
    public List<EquipmentSO> inventory;
    public List<QuestSO> activeQuests;
    public List<QuestSO> completedQuests;
    private static PlayerStats Instance;
    private List<EquipmentSO> equipmentLibrary;
    public bool firstLogin = true;
    public float interval = 1f; // Time interval in seconds for each invocation
    private bool isInvoking;
    public bool stopSaving = false;
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
        StartContinuousInvocation();
    }
    private void OnEnable()
    {
        LoadPlayerData(localPlayerData);
    }

    private void LoadPlayerData(PlayerData data)
    {
        GameData playerGameData = data.gameData;
        InitializeQuests(playerGameData);
        InitializeEquipments();
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
    public void SaveInventoryToServer()
    {
        foreach(ItemData item in localPlayerData.gameData.inventory.items){
            EquipmentSO iData = AccountManager.Instance.equipments.FirstOrDefault(q => q.equipmentId == item.equipmentId).CreateCopy(item);
            if(iData != null){
                item.equipmentId = iData.equipmentId;
                item.level = iData.level;
                item.tier = iData.tier;
                item.stackAmount = iData.stackCount;
                isDataDirty = true;
            }
        }        
    }
    private void StartContinuousInvocation()
    {
        if (!isInvoking)
        {
            isInvoking = true; // Set the flag to true
            StartCoroutine(InvokeContinuously());
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
    private IEnumerator InvokeContinuously()
    {
        while (isInvoking) // Loop while isInvoking is true
        {
            if(!stopSaving){
                SaveDataToServer();
            }
             // Call your desired method
            yield return new WaitForSecondsRealtime(interval); // Wait for the specified interval
        }
    }


    private void Update()
    {
        if(SmithingGameManager.GetInstance().inMiniGame){
            stopSaving = true;
        }else{
            stopSaving = false;
        }
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
        GameData playerGameData = localPlayerData.gameData;
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
    private void InitializeQuests(GameData playerGameData){
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
    }
    public void InitializeEquipments(){
        List<EquipmentSO> newList = new List<EquipmentSO>();
        int i = 0;
        equippedItems.Clear();
        foreach(ItemData equipment in localPlayerData.gameData.inventory.items){
            EquipmentSO equipmentSO = AccountManager.Instance.equipments.Where(eq => eq.equipmentId == equipment.equipmentId).FirstOrDefault();
            EquipmentSO copiedEquipmentSO = equipmentSO.CreateCopy(equipment, i);
            newList.Add(copiedEquipmentSO);
            i++;   
        }
        inventory = newList;
        if(localPlayerData.gameData.equippedData.helmSlot != null) {
            ItemData item = localPlayerData.gameData.equippedData.helmSlot;
            EquipmentSO helmSO = equipmentLibrary.Where(equipment => equipment.equipmentId == item.equipmentId).FirstOrDefault();
            EquipmentSO copiedhelm = helmSO.CreateCopy(item);
            equippedItems.Add(copiedhelm);
        }else equippedItems.Add(null);
        
        if(localPlayerData.gameData.equippedData.chestSlot != null) {
            ItemData item = localPlayerData.gameData.equippedData.chestSlot;
            EquipmentSO helmSO = equipmentLibrary.Where(equipment => equipment.equipmentId == item.equipmentId).FirstOrDefault();
            EquipmentSO copiedhelm = helmSO.CreateCopy(item);
            equippedItems.Add(copiedhelm);
        }else equippedItems.Add(null);

        if(localPlayerData.gameData.equippedData.waistSlot != null) {
            ItemData item = localPlayerData.gameData.equippedData.waistSlot;
            EquipmentSO helmSO = equipmentLibrary.Where(equipment => equipment.equipmentId == item.equipmentId).FirstOrDefault();
            EquipmentSO copiedhelm = helmSO.CreateCopy(item);
            equippedItems.Add(copiedhelm);
        }else equippedItems.Add(null);

        if(localPlayerData.gameData.equippedData.footSlot != null) {
            ItemData item = localPlayerData.gameData.equippedData.footSlot;
            EquipmentSO helmSO = equipmentLibrary.Where(equipment => equipment.equipmentId == item.equipmentId).FirstOrDefault();
            EquipmentSO copiedhelm = helmSO.CreateCopy(item);
            equippedItems.Add(copiedhelm);
        }else equippedItems.Add(null);

        if(localPlayerData.gameData.equippedData.mainSlot != null) {
            ItemData item = localPlayerData.gameData.equippedData.mainSlot;
            EquipmentSO helmSO = equipmentLibrary.Where(equipment => equipment.equipmentId == item.equipmentId).FirstOrDefault();
            EquipmentSO copiedhelm = helmSO.CreateCopy(item);
            equippedItems.Add(copiedhelm);
        }else equippedItems.Add(null);

        if(localPlayerData.gameData.equippedData.shieldSlot != null) {
            ItemData item = localPlayerData.gameData.equippedData.shieldSlot;
            EquipmentSO helmSO = equipmentLibrary.Where(equipment => equipment.equipmentId == item.equipmentId).FirstOrDefault();
            EquipmentSO copiedhelm = helmSO.CreateCopy(item);
            equippedItems.Add(copiedhelm);
        }else equippedItems.Add(null);

        if(localPlayerData.gameData.equippedData.javelinSlot != null) {
            ItemData item = localPlayerData.gameData.equippedData.javelinSlot;
            EquipmentSO helmSO = equipmentLibrary.Where(equipment => equipment.equipmentId == item.equipmentId).FirstOrDefault();
            EquipmentSO copiedhelm = helmSO.CreateCopy(item);
            equippedItems.Add(copiedhelm);
        }else equippedItems.Add(null);

        if(localPlayerData.gameData.equippedData.bandageSlot != null) {
            ItemData item = localPlayerData.gameData.equippedData.bandageSlot;
            EquipmentSO helmSO = equipmentLibrary.Where(equipment => equipment.equipmentId == item.equipmentId).FirstOrDefault();
            EquipmentSO copiedhelm = helmSO.CreateCopy(item);
            equippedItems.Add(copiedhelm);
        }else equippedItems.Add(null);
        int j = 0;
        float sumArmor = 0;
        damage = 1;
        foreach(EquipmentSO equipment in equippedItems){
            if(equipment && equipment.equipmentType == EquipmentEnum.Armor){
                sumArmor += equipment.baseArmor;
            }
            if(equipment && equipment.equipmentType == EquipmentEnum.Weapon && (equipment.weaponType == WeaponType.Sword ||  equipment.weaponType == WeaponType.Spear) && j == 4){
                damage = equipment.baseDamage;
                attackRange = equipment.attackRange;
            }
            j++;
        }
        armor = sumArmor;
    }
    private void CalculateStatsForCurrentLevel()
    {
        // Calculate the stats based on the current level without incrementing it
        maxXP = CalculateXPToNextLevel(level);
        maxHP = 100f * Mathf.Pow(1.02f, level - 1);
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
