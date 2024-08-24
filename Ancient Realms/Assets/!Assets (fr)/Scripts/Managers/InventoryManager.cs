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
    [SerializeField] GameObject inventoryPanel;
    [Header("Input System")]
    public InputActionAsset inputActions;
    private InputActionMap playerActionMap;
    private InputActionMap uiActionMap;
    private void Awake(){
        if(Instance != null){
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        Instance = this;
        playerActionMap = inputActions.FindActionMap("Player");
        uiActionMap = inputActions.FindActionMap("Inventory");
    }
    public static InventoryManager GetInstance(){
        return Instance;
    }
    public void OpenInventory(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if(inventoryPanel.activeSelf == true){
                Time.timeScale = 1f;
                playerActionMap.Enable();
                uiActionMap.Disable();
                inventoryPanel.SetActive(false);
            }else{  
                Time.timeScale = 0f;
                playerActionMap.Disable();
                uiActionMap.Enable();
                inventoryPanel.SetActive(true);
            }
            
        }
    }
}
