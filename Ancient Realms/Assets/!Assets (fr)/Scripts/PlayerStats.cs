using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using ESDatabase.Classes;
using ESDatabase.Entities;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] public TextMeshProUGUI denariiText;
    [SerializeField] public TextMeshProUGUI sol;
    [Header("Scripts")]
    public PlayerData localPlayerData;

    [Header("Player Stats")]
    public float currentHP = 100f;
    public float maxHP = 100f;
    public float maxStamina = 70f;
    public float stamina = 70f;
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
    public double solBalance = 101.023123045;
    public List<QuestSO> activeQuests;
    public List<QuestSO> completedQuests;
    private static PlayerStats Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Found more than one Player Stats in the scene");
        }
        Instance = this;
    }

    async void Start()
    {
        localPlayerData = await AccountManager.GetPlayer();
        LoadPlayerData(localPlayerData);
        InvokeRepeating("SaveDataToServer", 3f, 3f); // Save data to the server every 10 seconds
    }
    private void LoadPlayerData(PlayerData data)
    {
        GameData playerGameData = data.gameData;
        foreach(QuestData quest in playerGameData.quests){
            if(quest.isActive == true){
                QuestSO qData = QuestManager.GetInstance().quests.Find(q => q.questID == quest.questID);
                qData.currentKnot = quest.currentKnot;
                qData.currentGoal = quest.currentGoal;
                int i = 0;
                foreach(GoalData goal in quest.goals){
                    qData.goals[i].currentAmount = goal.currentAmount;
                    qData.goals[i].requiredAmount = goal.requiredAmount;
                    i++;
                }
                activeQuests.Add(qData);
            }else if(quest.completed == true){
                QuestSO qData = QuestManager.GetInstance().quests.Find(q => q.questID == quest.questID);
                qData.currentKnot = quest.currentKnot;
                qData.currentGoal = quest.currentGoal;
                int i = 0;
                foreach(GoalData goal in quest.goals){
                    qData.goals[i].currentAmount = goal.currentAmount;
                    qData.goals[i].requiredAmount = goal.requiredAmount;
                    i++;
                }
                completedQuests.Add(qData);
            }
        }
        level = playerGameData.level;
        currentXP = playerGameData.currentXP;
        denarii = playerGameData.denarii;
        
        // Calculate the stats for the current level
        CalculateStatsForCurrentLevel();

        // Update UI elements
        levelText.SetText(Utilities.FormatNumber(level));
        denariiText.SetText(Utilities.FormatNumber(denarii));
        sol.SetText(Utilities.FormatSolana(solBalance));
        hpSlider.maxValue = maxHP;
        hpSlider.value = currentHP;
        xpSlider.value = currentXP;
        xpSlider.maxValue = maxXP;
    }
    public void SaveQuestToServer()
    {
        GameData playerGameData = localPlayerData.gameData;
        
        foreach(QuestData quest in playerGameData.quests){
            QuestSO qData = activeQuests.Find(q => q.questID == quest.questID);
            if(quest.isActive == true){
                quest.isActive = qData.isActive;
                quest.completed = qData.isCompleted;
                quest.currentKnot = qData.currentKnot;
                quest.currentGoal = qData.currentGoal;
                int i = 0;
                foreach(GoalData goal in quest.goals){
                    quest.goals[i].currentAmount = qData.goals[i].currentAmount;
                    quest.goals[i].requiredAmount = qData.goals[i].requiredAmount;
                    i++;
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
        PlayerController playerController = PlayerController.GetInstance();
        if (!playerController.moveInputActive && playerController.IsRunning || !playerController.IsRunning)
        {
            stamina = Mathf.Min(maxStamina, stamina + staminaRegenRate * Time.deltaTime);
        }
    }

    public static PlayerStats GetInstance()
    {
        return Instance;
    }

    public void AddGold(int amount)
    {
        denarii += amount;
        AnimateGoldChange(denarii - amount, denarii);
        
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


    public void updateValues()
    {
        denariiText.SetText(Utilities.FormatNumber(denarii));
        sol.SetText(Utilities.FormatSolana(solBalance));
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

    private void AnimateGoldChange(int startValue, int endValue)
    {
        DOTween.To(() => startValue, x =>
        {
            startValue = x;
            denariiText.SetText(Utilities.FormatNumber(startValue));
        }, endValue, 1f).SetEase(Ease.Linear);
    }

    private void AnimateXPChange(int startValue, int endValue)
    {
        DOTween.To(() => startValue, x =>
        {
            startValue = x;
            xpSlider.value = startValue;
        }, endValue, 1f).SetEase(Ease.Linear);
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
        updateValues();
    }
    private void CalculateStatsForCurrentLevel()
    {
        // Calculate the stats based on the current level without incrementing it
        maxXP = CalculateXPToNextLevel(level);
        maxHP = 100f * Mathf.Pow(1.05f, level - 1); // Assuming initial maxHP is 100
        currentHP = maxHP;
        maxStamina = 70f * Mathf.Pow(1.03f, level - 1); // Assuming initial maxStamina is 70
        attack = 30f * Mathf.Pow(1.04f, level - 1); // Assuming initial attack is 30
        staminaRegenRate = 10f * Mathf.Pow(1.03f, level - 1); // Assuming initial staminaRegenRate is 10
    }
    private async void RefreshData()
    {
        localPlayerData = await AccountManager.GetPlayer();
        LoadPlayerData(localPlayerData);
    }
    private int CalculateXPToNextLevel(int level)
    {
        return 30 + level * 15; // Example linear growth, starting at 30 XP
    }
}
