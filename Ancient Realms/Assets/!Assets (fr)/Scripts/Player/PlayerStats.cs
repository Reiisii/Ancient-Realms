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
    public bool isDead = false;
    [Header("Movement Stats")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float staminaDepletionRate = 40f;
    public float staminaRegenRate = 20f;
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
    [Header("Base Stats")]
    public float baseHP;
    public float baseStamina;
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
        level = playerGameData.level;
        currentXP = playerGameData.currentXP;
        // maxXP = CalculateXPToNextLevel(level);;
        denarii = playerGameData.denarii;
        
        // Calculate the stats for the current level
        CalculateStatsForCurrentLevel();
        xpSlider.value = currentXP;
        xpSlider.maxValue = maxXP;
        InitializeQuests(playerGameData);
        InitializeEquipments();
        if(currentXP > maxXP){
            LevelUp();
        }
        // Update UI elements
        levelText.SetText(Utilities.FormatNumber(level));
        // hpSlider.maxValue = maxHP;
        // hpSlider.value = currentHP;
        // staminaSlider.maxValue = maxStamina;
        // staminaSlider.value = staminaSlider.maxValue;
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
    public void ReplenishStats(){
        isDead = false;
        currentHP = maxHP;
        stamina = maxStamina;
        isCombatMode = false;
    }

    private void Update()
    {
        LocationSO loadedLocation = LocationSettingsManager.GetInstance().locationSettings;
        if(loadedLocation != null && loadedLocation.instanceMission){
            stopSaving = true;
        }else if(SmithingGameManager.GetInstance().inMiniGame){
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
        AddStatistics(StatisticsType.DenariiTotal, amount.ToString());
        isDataDirty = true; // Mark data as dirty
    }

    public void AddXp(int amount)
    {
        // Store the previous XP before adding
        int previousXP = currentXP;
        
        currentXP += amount;
        
        // Check for level up
        if (currentXP >= maxXP) 
        {
            LevelUp();
        }
        
        // Animate the XP change
        AnimateXPChange(previousXP, currentXP);
        
        // Update the local player's data
        localPlayerData.gameData.currentXP += amount;
        isDataDirty = true;
    }
    public void SetRank(string rank)
    {
        switch(rank){
            case "Tiro":
                localPlayerData.gameData.rank = "Tiro";
                if(PlayerController.GetInstance() != null){
                    PlayerController.GetInstance().gameObject.GetComponent<SpriteRenderer>().sprite = PlayerController.GetInstance().skins[0];
                }
            break;
            case "Legionnaire":
                localPlayerData.gameData.rank = "Legionnaire";
                if(PlayerController.GetInstance() != null){
                    PlayerController.GetInstance().gameObject.GetComponent<SpriteRenderer>().sprite = PlayerController.GetInstance().skins[1];
                }
            break;
            case "Centurion":
                localPlayerData.gameData.rank = "Centurion";
                if(PlayerController.GetInstance() != null){
                    PlayerController.GetInstance().gameObject.GetComponent<SpriteRenderer>().sprite = PlayerController.GetInstance().skins[2];
                }
            break;
        }
        isDataDirty = true;
    }

    private void AnimateXPChange(int startValue, int endValue)
    {
        // Use a separate variable for the animation
        DOTween.To(() => startValue, x =>
        {
            // Update the value being animated
            startValue = x;
            xpSlider.value = startValue;
        }, endValue, 1f)
        .SetUpdate(true)
        .SetEase(Ease.Linear)
        .OnComplete(() =>
        {
            // Ensure xpSlider reflects the final value at the end of animation
            xpSlider.value = endValue;

        });
    }

    public void AddQuest(QuestData qData, QuestSO questSO)
    {
        activeQuests.Add(questSO);
        localPlayerData.gameData.quests.Add(qData);
        isDataDirty = true; // Mark data as dirty
    }
    public void AddStatistics(StatisticsType type, string amount){
        StatisticsData statisticsData = localPlayerData.gameData.statistics;
        switch(type){
            case StatisticsType.MoveDistance:
                statisticsData.moveDistanceTotal += float.Parse(amount);
            break;
            case StatisticsType.Kill:
                statisticsData.kills += int.Parse(amount);
            break;
            case StatisticsType.DenariiTotal:
                statisticsData.denariiTotal += int.Parse(amount);
            break;
            case StatisticsType.SmithingTotal:
                statisticsData.smithingTotal += int.Parse(amount);
            break;
            case StatisticsType.DeathTotal:
                statisticsData.deathTotal += int.Parse(amount);
            break;
        }
        isDataDirty = true;
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
    public void AddItem(int itemID, int tier, int level, int amount)
    {
        EquipmentSO item = equipmentLibrary.Where(q => q.equipmentId.Equals(itemID)).FirstOrDefault();
        AddEncyc(EncycType.Equipment, itemID);
        ItemData existingItemData = localPlayerData.gameData.inventory.items.FirstOrDefault(itm => itm.equipmentId.Equals(item.equipmentId));
        if(item.isStackable && existingItemData != null){
            existingItemData.stackAmount += amount;
        }else{
            ItemData itemData = new ItemData(item.equipmentId, tier, level, amount);
            localPlayerData.gameData.inventory.items.Add(itemData);
        }
        isDataDirty = true; // Mark data as dirty
    }
    public void AddEncyc(EncycType encycType, int value)
    {
        switch(encycType){
            case EncycType.Character:
                if(localPlayerData.gameData.characters.Contains(value)) return;
                localPlayerData.gameData.characters.Add(value);
                Notification chNotif = new Notification(){
                            title = "character",
                            notifType = NotifType.Character
                };
                PlayerUIManager.GetInstance().notification.AddQueue(chNotif);
            break;
            case EncycType.Event:
                if(localPlayerData.gameData.events.Contains(value)) return;
                localPlayerData.gameData.events.Add(value);
                Notification evNotif = new Notification(){
                            title = "Event",
                            notifType = NotifType.Event
                };
                PlayerUIManager.GetInstance().notification.AddQueue(evNotif);
            break;
            case EncycType.Equipment:
                if(localPlayerData.gameData.equipments.Contains(value)) return;
                localPlayerData.gameData.equipments.Add(value);
                Notification eqNotif = new Notification(){
                            title = "Equipment",
                            notifType = NotifType.Equipment
                };
                PlayerUIManager.GetInstance().notification.AddQueue(eqNotif);
            break;
        }
        isDataDirty = true; // Mark data as dirty
    }
    public void updateValues()
    {
        GameData playerGameData = localPlayerData.gameData;
        levelText.SetText(Utilities.FormatNumber(level));
        hpSlider.maxValue = maxHP;
        hpSlider.value = currentHP;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = stamina;
        // xpSlider.value = playerGameData.currentXP;

        hpText.SetText($"[{Utilities.ConvertToOneDecimal(currentHP)}/{Utilities.ConvertToOneDecimal(maxHP)}]");
        staminaText.SetText($"[{Utilities.ConvertToOneDecimal(stamina)}/{Utilities.ConvertToOneDecimal(maxStamina)}]");
    }

    private async void LevelUp()
    {
        level++;
        int excessXP = currentXP - maxXP;
        maxXP = CalculateXPToNextLevel(level);
        CalculateBaseStats();
        currentHP = maxHP;
        staminaRegenRate *= 1.03f;
        
        currentXP = excessXP;
        
        localPlayerData.gameData.currentXP = currentXP;
        localPlayerData.gameData.maxXP = maxXP;
        localPlayerData.gameData.level = level;
        xpSlider.maxValue = maxXP;
        isDataDirty = true;

        await AccountManager.SaveData(localPlayerData);
        isDataDirty = false;
        InitializeEquipments();
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
        InitializeEquippedItems();
        int j = 0;
        CalculateStatsForCurrentLevel();
        float baseArmor = 0;
        maxHP = baseHP;
        maxStamina = baseStamina;
        armor = baseArmor;
        walkSpeed = 3.5f;
        runSpeed = 5.5f;
        damage = 1;
        foreach (EquipmentSO equipment in equippedItems) {
            if (equipment && equipment.equipmentType == EquipmentEnum.Armor) {
                armor += equipment.baseArmor;
            }
            if (equipment && equipment.equipmentType == EquipmentEnum.Weapon && 
                (equipment.weaponType == WeaponType.Sword || equipment.weaponType == WeaponType.Spear) && j == 4) {
                damage = equipment.baseDamage;
                attackRange = equipment.attackRange;
            }
            j++;
        }
        foreach (NFTData nftData in localPlayerData.gameData.equippedNFT) {
            if (nftData != null) {
                NFTSO nftSO = AccountManager.Instance.nfts.FirstOrDefault(nft => nft.id.Equals(nftData.nftID));
                if (nftSO != null) {
                    foreach (StatBuff buffType in nftSO.buffList) {
                        switch (buffType.buffType) {
                            case BuffType.Health:
                                maxHP += buffType.value;
                                break;
                            case BuffType.Armor:
                                armor += buffType.value;
                                break;
                            case BuffType.Stamina:
                                maxStamina += buffType.value;
                                break;
                            case BuffType.Speed:
                                walkSpeed += buffType.value;
                                runSpeed += buffType.value;
                                break;
                        }
                    }
                }
            }
        }
        currentHP = maxHP;
        stamina = maxStamina;
    }
    private void InitializeEquippedItems()
    {
        // Populate equippedItems based on equipped slots, resetting null if slot is empty
        equippedItems.Add(localPlayerData.gameData.equippedData.helmSlot != null ? GetCopiedEquipment(localPlayerData.gameData.equippedData.helmSlot) : null);
        equippedItems.Add(localPlayerData.gameData.equippedData.chestSlot != null ? GetCopiedEquipment(localPlayerData.gameData.equippedData.chestSlot) : null);
        equippedItems.Add(localPlayerData.gameData.equippedData.waistSlot != null ? GetCopiedEquipment(localPlayerData.gameData.equippedData.waistSlot) : null);
        equippedItems.Add(localPlayerData.gameData.equippedData.footSlot != null ? GetCopiedEquipment(localPlayerData.gameData.equippedData.footSlot) : null);
        equippedItems.Add(localPlayerData.gameData.equippedData.mainSlot != null ? GetCopiedEquipment(localPlayerData.gameData.equippedData.mainSlot) : null);
        equippedItems.Add(localPlayerData.gameData.equippedData.shieldSlot != null ? GetCopiedEquipment(localPlayerData.gameData.equippedData.shieldSlot) : null);
        equippedItems.Add(localPlayerData.gameData.equippedData.javelinSlot != null ? GetCopiedEquipment(localPlayerData.gameData.equippedData.javelinSlot) : null);
        equippedItems.Add(localPlayerData.gameData.equippedData.bandageSlot != null ? GetCopiedEquipment(localPlayerData.gameData.equippedData.bandageSlot) : null);
    }
    private EquipmentSO GetCopiedEquipment(ItemData itemData)
    {
        EquipmentSO equipmentSO = equipmentLibrary.Where(equipment => equipment.equipmentId == itemData.equipmentId).FirstOrDefault();
        return equipmentSO?.CreateCopy(itemData);
    }

    private void CalculateStatsForCurrentLevel()
    {
        // Calculate the stats based on the current level without incrementing it
        CalculateBaseStats();  // Ensure base stats are calculated
        maxXP = CalculateXPToNextLevel(level);
        maxHP = baseHP * Mathf.Pow(1.05f, level - 1);  // Scale max HP based on the base value
        currentHP = maxHP;  // Reset current HP to max after calculating
        maxStamina = baseStamina * Mathf.Pow(1.02f, level - 1);  // Scale max Stamina based on the base value
        staminaRegenRate = 20f * Mathf.Pow(1.03f, level - 1);  // Assuming initial staminaRegenRate is 10
    }

    private void CalculateBaseStats()
    {
        // Base calculations for stats based on the current level
        baseHP = 100f * Mathf.Pow(1.01f, level - 1); // Starting HP calculation
        baseStamina = 70f * Mathf.Pow(1.02f, level - 1); // Starting Stamina calculation
        staminaRegenRate = 20f * Mathf.Pow(1.03f, level - 1); // Starting stamina regen rate calculation
    }

    private int CalculateXPToNextLevel(int level)
    {
        return 30 + level * 15; // Example linear growth, starting at 30 XP
    }
}
