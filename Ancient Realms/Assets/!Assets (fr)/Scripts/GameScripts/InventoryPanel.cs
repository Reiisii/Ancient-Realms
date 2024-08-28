using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ESDatabase.Classes;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryPanel : MonoBehaviour
{
    private static PlayerStats playerStats;
    [Header("Details")]
    [SerializeField] TextMeshProUGUI playerName;
    private void Start(){
        playerStats = PlayerStats.GetInstance();
    }
    private void OnEnable(){
        playerName.SetText(playerStats.localPlayerData.gameData.playerName);
    }
    
}
