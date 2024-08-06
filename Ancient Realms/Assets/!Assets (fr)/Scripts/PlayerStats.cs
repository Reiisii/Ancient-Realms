using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ESDatabase.Entities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Player UI")]
    [SerializeField] public GameObject UI;
    [SerializeField] public GameObject QuestPanel;
    [SerializeField] public Slider hpSlider;
    [SerializeField] public Slider staminaSlider;
    [SerializeField] public Slider xpSlider;
    [SerializeField] public TextMeshProUGUI levelText;
    [SerializeField] public TextMeshProUGUI denariiText;
    [SerializeField] public TextMeshProUGUI sol;
    [Header("Scripts")]
    public PlayerData playerData;
    [Header("Player Stats")]
    public float HP = 100f;
    public float maxStamina = 70f;
    public float stamina = 70f;
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float staminaDepletionRate = 40f;
    public float staminaRegenRate = 10f;
    public float attack = 30;
    [Header("Temp")]
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

    void Start()
    {
        levelText.SetText(Utilities.FormatNumber(level));
        denariiText.SetText(Utilities.FormatNumber(denarii));
        sol.SetText(Utilities.FormatSolana(solBalance));
        xpSlider.value = currentXP;
        xpSlider.maxValue = maxXP;
    }

    private void Update()
    {
        updateValues();
        PlayerController playerController = PlayerController.GetInstance();
        if (!playerController.moveInputActive && playerController.IsRunning || !playerController.IsRunning)
        {
            stamina = Mathf.Min(maxStamina, stamina + staminaRegenRate * Time.deltaTime);
        }
        // Update stamina slider
        staminaSlider.value = stamina;
    }

    public static PlayerStats GetInstance()
    {
        return Instance;
    }

    public void AddGold(int amount)
    {
        denarii += amount;
        AnimateGoldChange(denarii - amount, denarii);
        Debug.Log("Gold added: " + amount);
    }

    public void AddXp(int amount)
    {
        StartCoroutine(AddXpCoroutine(amount));
    }

    private IEnumerator AddXpCoroutine(int amount)
    {
        int startXP = currentXP;
        int totalXP = currentXP + amount;
        
        while (totalXP >= maxXP && level < 30)
        {
            int xpToNextLevel = maxXP - currentXP;
            AnimateXPChange(currentXP, maxXP);
            yield return new WaitForSeconds(1f); // Wait for the XP animation to complete
            
            totalXP -= xpToNextLevel;
            currentXP = 0;
            LevelUp();
        }

        currentXP = totalXP;
        AnimateXPChange(0, currentXP);
    }

    public void updateValues()
    {
        denariiText.SetText(Utilities.FormatNumber(denarii));
        sol.SetText(Utilities.FormatSolana(solBalance));
        levelText.SetText(Utilities.FormatNumber(level));
        xpSlider.value = currentXP;
        xpSlider.maxValue = maxXP;
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

    private void LevelUp()
    {
        level++;
        maxXP = CalculateXPToNextLevel(level);
        HP *= 1.05f; // Increase health by 5%
        maxStamina *= 1.03f; // Increase stamina by 3%
        attack *= 1.04f; // Increase attack by 4%

        updateValues();
    }

    private int CalculateXPToNextLevel(int level)
    {
        return 30 + level * 10; // Example linear growth, starting at 30 XP
    }
}
