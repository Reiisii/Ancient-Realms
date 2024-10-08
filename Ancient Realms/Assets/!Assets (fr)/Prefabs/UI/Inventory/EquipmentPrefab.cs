using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentPrefab : MonoBehaviour
{
    [SerializeField] Image equipmentIcon;
    // [SerializeField] TextMeshProUGUI tier;
    // [SerializeField] TextMeshProUGUI level;
    public EquipmentSO equipment; 
    
    public void InitializeEquipment()
    {
        if (equipment != null)
        {
            
            equipmentIcon.sprite = equipment.image;
        }
        else
        {
            Debug.LogError("Equipment is null, cannot display equipment data.");
        }
    }
    // Update is called once per frame
    public void SetData(EquipmentSO equipmentData)
    {
        equipment = equipmentData;
        InitializeEquipment();
    }
}
