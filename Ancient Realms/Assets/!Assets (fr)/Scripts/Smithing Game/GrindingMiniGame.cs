using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class GrindingMiniGame : MonoBehaviour
{
    [Header("Tool Tip")]
    [SerializeField] GameObject tooltip;
    [Header("Slider Settings")]
    public Slider slider;
    public float incrementAmount = 0.1f; // Fixed amount to increase the slider per press
    public float decrementSpeed = 0.5f;
    private bool gameStarted = false;

    [Header("UI Elements")]
    public Text timerText;
    public Text startPromptText;
    public float roundTime = 10f;
    public float delayBeforeNextRound = 1f;

    [Header("Grindstone Animation Settings")]
    public GameObject grindstonePrefab;
    private Animator grindstoneAnimator;
    public string staticAnimationName = "Static";
    public string spinAnimationName = "Spin";

    [Header("Timer Radial Settings")]
    public Image timerCircleImage; // Radial timer image
    [Header("Proceed")]
    [SerializeField] SpriteRenderer item;
    [SerializeField] Sprite gSprite;
    [SerializeField] Sprite piSprite;
    [SerializeField] Sprite puSprite;
    [SerializeField] GameObject gladius;
    [SerializeField] GameObject pilum;
    [SerializeField] GameObject pugio;
    private bool gameOver = false;
    private float timeLeft;
    private bool timeRunning = false;

    void OnEnable()
    {
        if(PlayerStats.GetInstance().localPlayerData.gameData.uiSettings.Contains("grindstone")){
            tooltip.SetActive(false);
        }else{
            tooltip.SetActive(true);
        }
        item.gameObject.SetActive(true);
        switch(SmithingGameManager.GetInstance().order){
            case OrderType.Gladius:
                item.sprite = gSprite;
            break;
            case OrderType.Pila:
                item.sprite = piSprite;
            break;
            case OrderType.Pugio:
                item.sprite = puSprite;
            break;
        }
        InitializeSlider();
        InitializeTimer();
        DisplayStartPrompt(true);
        InitializeGrindstoneAnimation();
        SetGrindstoneAnimation(staticAnimationName); // Set the grindstone to Static state at the start
    }
    void OnDisable(){
        gladius.SetActive(false);
        pilum.SetActive(false);
        pugio.SetActive(false);
        SetGrindstoneAnimation(staticAnimationName);
        grindstonePrefab.SetActive(false);
        gameStarted = false;
        gameOver = false;
        timeRunning = false;
    }
    void Update()
    {
        if (!gameOver)
        {
            HandleSpaceBarPress();
            if (gameStarted)
            {
                UpdateSlider();
                UpdateTimer();
            }
        }
    }

    void StartGame()
    {
        gameStarted = true;
        DisplayStartPrompt(false);
        timeRunning = true;
        item.gameObject.SetActive(false);
        switch(SmithingGameManager.GetInstance().order){
            case OrderType.Gladius:
                gladius.SetActive(true);
                pilum.SetActive(false);
                pugio.SetActive(false);
                SetGrindstoneAnimation("Gladius Grind");
            break;
            case OrderType.Pila:
                gladius.SetActive(false);
                pilum.SetActive(true);
                pugio.SetActive(false);
                SetGrindstoneAnimation("Pila Grind");
            break;
            case OrderType.Pugio:
                gladius.SetActive(false);
                pilum.SetActive(false);
                pugio.SetActive(true);
                SetGrindstoneAnimation("Pugio Grind");
            break;
        }
    }

    void InitializeSlider()
    {
        slider.minValue = 0;
        slider.maxValue = 1;
        slider.value = 0;
    }

    void UpdateSlider()
    {
        // Always decrease the slider value over time
        slider.value -= decrementSpeed * Time.deltaTime;
        if (slider.value <= slider.minValue)
        {
            slider.value = slider.minValue;
        }
    }

    void InitializeGrindstoneAnimation()
    {
        if (grindstonePrefab != null)
        {
            grindstoneAnimator = grindstonePrefab.GetComponent<Animator>();
            if (grindstoneAnimator != null)
            {
                grindstonePrefab.SetActive(true); // Ensure prefab is active
            }
            else
            {
                Debug.LogError("Animator component is missing or not assigned on the grindstonePrefab.");
            }
        }
        else
        {
            Debug.LogError("Grindstone prefab is not assigned.");
        }
    }

    void SetGrindstoneAnimation(string animationName)
    {
        if (grindstoneAnimator != null)
        {
            grindstoneAnimator.Play(animationName);
        }
        else
        {
            Debug.LogError("Animator is not assigned in the SetGrindstoneAnimation method.");
        }
    }

    void InitializeTimer()
    {
        timeLeft = roundTime;
        UpdateTimerText();
        timerCircleImage.fillAmount = 1f; // Initialize radial timer to full
    }

    void UpdateTimer()
    {
        if (timeRunning && timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimerText();
            timerCircleImage.fillAmount = timeLeft / roundTime;

            if (timeLeft <= 0)
            {
                timeLeft = 0;
                StopGame();
                AudioManager.GetInstance().PlayAudio(SoundType.RED);
            }
        }
    }

    void UpdateTimerText()
    {
        if (timeLeft <= 0)
        {
            timerText.text = "0:00";
        }
        else
        {
            int seconds = Mathf.FloorToInt(timeLeft % 60);
            int milliseconds = Mathf.FloorToInt((timeLeft * 100) % 100);
            timerText.text = string.Format("{0}:{1:D2}", seconds, milliseconds);
        }
    }

    void StopGame()
    {
        gameOver = true;
        timeRunning = false;
        SetGrindstoneAnimation(staticAnimationName); // Set to Static animation before ending
        SmithingGameManager.GetInstance().EndWorkStation(WorkStation.Grindstone);
    }

    void DisplayStartPrompt(bool display)
    {
        startPromptText.gameObject.SetActive(display);
        if (display)
            startPromptText.text = "--- Press Spacebar to Start ---";
    }

    void HandleSpaceBarPress()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!gameStarted)
            {
                tooltip.SetActive(false);
                StartGame();
            }
            else
            {
                slider.value += incrementAmount; // Increase slider by a fixed amount per press

                if (slider.value >= slider.maxValue)
                {
                    slider.value = slider.maxValue;
                    StopGame(); // Stop the game when the slider is full
                    SmithingGameManager.GetInstance().score += 25;
                    SmithingGameManager.GetInstance().grindstoneUsed = true;
                    AudioManager.GetInstance().PlayAudio(SoundType.GREEN);
                }
            }
        }
    }
}
