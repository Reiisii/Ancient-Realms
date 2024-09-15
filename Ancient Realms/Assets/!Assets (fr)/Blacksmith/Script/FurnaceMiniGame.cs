using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FurnaceMiniGame : MonoBehaviour
{
    [Header("Slider Settings")]
    public Slider slider;
    public float fillSpeed = 1.0f;
    private bool isFilling = false;

    [Header("Game Settings")]
    public int currentRound = 1;
    public int maxRounds = 5;
    private bool gameOver = false;

    [Header("Green Score Areas")]
    public GameObject[] greenAreas;  // Array of green GameObjects

    [Header("Score Colors")]
    public Color greenColor = Color.green;  // Define greenColor
    public Color xColor = Color.red;  // For overfill or not green score "X"

    [Header("UI Settings")]
    public Image[] scoreCircles;

    [Header("Round Settings")]
    public float delayBeforeNextRound = 3f;  // Public delay variable

    private int lastGreenIndex = -1; // Track the last shown green area index
    private bool isOverfilled = false; // Flag to track overfill status

    void Start()
    {
        InitializeSlider();
        UpdateScoreCircles();
        SetActiveGreenArea();
    }

    void Update()
    {
        if (!gameOver && !isOverfilled) // Reject input if overfilled
        {
            HandleSpaceBarInput();
        }
    }

    void InitializeSlider()
    {
        slider.minValue = 0;
        slider.maxValue = 1;
        slider.value = 0;
    }

    void HandleSpaceBarInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!isFilling)
            {
                isFilling = true;
            }
            FillSlider();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            if (isFilling)
            {
                isFilling = false;
                if (!gameOver && !isOverfilled) // Ensure coroutine is not started if the game is over or already overfilled
                {
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
            if (!gameOver && !isOverfilled) // Ensure coroutine is not started if the game is over or already overfilled
            {
                isOverfilled = true; // Set overfill flag
                StartCoroutine(EndRoundWithDelay(true));  // End round if overfilled
            }
        }
    }

    IEnumerator EndRoundWithDelay(bool overfilled = false)
    {
        if (overfilled)
        {
            Debug.Log("Round Overfilled!");
            UpdateScoreCircle(currentRound - 1, xColor);  // Mark round as overfilled with "X"
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
            Debug.Log("Advancing to Round: " + currentRound);
            SetActiveGreenArea();
            ResetSlider();
        }
        else
        {
            gameOver = true;
            Debug.Log("Game Over! Total Rounds Played: " + currentRound);
        }

        // Reset overfill flag after transitioning to the next round
        isOverfilled = false;
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
            Debug.Log("Landed in Green");
            UpdateScoreCircle(currentRound - 1, greenColor);
        }
        else
        {
            Debug.Log("Did Not Land in Green");
            UpdateScoreCircle(currentRound - 1, xColor);  // Mark round as not green with "X"
        }
    }

    void UpdateScoreCircle(int roundIndex, Color scoreColor)
    {
        if (roundIndex >= 0 && roundIndex < scoreCircles.Length)
        {
            scoreCircles[roundIndex].color = scoreColor;
        }
    }

    void UpdateScoreCircles()
    {
        for (int i = 0; i < scoreCircles.Length; i++)
        {
            scoreCircles[i].color = Color.clear;  // Reset colors
        }
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
