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
    [Header("Details")]
    [SerializeField] TextMeshProUGUI playerName;
    private void OnEnable(){
        playerName.SetText(PlayerStats.GetInstance().localPlayerData.gameData.playerName);
    }
    
}
