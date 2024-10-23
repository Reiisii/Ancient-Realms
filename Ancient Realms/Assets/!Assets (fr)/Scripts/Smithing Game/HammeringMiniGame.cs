using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HammeringMiniGame : MonoBehaviour
{
    [Header("Tool Tip")]
    [SerializeField] GameObject tooltip;
    [Header("Slider Settings")]
    public Slider slider;
    public float speed = 1.0f;
    private float speedIncrement = 0.2f;
    private bool isMoving = false;
    private bool isMovingRight = true;

    [Header("Game Settings")]
    public int maxPresses = 5;
    private int pressCount = 0;
    private int score = 0;

    [Header("UI Elements")]
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
    public WorkstationScore workstationScore;
    public Color redColor = Color.red;
    public Color yellowColor = Color.yellow;
    public Color greenColor = Color.green;

    [Header("Hammer Animation Settings")]
    public GameObject hammerPrefab;
    private Animator hammerAnimator;
    public GameObject[] items;
    public string hammerAnimationParam = "isHammering";

    [Header("Timer Radial Settings")]
    public Image timerCircleImage;
    [Header("Proceed")]
    [SerializeField] GameObject hammering;
    private bool gameOver = false;
    private bool gameStarted = false;
    private float timeLeft;
    private bool timeRunning = false;

    void OnEnable()
    {
        if(PlayerStats.GetInstance().localPlayerData.gameData.uiSettings.Contains("hammering")){
            tooltip.SetActive(false);
        }else{
            tooltip.SetActive(true);
        }
        switch(SmithingGameManager.GetInstance().order){
            case OrderType.Gladius:
                items[0].SetActive(true);
            break;
            case OrderType.Pila:
                items[1].SetActive(true);
            break;
            case OrderType.Pugio:
                items[2].SetActive(true);
            break;

        }
        InitializeSlider();
        InitializeTimer();
        DisplayStartPrompt(true);
        InitializeHammer();
    }
    void OnDisable(){
        foreach(GameObject go in items){
            go.SetActive(false);
        }
        gameOver = false;
        gameStarted = false;
        timeRunning = false;
        pressCount = 0;
        speed = 1;
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
        switch(color){
            case "Green":
                SmithingGameManager.GetInstance().score += 5;
                AudioManager.GetInstance().PlayAudio(SoundType.GREEN);
            break;
            case "Yellow":
                SmithingGameManager.GetInstance().score += 3;
                AudioManager.GetInstance().PlayAudio(SoundType.YELLOW);
            break;
            case "Red":
                SmithingGameManager.GetInstance().score += 0;
                AudioManager.GetInstance().PlayAudio(SoundType.RED);
            break;
        }
    }

    void InitializeSlider()
    {
        slider.minValue = 0;
        slider.maxValue = 1;
        slider.value = 0.5f;
    }

    void ResetSlider()
    {
        slider.value = Random.Range(0.0f, 1f);
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
    }

    void UpdateScoreCircle()
    {
        if (pressCount < workstationScore.circleColors.Length)
        {
            Color color = DetermineScoreCircleColor();
            workstationScore.UpdateScoreCircle(pressCount, color);
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
        yield return new WaitForSeconds(delay);

        if (pressCount < maxPresses)
        {
            ResetSlider();
            StartSlider();
            IncreaseSpeed();
            SetHammerAnimation("Static");
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
            workstationScore.gameObject.SetActive(false);
            SmithingGameManager.GetInstance().hammerUsed = true;
            SmithingGameManager.GetInstance().EndWorkStation(WorkStation.Hammering);
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
                tooltip.SetActive(false);
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
                    if(SmithingGameManager.GetInstance().order == OrderType.Pugio){
                        SetHammerAnimation(""); // Pugio animation name
                    }else{
                        SetHammerAnimation("Hammering");
                    }
                }
            }
        }
    }
}
