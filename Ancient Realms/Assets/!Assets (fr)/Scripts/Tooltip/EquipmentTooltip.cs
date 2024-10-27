using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public EquipmentSO equipment;
    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.GetInstance().ShowEquipmentTooltip(gameObject.GetComponent<EquipmentPrefab>().equipment);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.GetInstance().HideEquipmentTooltip();
    }
    private void OnDestroy(){
        if(TooltipManager.GetInstance().equipmentTooltip.activeSelf) TooltipManager.GetInstance().HideEquipmentTooltip();
    }
    private void OnDisable(){
        if(TooltipManager.GetInstance().equipmentTooltip.activeSelf) TooltipManager.GetInstance().HideEquipmentTooltip();
    }
}
