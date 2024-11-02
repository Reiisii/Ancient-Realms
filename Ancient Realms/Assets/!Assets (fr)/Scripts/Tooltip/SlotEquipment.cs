using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ESDatabase.Classes;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotEquipment : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private int slotNo;
    [SerializeField] private ItemData equipment;
    [SerializeField] private bool isHovering = false;
    public void Start(){
        UpdateEquipment();
    }
    public void UpdateEquipment()
    {
        
        switch(slotNo){
            case 0:
                equipment = PlayerStats.GetInstance().localPlayerData.gameData.equippedData.helmSlot;
            break;
            case 1:
                equipment = PlayerStats.GetInstance().localPlayerData.gameData.equippedData.chestSlot;
            break;
            case 2:
                equipment = PlayerStats.GetInstance().localPlayerData.gameData.equippedData.waistSlot;
            break;
            case 3:
                equipment = PlayerStats.GetInstance().localPlayerData.gameData.equippedData.footSlot;
            break;
            case 4:
                equipment = PlayerStats.GetInstance().localPlayerData.gameData.equippedData.mainSlot;
            break;
            case 5:
                equipment = PlayerStats.GetInstance().localPlayerData.gameData.equippedData.shieldSlot;
            break;
            case 6:
                equipment = PlayerStats.GetInstance().localPlayerData.gameData.equippedData.javelinSlot;
            break;
            case 7:
                equipment = PlayerStats.GetInstance().localPlayerData.gameData.equippedData.bandageSlot;
            break;
        }
        if (equipment == null && isHovering) {
            isHovering = false;
            TooltipManager.GetInstance().HideEquipmentTooltip();
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isHovering)
        {
            if(equipment == null) return;
            isHovering = true;
            EquipmentSO equipmentSO = AccountManager.Instance.equipments.FirstOrDefault(eq => eq.equipmentId.Equals(equipment.equipmentId)).CreateCopy(equipment);
            TooltipManager.GetInstance().ShowEquipmentTooltip(equipmentSO);
        }

    }
    public void OnPointerClick(PointerEventData eventData){
        EquipmentSO equipmentSO = AccountManager.Instance.equipments.FirstOrDefault(eq => eq.equipmentId.Equals(equipment.equipmentId)).CreateCopy(equipment);
        if (equipmentSO == null && isHovering) {
            isHovering = false;
            TooltipManager.GetInstance().HideEquipmentTooltip();
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isHovering)
        {
            isHovering = false;
            TooltipManager.GetInstance().HideEquipmentTooltip();
        }

    }
    private void OnDisable(){
        if (isHovering)
        {   
            isHovering = false;
            TooltipManager.GetInstance().HideEquipmentTooltip();
        }
    }
    private void OnDestroy(){
        if (isHovering)
        {
            isHovering = false;
            TooltipManager.GetInstance().HideEquipmentTooltip();
        }
    }
}
