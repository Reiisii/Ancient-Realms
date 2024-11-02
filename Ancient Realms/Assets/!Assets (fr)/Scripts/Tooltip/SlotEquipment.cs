using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ESDatabase.Classes;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotEquipment : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private int slotNo;
    [SerializeField] private EquipmentSO equipment;
    [SerializeField] private ItemData itemData;
    [SerializeField] private bool isHovering = false;
    public void Update(){
        UpdateEquipment();
    }
    public void UpdateEquipment()
    {
        switch(slotNo){
            case 0:
                itemData = PlayerStats.GetInstance().localPlayerData.gameData.equippedData.helmSlot;
            break;
            case 1:
                itemData = PlayerStats.GetInstance().localPlayerData.gameData.equippedData.chestSlot;
            break;
            case 2:
                itemData = PlayerStats.GetInstance().localPlayerData.gameData.equippedData.waistSlot;
            break;
            case 3:
                itemData = PlayerStats.GetInstance().localPlayerData.gameData.equippedData.footSlot;
            break;
            case 4:
                itemData = PlayerStats.GetInstance().localPlayerData.gameData.equippedData.mainSlot;
            break;
            case 5:
                itemData = PlayerStats.GetInstance().localPlayerData.gameData.equippedData.shieldSlot;
            break;
            case 6:
                itemData = PlayerStats.GetInstance().localPlayerData.gameData.equippedData.javelinSlot;
            break;
            case 7:
                itemData = PlayerStats.GetInstance().localPlayerData.gameData.equippedData.bandageSlot;
            break;
        }
        if(itemData != null){
            equipment = AccountManager.Instance.equipments.FirstOrDefault(eq => eq.equipmentId.Equals(itemData.equipmentId)).CreateCopy(itemData);
        }else{
            equipment = null;
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
            if(itemData == null) return;
            isHovering = true;
            TooltipManager.GetInstance().ShowEquipmentTooltip(equipment);
        }

    }
    public void OnPointerClick(PointerEventData eventData){
        if (itemData == null && isHovering) {
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
