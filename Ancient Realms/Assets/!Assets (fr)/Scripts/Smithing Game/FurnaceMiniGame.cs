using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FurnaceMiniGame : MonoBehaviour
{
    [Header("Tool Tip")]
    [SerializeField] GameObject tooltip;
    [Header("Slider Settings")]
    public Slider slider;
    public float fillSpeed = 1.0f;
    private bool isFilling = false;
    private bool isProcessingRound = false; // Flag to handle input rejection

    [Header("Game Settings")]
    public int currentRound = 1;
    public int maxRounds = 5;
    private bool gameOver = false;

    [Header("Green Score Areas")]
    public GameObject[] greenAreas;  // Array of green GameObjects

    [Header("Score Colors")]
    public Color greenColor = Color.green;  // Define greenColor
    public Color xColor = Color.red;  // For overfill or not green score "X"
    public Text startPromptText;
    [Header("Scoreboard")]
    public WorkstationScore scoreBoard;

    [Header("Round Settings")]
    public float delayBeforeNextRound = 3f;  // Public delay variable

    private int lastGreenIndex = -1; // Track the last shown green area index

    void OnEnable()
    {
        DisplayStartPrompt(true);
        if(PlayerStats.GetInstance().localPlayerData.gameData.uiSettings.Contains("furnace")){
            tooltip.SetActive(false);
        }else{
            tooltip.SetActive(true);
        }
        InitializeSlider();
        SetActiveGreenArea();
    }
    void OnDisable(){
        currentRound = 1;
        gameOver = false;
        DisplayStartPrompt(false);
    }
    void Update()
    {
        if (!gameOver && !isProcessingRound) // Reject input if processing a round
        {
            HandleSpaceBarInput();
        }
    }

    void InitializeSlider()
    {
        slider.minValue = 0;
        slider.maxValue = 1;
        slider.value = 0;
        isFilling = false;
    }

    void HandleSpaceBarInput()
    {
        DisplayStartPrompt(false);
        if (Input.GetKey(KeyCode.Space))
        {
            if (!isFilling)
            {
                tooltip.SetActive(false);
                isFilling = true;
            }
            FillSlider();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            if (isFilling)
            {
                isFilling = false;
                if (!gameOver && !isProcessingRound) // Ensure coroutine is not started if the game is over or processing a round
                {
                    isProcessingRound = true; // Set processing flag
                    StartCoroutine(EndRoundWithDelay());
                }
            }
        }
    }

    void FillSlider()
    {
        if (slider.value < slider.maxValue)
        {
            slider.value += fillSpeed * Time.deltaTime;
        }
        else
        {
            if (!gameOver && !isProcessingRound) // Ensure coroutine is not started if the game is over or processing a round
            {
                isProcessingRound = true; // Set processing flag
                StartCoroutine(EndRoundWithDelay(true));  // End round if overfilled
            }
        }
    }

    IEnumerator EndRoundWithDelay(bool overfilled = false)
    {
        if (overfilled)
        {
            scoreBoard.UpdateScoreCircle(currentRound - 1, xColor);  // Mark round as overfilled with "X"
        }
        else
        {
            CalculateScore();
        }

        // Wait for the specified delay before moving to the next round
        yield return new WaitForSeconds(delayBeforeNextRound);

        // Move to the next round or end the game if maximum rounds are reached
        if (currentRound < maxRounds)
        {
            currentRound++;
            SetActiveGreenArea();
            ResetSlider();
        }
        else
        {
            gameOver = true;
            SmithingGameManager.GetInstance().furnaceUsed = true;
            SmithingGameManager.GetInstance().EndWorkStation(WorkStation.Furnace);
            scoreBoard.gameObject.SetActive(false);
        }

        // Reset processing flag after transitioning to the next round
        isProcessingRound = false;
    }

    void ResetSlider()
    {
        slider.value = 0;
        isFilling = false;
    }

    void CalculateScore()
    {
        Vector3 handlePosition = slider.handleRect.position;

        if (RectTransformUtility.RectangleContainsScreenPoint(greenAreas[lastGreenIndex].GetComponent<RectTransform>(), handlePosition))
        {
            SmithingGameManager.GetInstance().score += 5;
            scoreBoard.UpdateScoreCircle(currentRound - 1, greenColor);
            AudioManager.GetInstance().PlayAudio(SoundType.GREEN);
        }
        else
        {
            scoreBoard.UpdateScoreCircle(currentRound - 1, xColor);
            AudioManager.GetInstance().PlayAudio(SoundType.RED);
        }
    }
    void DisplayStartPrompt(bool display)
    {
        startPromptText.gameObject.SetActive(display);
        if (display)
            startPromptText.text = "--- Hold Spacebar to increase the slider bar ---";
    }

    void SetActiveGreenArea()
    {
        // Hide all green areas
        foreach (GameObject greenArea in greenAreas)
        {
            greenArea.SetActive(false);
        }

        // Randomly select a new green area that is not the same as the last one shown
        int newIndex;
        do
        {
            newIndex = Random.Range(0, greenAreas.Length);
        } while (newIndex == lastGreenIndex);

        lastGreenIndex = newIndex;
        greenAreas[lastGreenIndex].SetActive(true);
    }
}
