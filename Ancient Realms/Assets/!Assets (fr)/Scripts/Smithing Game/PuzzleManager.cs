using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [Header("Tool Tip")]
    [SerializeField] GameObject tooltip;
    public static PuzzleManager instance;
    [Header("Dropzone")]
    [SerializeField] public GameObject swordDz;
    [SerializeField] public GameObject pilaDz;
    [SerializeField] public GameObject pugioDz;
    
    [Header("Panel")]
    [SerializeField] public GameObject sword;
    [SerializeField] public GameObject pila;
    [SerializeField] public GameObject pugio;
    
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
    private void OnEnable(){
        if(PlayerStats.GetInstance().localPlayerData.gameData.uiSettings.Contains("assembly")){
            tooltip.SetActive(false);
        }else{
            tooltip.SetActive(true);
        }
        switch(SmithingGameManager.GetInstance().order){
            case OrderType.Gladius:
                sword.SetActive(true);
                swordDz.SetActive(true);
            break;
            case OrderType.Pila:
                pila.SetActive(true);
                pilaDz.SetActive(true);
            break;
            case OrderType.Pugio:
                pugio.SetActive(true);
                pugioDz.SetActive(true);
            break;
        }
    }

    private void OnDisable(){
        Clear();
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
    public void Clear(){
        isSwordBladePlaced = false;
        isSwordRainGuardPlaced = false;
        isSwordGripPlaced = false;
        isSwordPommelPlaced = false;
        isPila1Placed = false;
        isPila2Placed = false;
        isPila3Placed = false;
        isPila4Placed = false;
        isPugioBladePlaced = false;
        isPugioRainGuardPlaced = false;
        isPugioGripPlaced = false;
        isPugioPommelPlaced = false;
        sword.SetActive(false);
        swordDz.SetActive(false);
        pila.SetActive(false);
        pilaDz.SetActive(false);
        pugio.SetActive(false);
        pugioDz.SetActive(false);
        ResetPosition(sword);
        ResetPosition(pila);
        ResetPosition(pugio);
    }
    public void ResetPosition(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            DragDrop dragDropComponent = child.GetComponent<DragDrop>();
            if (dragDropComponent != null)
            {
                dragDropComponent.ResetPostion();
            }
        }
    }
}
