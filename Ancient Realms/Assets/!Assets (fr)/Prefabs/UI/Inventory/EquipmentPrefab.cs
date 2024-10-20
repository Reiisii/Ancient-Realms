using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ESDatabase.Classes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentPrefab : MonoBehaviour
{
    [Header("Equipment Data")]
    [SerializeField] Image equipmentIcon;
    [SerializeField] TextMeshProUGUI level;
    [SerializeField] GameObject levelGO;
    [Header("Equipment Tier")]
    [SerializeField] GameObject tier0;
    [SerializeField] GameObject tier1;
    [SerializeField] GameObject tier2;
    [SerializeField] GameObject tier3;
    [SerializeField] GameObject tier4;
    [SerializeField] GameObject tier5;
    public InventoryPanel inventoryPanel;
    public EquipmentSO equipment;
    public void InitializeEquipment()
    {
        if (equipment != null)
        {
            
            equipmentIcon.sprite = equipment.image;
            
            level.SetText(equipment.level.ToString());
            levelGO.SetActive(true);
            switch(equipment.tier){
                case 0:
                    tier0.SetActive(true);
                break;
                case 1:
                    tier1.SetActive(true);
                break;
                case 2:
                    tier2.SetActive(true);
                break;
                case 3:
                    tier3.SetActive(true);
                break;
                case 4:
                    tier4.SetActive(true);
                break;
                case 5:
                    tier5.SetActive(true);
                break;
            }
        }
        else
        {
            Debug.LogError("Equipment is null, cannot display equipment data.");
        }
    }
    public void EquipArmor(){
        if(equipment.equipmentType == EquipmentEnum.Armor)
        {
        PlayerStats playerStats = PlayerStats.GetInstance();
        GameData gameData = playerStats.localPlayerData.gameData;
        List<ItemData> inventoryItems = gameData.inventory.items;

        switch(equipment.armorType)
        {
            case ArmorType.Helmet:
                ItemData invToHelm = inventoryItems[equipment.dbIndex];
                ItemData helmToInv = gameData.equippedData.helmSlot;
                
                if(helmToInv != null){
                    EquipmentSO copiedEquipment = AccountManager.Instance.equipments.FirstOrDefault(eq => eq.equipmentId == helmToInv.equipmentId).CreateCopy(helmToInv);
                    inventoryItems.Add(helmToInv);
                    inventoryPanel.equipments.Add(copiedEquipment);
                }
                gameData.equippedData.helmSlot = invToHelm;
                inventoryItems.RemoveAt(equipment.dbIndex);  // Remove from inventory
                inventoryPanel.equipments.Remove(equipment); // Remove from equipments list
                AudioManager.GetInstance().PlayAudio(SoundType.HELMET, 0.5f);
                break;

            case ArmorType.Chest:
                ItemData invToChest = inventoryItems[equipment.dbIndex];
                ItemData chestToInv = gameData.equippedData.chestSlot;
                if(chestToInv != null){
                    EquipmentSO copiedEquipment = AccountManager.Instance.equipments.FirstOrDefault(eq => eq.equipmentId == chestToInv.equipmentId).CreateCopy(chestToInv);
                    inventoryItems.Add(chestToInv);
                    inventoryPanel.equipments.Add(copiedEquipment);
                }
                gameData.equippedData.chestSlot = invToChest;
                inventoryItems.RemoveAt(equipment.dbIndex);  // Remove from inventory
                inventoryPanel.equipments.Remove(equipment); // Remove from equipments list
                AudioManager.GetInstance().PlayAudio(SoundType.CHEST, 0.7f);
                break;

            case ArmorType.Waist:
                ItemData invToWaist = inventoryItems[equipment.dbIndex];
                ItemData waistToInv = gameData.equippedData.waistSlot;
                if(waistToInv != null){
                    EquipmentSO copiedEquipment = AccountManager.Instance.equipments.FirstOrDefault(eq => eq.equipmentId == waistToInv.equipmentId).CreateCopy(waistToInv);
                    inventoryItems.Add(waistToInv);
                    inventoryPanel.equipments.Add(copiedEquipment);
                }
                gameData.equippedData.waistSlot = invToWaist;
                inventoryItems.RemoveAt(equipment.dbIndex);  // Remove from inventory
                inventoryPanel.equipments.Remove(equipment); // Remove from equipments list
                AudioManager.GetInstance().PlayAudio(SoundType.WAIST, 1f);
                break;

            case ArmorType.Foot:
                ItemData invToFoot = inventoryItems[equipment.dbIndex];
                ItemData footToInv = gameData.equippedData.footSlot;
                if(footToInv != null){
                    EquipmentSO copiedEquipment = AccountManager.Instance.equipments.FirstOrDefault(eq => eq.equipmentId == footToInv.equipmentId).CreateCopy(footToInv);
                    inventoryItems.Add(footToInv);
                    inventoryPanel.equipments.Add(copiedEquipment);
                }
                gameData.equippedData.footSlot = invToFoot;
                inventoryItems.RemoveAt(equipment.dbIndex);  // Remove from inventory
                inventoryPanel.equipments.Remove(equipment); // Remove from equipments list
                AudioManager.GetInstance().PlayAudio(SoundType.FOOT, 1f);
                break;
        }

        // Refresh and reinitialize inventory to reflect the changes
        PlayerStats.GetInstance().InitializeEquipments();
        inventoryPanel.InitializeInventory();

        playerStats.isDataDirty = true;  // Mark data as dirty to ensure changes are saved
        }else if(equipment.equipmentType == EquipmentEnum.Shield){
            PlayerStats playerStats = PlayerStats.GetInstance();
            GameData gameData = playerStats.localPlayerData.gameData;
            List<ItemData> inventoryItems = gameData.inventory.items;

            ItemData invToShield = inventoryItems[equipment.dbIndex];
            ItemData shieldToInv = gameData.equippedData.shieldSlot;
                    
            if(shieldToInv != null){
                EquipmentSO copiedEquipment = AccountManager.Instance.equipments.FirstOrDefault(eq => eq.equipmentId == shieldToInv.equipmentId).CreateCopy(shieldToInv);
                inventoryItems.Add(shieldToInv);
                inventoryPanel.equipments.Add(copiedEquipment);
            }
            gameData.equippedData.shieldSlot = invToShield;
            inventoryItems.RemoveAt(equipment.dbIndex);  // Remove from inventory
            inventoryPanel.equipments.Remove(equipment); // Remove from equipments list

            // Refresh and reinitialize inventory to reflect the changes
            PlayerStats.GetInstance().InitializeEquipments();
            inventoryPanel.InitializeInventory();

            playerStats.isDataDirty = true; 
        }else if(equipment.equipmentType == EquipmentEnum.Weapon){
            PlayerStats playerStats = PlayerStats.GetInstance();
            GameData gameData = playerStats.localPlayerData.gameData;
            List<ItemData> inventoryItems = gameData.inventory.items;

            switch(equipment.weaponType)
            {
                case WeaponType.Sword:
                    ItemData invToSword = inventoryItems[equipment.dbIndex];
                    ItemData swordToInv = gameData.equippedData.mainSlot;
                    
                    if(swordToInv != null){
                        EquipmentSO copiedEquipment = AccountManager.Instance.equipments.FirstOrDefault(eq => eq.equipmentId == swordToInv.equipmentId).CreateCopy(swordToInv);
                        inventoryItems.Add(swordToInv);
                        inventoryPanel.equipments.Add(copiedEquipment);
                    }
                    gameData.equippedData.mainSlot = invToSword;
                    inventoryItems.RemoveAt(equipment.dbIndex);  // Remove from inventory
                    inventoryPanel.equipments.Remove(equipment); // Remove from equipments list
                break;
                case WeaponType.Spear:
                    ItemData invToSpear = inventoryItems[equipment.dbIndex];
                    ItemData spearToInv = gameData.equippedData.mainSlot;
                    
                    if(spearToInv != null){
                        EquipmentSO copiedEquipment = AccountManager.Instance.equipments.FirstOrDefault(eq => eq.equipmentId == spearToInv.equipmentId).CreateCopy(spearToInv);
                        inventoryItems.Add(spearToInv);
                        inventoryPanel.equipments.Add(copiedEquipment);
                    }
                    gameData.equippedData.mainSlot = invToSpear;
                    inventoryItems.RemoveAt(equipment.dbIndex);  // Remove from inventory
                    inventoryPanel.equipments.Remove(equipment); // Remove from equipments list
                break;
                case WeaponType.Javelin:
                    ItemData invToJavelin = inventoryItems[equipment.dbIndex];
                    ItemData javelinToInv = gameData.equippedData.javelinSlot;
                    
                    if(javelinToInv != null){
                        EquipmentSO copiedEquipment = AccountManager.Instance.equipments.FirstOrDefault(eq => eq.equipmentId == javelinToInv.equipmentId).CreateCopy(javelinToInv);
                        inventoryItems.Add(javelinToInv);
                        inventoryPanel.equipments.Add(copiedEquipment);
                    }
                    gameData.equippedData.javelinSlot = invToJavelin;
                    inventoryItems.RemoveAt(equipment.dbIndex);  // Remove from inventory
                    inventoryPanel.equipments.Remove(equipment); // Remove from equipments list
                break;
                case WeaponType.SpearJavelin:
                    ItemData invToSpearJavelin = inventoryItems[equipment.dbIndex];
                    ItemData spearJavelinToInv = gameData.equippedData.javelinSlot;
                    
                    if(spearJavelinToInv != null){
                        EquipmentSO copiedEquipment = AccountManager.Instance.equipments.FirstOrDefault(eq => eq.equipmentId == spearJavelinToInv.equipmentId).CreateCopy(spearJavelinToInv);
                        inventoryItems.Add(spearJavelinToInv);
                        inventoryPanel.equipments.Add(copiedEquipment);
                    }
                    gameData.equippedData.javelinSlot = invToSpearJavelin;
                    inventoryItems.RemoveAt(equipment.dbIndex);  // Remove from inventory
                    inventoryPanel.equipments.Remove(equipment); // Remove from equipments list
                break;
            }

            // Refresh and reinitialize inventory to reflect the changes
            PlayerStats.GetInstance().InitializeEquipments();
            inventoryPanel.InitializeInventory();

            playerStats.isDataDirty = true; 
            }
    }
    public void SetData(EquipmentSO equipmentData, GameObject inventoryGO)
    {
        equipment = equipmentData;
        inventoryPanel = inventoryGO.GetComponent<InventoryPanel>();
        InitializeEquipment();
    }
}
