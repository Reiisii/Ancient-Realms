using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    private static TooltipManager Instance;
    [Header("Equipment Tooltip")]
    [SerializeField] public GameObject equipmentTooltip;
    [SerializeField] StatPrefab statPrefab;
    [SerializeField] TextMeshProUGUI eqName;
    [SerializeField] TextMeshProUGUI eqTierAndLevel;
    [SerializeField] RectTransform stats;
    private void Awake(){
        if (Instance == null)
        {
            // If not, set this as the instance and make it persistent
            Instance = this;
            equipmentTooltip.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Found more than one Player UI Manager in the scene");
            Destroy(gameObject);
        }
    }
    public static TooltipManager GetInstance(){
        return Instance;
    }
    // Update is called once per frame
    void Update()
    {
        equipmentTooltip.transform.position = Input.mousePosition;
    }
    public void ShowEquipmentTooltip(EquipmentSO equipment){
        if (!equipmentTooltip.activeSelf)
        {
            equipmentTooltip.SetActive(true);
            equipmentTooltip.transform.position = Input.mousePosition;
        }
        eqName.SetText(equipment.itemName);
        eqName.color = Utilities.GetColorForCulture(equipment.culture);
        eqTierAndLevel.SetText($"Tier: {equipment.tier}                                        Level: {equipment.level}");
        if(equipment.equipmentType == EquipmentEnum.Armor){
            StatPrefab stat = Instantiate(statPrefab, Vector3.zero, Quaternion.identity);
            stat.transform.SetParent(stats);
            stat.transform.localScale = Vector3.one;
            stat.SetArmor(Utilities.ConvertToOneDecimal(equipment.baseArmor));
        }
        if(equipment.equipmentType == EquipmentEnum.Weapon){
            StatPrefab stat = Instantiate(statPrefab, Vector3.zero, Quaternion.identity);
            stat.transform.SetParent(stats);
            stat.transform.localScale = Vector3.one;
            stat.SetDamage(Utilities.ConvertToOneDecimal(equipment.baseDamage));
            StatPrefab stat2 = Instantiate(statPrefab, Vector3.zero, Quaternion.identity);
            stat2.transform.SetParent(stats);
            stat2.transform.localScale = Vector3.one;
            stat2.SetRange(Utilities.ConvertToOneDecimal(equipment.baseDamage));
        }
        
    }
    public void HideEquipmentTooltip(){
        equipmentTooltip.SetActive(false);
        ClearContent(stats);
    }
    public void ClearContent(RectTransform cPanel)
    {
        foreach (Transform child in cPanel)
        {
            Destroy(child.gameObject);
        }
    }
}
