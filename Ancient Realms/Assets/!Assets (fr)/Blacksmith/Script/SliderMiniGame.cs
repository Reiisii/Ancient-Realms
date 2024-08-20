using UnityEngine;
using UnityEngine.UI;

public class SliderMiniGame : MonoBehaviour
{
    public Slider slider;
    public float speed = 1.0f;
    private bool isMoving = true;
    private bool isMovingRight = true;

    private float speedIncrement = 0.5f;
    private int pressCount = 0;
    private int maxPresses = 5;

    void Start()
    {
        InitializeSlider();
    }

    void Update()
    {
        // Only move the slider if it's supposed to be moving and we haven't exceeded max tries
        if (isMoving && pressCount < maxPresses)
        {
            MoveSlider();
        }

        if (Input.GetKeyDown(KeyCode.Space) && pressCount < maxPresses)
        {
            if (isMoving)
            {
                StopSlider();
            }
            else
            {
                StartSlider();
                IncreaseSpeed();
                pressCount++;

                if (pressCount >= maxPresses)
                {
                    // After the last press, stop the game
                    isMoving = false;
                    Debug.Log("Max tries reached. Game over.");
                }
            }
        }
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
        Debug.Log("Stopped at value: " + slider.value);
    }

    void StartSlider()
    {
        isMoving = true;
    }

    void InitializeSlider()
    {
        slider.minValue = 0;
        slider.maxValue = 1;
        slider.value = 0.5f; // Start in the middle or any initial value
    }

    void IncreaseSpeed()
    {
        speed += speedIncrement;
    }
}
