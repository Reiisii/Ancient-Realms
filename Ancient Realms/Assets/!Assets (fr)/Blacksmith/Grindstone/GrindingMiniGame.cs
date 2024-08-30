using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GrindingMiniGame : MonoBehaviour
{
    [Header("Slider Settings")]
    public Slider slider;
    public float speed = 1.0f;
    public float decrementSpeed = 0.5f;
    private bool gameStarted = false;
    private bool spacebarPressed = false; // To track if spacebar is pressed or not

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

    private bool gameOver = false;
    private float timeLeft;
    private bool timeRunning = false;

    void Start()
    {
        InitializeSlider();
        InitializeTimer();
        DisplayStartPrompt(true);
        InitializeGrindstoneAnimation();
        SetGrindstoneAnimation(staticAnimationName); // Set the grindstone to Static state at the start
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
        SetGrindstoneAnimation(spinAnimationName); // Start the spinning animation
    }

    void InitializeSlider()
    {
        slider.minValue = 0;
        slider.maxValue = 1;
        slider.value = 0;
    }

    void UpdateSlider()
    {
        if (spacebarPressed)
        {
            slider.value += speed * Time.deltaTime;
            if (slider.value >= slider.maxValue)
            {
                slider.value = slider.maxValue;
                StopGame(); // Stop the game when the slider is full
            }
        }
        else
        {
            slider.value -= decrementSpeed * Time.deltaTime;
            if (slider.value <= slider.minValue)
            {
                slider.value = slider.minValue;
            }
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
        Debug.Log("Game over.");
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
                StartGame();
            }
            else
            {
                spacebarPressed = true; // Set spacebarPressed to true when spacebar is initially pressed
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            spacebarPressed = false; // Set spacebarPressed to false when spacebar is released
        }
    }
}
