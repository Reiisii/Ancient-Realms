using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderMiniGame : MonoBehaviour
{
    [Header("Slider Settings")]
    public Slider slider;
    public float speed = 1.0f;
    public float speedIncrement = 0.5f;
    private bool isMoving = false;
    private bool isMovingRight = true;

    [Header("Game Settings")]
    public int maxPresses = 5;
    private int pressCount = 0;
    private int score = 0;

    [Header("UI Elements")]
    public Text scoreText;
    public Text timerText;
    public Text startPromptText;
    public float roundTime = 5f;
    public float delayBeforeNextRound = 1f;

    [Header("Score Areas")]
    public RectTransform leftRed, leftYellow, green, rightYellow, rightRed;

    [Header("Score Values")]
    public int redScore = 1;
    public int yellowScore = 2;
    public int greenScore = 3;

    [Header("Score Circle Settings")]
    public Image[] scoreCircles;
    public Color redColor = Color.red;
    public Color yellowColor = Color.yellow;
    public Color greenColor = Color.green;

    [Header("Hammer Animation Settings")]
    public GameObject hammerPrefab;
    private Animator hammerAnimator;
    public string hammerAnimationParam = "isHammering";

    [Header("Timer Radial Settings")]
    public Image timerCircleImage;

    private bool gameOver = false;
    private bool gameStarted = false;
    private float timeLeft;
    private bool timeRunning = false;

    void Start()
    {
        InitializeSlider();
        InitializeTimer();
        DisplayStartPrompt(true);

        InitializeHammer();
    }

    void Update()
    {
        if (!gameOver)
        {
            HandleSpaceBarPress();
            if (isMoving)
            {
                MoveSlider();
                UpdateTimer();
            }
        }
    }

    void StartGame()
    {
        gameStarted = true;
        DisplayStartPrompt(false);
        StartSlider();
        SetHammerAnimation("Static");
        Debug.Log("Round 1 started.");
    }

    void StartSlider()
    {
        isMoving = true;
        timeRunning = true;
        ResetSlider();
        ResetTimer();
    }

    void MoveSlider()
    {
        slider.value += (isMovingRight ? 1 : -1) * speed * Time.deltaTime;

        if (slider.value >= slider.maxValue || slider.value <= slider.minValue)
            isMovingRight = !isMovingRight;
    }

    void StopSlider()
    {
        isMoving = false;
        string color = DetermineSliderColor();
        Debug.Log("Stopped at color: " + color);
    }

    void InitializeSlider()
    {
        slider.minValue = 0;
        slider.maxValue = 1;
        slider.value = 0.5f;
    }

    void ResetSlider()
    {
        slider.value = 0.5f;
        isMovingRight = true;
    }

    void IncreaseSpeed()
    {
        if (pressCount < maxPresses)
            speed += speedIncrement;
    }

    void CalculateScore()
    {
        Vector3 handlePosition = slider.handleRect.position;

        if (RectTransformUtility.RectangleContainsScreenPoint(leftRed, handlePosition) || RectTransformUtility.RectangleContainsScreenPoint(rightRed, handlePosition))
            score += redScore;
        else if (RectTransformUtility.RectangleContainsScreenPoint(leftYellow, handlePosition) || RectTransformUtility.RectangleContainsScreenPoint(rightYellow, handlePosition))
            score += yellowScore;
        else if (RectTransformUtility.RectangleContainsScreenPoint(green, handlePosition))
            score += greenScore;

        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    void UpdateScoreCircle()
    {
        if (pressCount < scoreCircles.Length)
        {
            Color color = DetermineScoreCircleColor();
            scoreCircles[pressCount].color = color;
        }
        else
        {
            Debug.LogWarning("Press count exceeds the number of circles.");
        }
    }


    Color DetermineScoreCircleColor()
    {
        Vector3 handlePosition = slider.handleRect.position;

        if (RectTransformUtility.RectangleContainsScreenPoint(leftRed, handlePosition) || RectTransformUtility.RectangleContainsScreenPoint(rightRed, handlePosition))
            return redColor;
        else if (RectTransformUtility.RectangleContainsScreenPoint(leftYellow, handlePosition) || RectTransformUtility.RectangleContainsScreenPoint(rightYellow, handlePosition))
            return yellowColor;
        else if (RectTransformUtility.RectangleContainsScreenPoint(green, handlePosition))
            return greenColor;

        Debug.LogWarning("Handle position did not match any color areas.");
        return Color.clear; // Return a transparent color if no match found
    }

    void InitializeTimer()
    {
        timeLeft = roundTime;
        UpdateTimerText();
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
                StopSlider();
                StopTimer();
                EndGame();
            }
        }
    }

    void ResetTimer()
    {
        timeLeft = roundTime;
        timeRunning = true;
        UpdateTimerText();
    }

    void StopTimer()
    {
        timeRunning = false;
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


    IEnumerator WaitAndStartNextRound(float delay)
    {
        Debug.Log("Waiting for " + delay + " seconds before starting the next round...");
        yield return new WaitForSeconds(delay);

        if (pressCount < maxPresses)
        {
            ResetSlider();
            StartSlider();
            IncreaseSpeed();
            SetHammerAnimation("Static");
            Debug.Log("Round " + (pressCount + 1) + " started.");
        }
        else
        {
            EndGame();
        }
    }

    void EndGame()
    {
        if (!gameOver)
        {
            isMoving = false;
            gameOver = true;
            SetHammerAnimation("Static");
            Debug.Log("Game over.");
        }
    }

    void SetHammerAnimation(string animationName)
    {
        if (hammerAnimator != null)
            hammerAnimator.Play(animationName);
    }

    void DisplayStartPrompt(bool display)
    {
        startPromptText.gameObject.SetActive(display);
        if (display)
            startPromptText.text = "--- Press Spacebar to Start ---";
    }

    void InitializeHammer()
    {
        if (hammerPrefab != null)
        {
            hammerAnimator = hammerPrefab.GetComponent<Animator>();
            hammerPrefab.SetActive(true);
            SetHammerAnimation("Static");
        }
    }

    string DetermineSliderColor()
    {
        Vector3 handlePosition = slider.handleRect.position;

        if (RectTransformUtility.RectangleContainsScreenPoint(leftRed, handlePosition) || RectTransformUtility.RectangleContainsScreenPoint(rightRed, handlePosition))
            return "Red";
        else if (RectTransformUtility.RectangleContainsScreenPoint(leftYellow, handlePosition) || RectTransformUtility.RectangleContainsScreenPoint(rightYellow, handlePosition))
            return "Yellow";
        else if (RectTransformUtility.RectangleContainsScreenPoint(green, handlePosition))
            return "Green";

        return "Unknown";
    }

    void HandleSpaceBarPress()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!gameStarted)
            {
                StartGame();
            }
            else if (pressCount < maxPresses)
            {
                if (isMoving)
                {
                    StopSlider();
                    CalculateScore();
                    UpdateScoreCircle();
                    StopTimer();
                    pressCount++;
                    StartCoroutine(WaitAndStartNextRound(delayBeforeNextRound));
                    SetHammerAnimation("Hammering");
                }
            }
        }
    }
}
