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
    [SerializeField] public TextMeshProUGUI level;
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
    [Header("Temp")]
    public int Level = 0;
    public int denarii = 0;
    public int xp = 0;
    public double solBalance = 101.023123045;
    public List<QuestSO> activeQuests;
    public List<QuestSO> completedQuests;
    private static PlayerStats Instance;
    
    private void Awake(){
        if(Instance != null){
            Debug.LogWarning("Found more than one Player Stats in the scene");
        }
        Instance = this;
    }
    void Start()
    {
        level.SetText(Utilities.FormatNumber(Level));
        denariiText.SetText(Utilities.FormatNumber(denarii));
        sol.SetText(Utilities.FormatSolana(solBalance));
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
    public static PlayerStats GetInstance(){
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
        xp += amount;
        Debug.Log("XP added: " + amount);
    }
    public void updateValues()
    {
        // Optionally, animate changes here as well
        // For now, it directly updates the text
        denariiText.SetText(Utilities.FormatNumber(denarii));
        sol.SetText(Utilities.FormatSolana(solBalance));
        level.SetText(Utilities.FormatNumber(Level));
    }

    private void AnimateGoldChange(int startValue, int endValue)
    {
        DOTween.To(() => startValue, x =>
        {
            startValue = x;
            denariiText.SetText(Utilities.FormatNumber(startValue));
        }, endValue, 1f).SetEase(Ease.Linear);
    }
}
