using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderMiniGame : MonoBehaviour
{
    public Slider slider;
    public float speed = 1.0f;
    private bool isMoving = false;
    private bool isMovingRight = true;

    private float speedIncrement = 0.5f;
    private int pressCount = 0;
    private int maxPresses = 5;

    public RectTransform leftRed, leftYellow, green, rightYellow, rightRed;
    private int score = 0;

    public Text scoreText;
    public Text timerText;
    public Text startPromptText; // Text UI element for the start prompt
    public float roundTime = 5f; // Time for each round in seconds
    private float timeLeft;
    private bool timeRunning = false;

    public float delayBeforeNextRound = 1f; // Customizable delay before the next round starts

    // Customizable scores
    public int redScore = 1;
    public int yellowScore = 2;
    public int greenScore = 3;

    private bool gameOver = false;
    private bool gameStarted = false;

    public GameObject hammerPrefab; // Reference to the Hammer Prefab
    private Animator hammerAnimator; // Animator component for the Hammer
    public string hammerAnimationParam = "isHammering"; // The name of the animation parameter

    void Start()
    {
        InitializeSlider();
        InitializeTimer();
        startPromptText.text = "Press Spacebar to Start"; // Show the start prompt

        // Initialize hammer
        if (hammerPrefab != null)
        {
            hammerAnimator = hammerPrefab.GetComponent<Animator>();
            hammerPrefab.SetActive(true); // Make sure hammer is active
            SetHammerAnimation("Static"); // Set to Static animation at the start
        }
    }

    void Update()
    {
        if (!gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!gameStarted)
                {
                    StartGame(); // Start the game on the first spacebar press
                }
                else if (pressCount < maxPresses)
                {
                    if (isMoving)
                    {
                        StopSlider();
                        CalculateScore();
                        StopTimer();
                        pressCount++;
                        StartCoroutine(WaitAndStartNextRound(delayBeforeNextRound));

                        // Play Hammering animation when space is pressed
                        SetHammerAnimation("Hammering");
                    }
                }
            }

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
        startPromptText.gameObject.SetActive(false); // Hide the start prompt
        StartSlider(); // Start moving the slider and timer

        // Ensure hammer is active and play Static animation initially
        if (hammerPrefab != null && hammerAnimator != null)
        {
            hammerPrefab.SetActive(true);
            SetHammerAnimation("Static");
        }

        Debug.Log("Round 1 started."); // Log message for the first round
    }

    void StartSlider()
    {
        isMoving = true;
        timeRunning = true; // Start the timer
        ResetSlider();
        ResetTimer();
    }

    void MoveSlider()
    {
        if (isMovingRight)
        {
            slider.value += speed * Time.deltaTime;
            if (slider.value >= slider.maxValue)
            {
                isMovingRight = false;
            }
        }
        else
        {
            slider.value -= speed * Time.deltaTime;
            if (slider.value <= slider.minValue)
            {
                isMovingRight = true;
            }
        }
    }

    void StopSlider()
    {
        isMoving = false;
        string color = DetermineSliderColor(); // Determine the color where the slider stopped
        Debug.Log("Stopped at color: " + color);
    }

    void InitializeSlider()
    {
        slider.minValue = 0;
        slider.maxValue = 1;
        slider.value = 0.5f; // Start in the middle or any initial value
    }

    void ResetSlider()
    {
        slider.value = 0.5f; // Reset slider to the starting position
        isMovingRight = true; // Ensure slider starts moving in the correct direction
    }

    void IncreaseSpeed()
    {
        if (pressCount < maxPresses)
        {
            speed += speedIncrement;
        }
    }

    void CalculateScore()
    {
        Vector3 handlePosition = slider.handleRect.position;

        if (RectTransformUtility.RectangleContainsScreenPoint(leftRed, handlePosition))
        {
            score += redScore; // Red score
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(leftYellow, handlePosition))
        {
            score += yellowScore; // Yellow score
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(green, handlePosition))
        {
            score += greenScore; // Green score
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(rightYellow, handlePosition))
        {
            score += yellowScore; // Yellow score
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(rightRed, handlePosition))
        {
            score += redScore; // Red score
        }

        scoreText.text = "Score: " + score;
        Debug.Log("Score: " + score);
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

            if (timeLeft <= 0)
            {
                timeLeft = 0; // Ensure the timer doesn't go below zero
                StopSlider();
                StopTimer();
                EndGame(); // End the game immediately when time runs out
            }
        }
    }

    void ResetTimer()
    {
        timeLeft = roundTime;
        timeRunning = true; // Reset and start the timer
        UpdateTimerText();
    }

    void StopTimer()
    {
        timeRunning = false; // Stop the timer
    }

    void UpdateTimerText()
    {
        // Convert timeLeft to seconds and milliseconds (rounded to nearest hundred)
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        int milliseconds = Mathf.FloorToInt((timeLeft * 100) % 100);

        // Format the time string as SS:MM
        timerText.text = string.Format("{0:D2}:{1:D2}", seconds, milliseconds);
    }

    IEnumerator WaitAndStartNextRound(float delay)
    {
        Debug.Log("Waiting for " + delay + " seconds before starting the next round...");
        yield return new WaitForSeconds(delay);

        // Check if we still have rounds left
        if (pressCount < maxPresses)
        {
            ResetSlider();
            StartSlider();
            IncreaseSpeed();
            Debug.Log("Round " + (pressCount + 1) + " started."); // Log message for each round

            // Reset hammer animation to Static after round delay
            if (hammerPrefab != null && hammerAnimator != null)
            {
                SetHammerAnimation("Static");
            }
        }
        else
        {
            EndGame(); // Ensure game over message is shown after max presses
        }
    }

    void EndGame()
    {
        if (!gameOver) // Ensure EndGame is only executed once
        {
            isMoving = false;
            gameOver = true; // Set game over flag

            // Ensure hammer is active and show Static animation
            if (hammerPrefab != null && hammerAnimator != null)
            {
                hammerPrefab.SetActive(true); // Keep hammer active
                SetHammerAnimation("Static"); // Play Static animation
            }

            Debug.Log("Game over.");
        }
    }

    void SetHammerAnimation(string animationName)
    {
        if (hammerPrefab != null && hammerAnimator != null)
        {
            hammerAnimator.Play(animationName);
        }
    }

    string DetermineSliderColor()
    {
        Vector3 handlePosition = slider.handleRect.position;

        if (RectTransformUtility.RectangleContainsScreenPoint(leftRed, handlePosition))
            return "Red";
        else if (RectTransformUtility.RectangleContainsScreenPoint(leftYellow, handlePosition))
            return "Yellow";
        else if (RectTransformUtility.RectangleContainsScreenPoint(green, handlePosition))
            return "Green";
        else if (RectTransformUtility.RectangleContainsScreenPoint(rightYellow, handlePosition))
            return "Yellow";
        else if (RectTransformUtility.RectangleContainsScreenPoint(rightRed, handlePosition))
            return "Red";
        else
            return "Unknown";
    }
}
