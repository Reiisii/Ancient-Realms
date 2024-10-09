using System.Collections;
using System.Collections.Generic;
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
    // Update is called once per frame
    public void SetData(EquipmentSO equipmentData)
    {
        equipment = equipmentData;

        InitializeEquipment();
    }
}
