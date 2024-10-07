using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager instance;

    private bool isSwordBladePlaced = false;
    private bool isSwordRainGuardPlaced = false;
    private bool isSwordGripPlaced = false;
    private bool isSwordPommelPlaced = false;

    private bool isPila1Placed = false;
    private bool isPila2Placed = false;
    private bool isPila3Placed = false;
    private bool isPila4Placed = false;

    private bool isPugioBladePlaced = false;
    private bool isPugioRainGuardPlaced = false;
    private bool isPugioGripPlaced = false;
    private bool isPugioPommelPlaced = false;

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
            // Sword parts
            case "SwordBlade":
                isSwordBladePlaced = true;
                break;
            case "SwordRainGuard":
                isSwordRainGuardPlaced = true;
                break;
            case "SwordGrip":
                isSwordGripPlaced = true;
                break;
            case "SwordPommel":
                isSwordPommelPlaced = true;
                break;
            
            // Pila parts
            case "1":
                isPila1Placed = true;
                break;
            case "2":
                isPila2Placed = true;
                break;
            case "3":
                isPila3Placed = true;
                break;
            case "4":
                isPila4Placed = true;
                break;
            
            // Pugio parts
            case "PugioBlade":
                isPugioBladePlaced = true;
                break;
            case "PugioRainGuard":
                isPugioRainGuardPlaced = true;
                break;
            case "PugioGrip":
                isPugioGripPlaced = true;
                break;
            case "PugioPommel":
                isPugioPommelPlaced = true;
                break;
        }

        CheckIfPuzzleComplete();
    }

    private void CheckIfPuzzleComplete()
    {
        if (isSwordBladePlaced && isSwordRainGuardPlaced && isSwordGripPlaced && isSwordPommelPlaced)
        {
            SmithingGameManager.GetInstance().score += 25;
            SmithingGameManager.GetInstance().assemblyUsed = true;
            SmithingGameManager.GetInstance().EndWorkStation(WorkStation.Assembly);
        }

        if (isPila1Placed && isPila2Placed && isPila3Placed && isPila4Placed)
        {
            SmithingGameManager.GetInstance().score += 25;
            SmithingGameManager.GetInstance().assemblyUsed = true;
            SmithingGameManager.GetInstance().EndWorkStation(WorkStation.Assembly);
        }

        if (isPugioBladePlaced && isPugioRainGuardPlaced && isPugioGripPlaced && isPugioPommelPlaced)
        {
            SmithingGameManager.GetInstance().score += 25;
            SmithingGameManager.GetInstance().assemblyUsed = true;
            SmithingGameManager.GetInstance().EndWorkStation(WorkStation.Assembly);
        }
    }
}
