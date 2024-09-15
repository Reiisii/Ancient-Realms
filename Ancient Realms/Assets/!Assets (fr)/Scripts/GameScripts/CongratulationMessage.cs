using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CongratulationMessage : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    private void OnEnable(){
        nameText.SetText(PlayerStats.GetInstance().localPlayerData.gameData.playerName + ",");
        PlayerController.GetInstance().playerActionMap.Disable();        
    }
    public async void BackToMenu(){
        await PlayerUIManager.GetInstance().ClosePlayerUI();
        await PlayerUIManager.GetInstance().OpenDarkenUI();
        Play.GetInstance().PlayMainMenu();
    }
    public void Chapter1(){
        Play.GetInstance().Chapter1();
    }
}
