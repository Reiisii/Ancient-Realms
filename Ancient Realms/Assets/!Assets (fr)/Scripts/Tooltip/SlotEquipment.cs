using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotEquipment : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private int slotNo;
    [SerializeField] private EquipmentSO equipment;
    [SerializeField] private bool isHovering = false;
    public void Start(){
        UpdateEquipment();
    }
    public void UpdateEquipment()
    {
        equipment = PlayerStats.GetInstance().equippedItems[slotNo];
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
            TooltipManager.GetInstance().ShowEquipmentTooltip(equipment);
        }

    }
    public void OnPointerClick(PointerEventData eventData){
        equipment = PlayerStats.GetInstance().equippedItems[slotNo];
        if (equipment == null && isHovering) {
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
    private void OnDestroy(){
        if (isHovering)
        {
            isHovering = false;
            TooltipManager.GetInstance().HideEquipmentTooltip();
        }
    }
}
