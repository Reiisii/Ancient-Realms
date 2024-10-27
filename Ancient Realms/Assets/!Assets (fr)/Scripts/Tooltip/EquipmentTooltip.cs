using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentTooltip : MonoBehaviour
{
    public EquipmentSO equipment;
    private void OnMouseEnter(){
        Debug.Log("ya");
        TooltipManager.GetInstance().ShowEquipmentTooltip(gameObject.GetComponent<EquipmentPrefab>().equipment);
    }

    private void OnMouseExit(){
        Debug.Log("no");
        TooltipManager.GetInstance().HideEquipmentTooltip();
    }
    private void OnDestroy(){
        if(TooltipManager.GetInstance().equipmentTooltip.activeSelf) TooltipManager.GetInstance().HideEquipmentTooltip();
    }
    private void OnDisable(){
        if(TooltipManager.GetInstance().equipmentTooltip.activeSelf) TooltipManager.GetInstance().HideEquipmentTooltip();
    }
}
