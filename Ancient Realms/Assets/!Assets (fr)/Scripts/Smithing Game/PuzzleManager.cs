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

    [SerializeField] private bool isPugioBladePlaced = false;
    [SerializeField] private bool isPugioRainGuardPlaced = false;
    [SerializeField] private bool isPugioGripPlaced = false;
    [SerializeField] private bool isPugioPommelPlaced = false;

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
    public void PiecePlaced(PieceType piece)
    {
        switch (piece)
        {
            case PieceType.G_BLADE:
                isSwordBladePlaced = true;
                break;
            case PieceType.G_RAINGUARD:
                isSwordRainGuardPlaced = true;
                break;
            case PieceType.G_GRIP:
                isSwordGripPlaced = true;
                break;
            case PieceType.G_POMMEL:
                isSwordPommelPlaced = true;
                break;
            case PieceType.P_TIP:
                isPila1Placed = true;
                break;
            case PieceType.P_TIPSHAFT:
                isPila2Placed = true;
                break;
            case PieceType.P_SHAFT:
                isPila3Placed = true;
                break;
            case PieceType.P_Pommel:
                isPila4Placed = true;
                break;
            case PieceType.PG_BLADE:
                isPugioBladePlaced = true;
                Debug.Log("Placed");
                break;
            case PieceType.PG_RAINGUARD:
                isPugioRainGuardPlaced = true;
                break;
            case PieceType.PG_GRIP:
                isPugioGripPlaced = true;
                break;
            case PieceType.PG_POMMEL:
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
        switch(SmithingGameManager.GetInstance().order){
            case OrderType.Gladius:
                ResetPosition(sword);
            break;
            case OrderType.Pila:
                ResetPosition(pila);
            break;
            case OrderType.Pugio:
                ResetPosition(pugio);
            break;
        }
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
