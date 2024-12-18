using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ESDatabase.Classes;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager Instance;
    private static PlayerStats playerStats;
    [Header("Game Object")]
    [SerializeField] public GameObject inventoryPanel;
    [SerializeField] public InventoryPanel invPanel;
    private void Awake(){
        if(Instance != null){
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        Instance = this;
    }
    public static InventoryManager GetInstance(){
        return Instance;
    }
    public void OpenInventory()
    {
        if(PlayerController.GetInstance() != null){
            if(!PlayerController.GetInstance().canAccessInventory) {
                    PlayerUIManager.GetInstance().SpawnMessage(MType.Error, "You can't access inventory in a combat location.");
                    return;
            }
            if(inventoryPanel.activeSelf == true){
                Time.timeScale = 1f;
                PlayerController.GetInstance().playerActionMap.Enable();
                PlayerController.GetInstance().inventoryActionMap.Disable();
                inventoryPanel.SetActive(false);
            }else{  
                Time.timeScale = 0f;
                QuestManager.GetInstance().UpdateBackpackGoal();
                PlayerController.GetInstance().playerActionMap.Disable();
                PlayerController.GetInstance().inventoryActionMap.Enable();
                inventoryPanel.SetActive(true);
            }
        }
    }
}
