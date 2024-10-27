using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public EquipmentPrefab equipmentPrefab;
    [SerializeField] private bool isHovering = false;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isHovering)
        {
            isHovering = true;
            TooltipManager.GetInstance().ShowEquipmentTooltip(equipmentPrefab.equipment);
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
