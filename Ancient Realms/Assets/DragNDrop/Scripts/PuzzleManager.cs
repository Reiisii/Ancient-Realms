using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager instance;

    private bool isBladePlaced = false;
    private bool isRainGuardPlaced = false;
    private bool isGripPlaced = false;
    private bool isPommelPlaced = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PiecePlaced(string pieceName)
    {
        switch (pieceName)
        {
            case "SwordBlade":
                isBladePlaced = true;
                break;
            case "SwordRainGuard":
                isRainGuardPlaced = true;
                break;
            case "SwordGrip":
                isGripPlaced = true;
                break;
            case "SwordPommel":
                isPommelPlaced = true;
                break;
        }

        CheckIfPuzzleComplete();
    }

    private void CheckIfPuzzleComplete()
    {
        if (isBladePlaced && isRainGuardPlaced && isGripPlaced && isPommelPlaced)
        {
            Debug.Log("The sword is built!");
            // You can also use Unity's UI system to display a message to the player
        }
    }
}