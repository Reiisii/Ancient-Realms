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
        Time.timeScale = 0f;
        PlayerController.GetInstance().playerActionMap.Disable();        
    }
    public void BackToMenu(){
        Time.timeScale = 1f;
        Play.GetInstance().PlayMainMenu();
    }
    public void Chapter1(){
        Time.timeScale = 1f;
        Play.GetInstance().Chapter1();
    }
}
