using System.Collections;
using System.Collections.Generic;
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
                gameData.equippedData.helmSlot = invToHelm;
                inventoryItems.RemoveAt(equipment.dbIndex);  // Remove from inventory
                inventoryPanel.equipments.Remove(equipment); // Remove from equipments list
                break;

            case ArmorType.Chest:
                ItemData invToChest = inventoryItems[equipment.dbIndex];
                gameData.equippedData.chestSlot = invToChest;
                inventoryItems.RemoveAt(equipment.dbIndex);  // Remove from inventory
                inventoryPanel.equipments.Remove(equipment); // Remove from equipments list
                break;

            case ArmorType.Waist:
                ItemData invToWaist = inventoryItems[equipment.dbIndex];
                gameData.equippedData.waistSlot = invToWaist;
                inventoryItems.RemoveAt(equipment.dbIndex);  // Remove from inventory
                inventoryPanel.equipments.Remove(equipment); // Remove from equipments list
                break;

            case ArmorType.Foot:
                ItemData invToFoot = inventoryItems[equipment.dbIndex];
                gameData.equippedData.footSlot = invToFoot;
                inventoryItems.RemoveAt(equipment.dbIndex);  // Remove from inventory
                inventoryPanel.equipments.Remove(equipment); // Remove from equipments list
                break;
        }

        // Refresh and reinitialize inventory to reflect the changes
        inventoryPanel.RefreshInventory();
        inventoryPanel.LoadPlayerData(PlayerStats.GetInstance());

        playerStats.isDataDirty = true;  // Mark data as dirty to ensure changes are saved
    }
    }
    public void SetData(EquipmentSO equipmentData, GameObject inventoryGO)
    {
        equipment = equipmentData;
        inventoryPanel = inventoryGO.GetComponent<InventoryPanel>();
        InitializeEquipment();
    }
}
