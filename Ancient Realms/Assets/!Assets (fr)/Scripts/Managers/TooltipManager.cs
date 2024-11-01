using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    private static TooltipManager Instance;
    [Header("Equipment Tooltip")]
    [SerializeField] public GameObject equipmentTooltip;
    [SerializeField] StatPrefab statPrefab;
    [SerializeField] TextMeshProUGUI eqName;
    [SerializeField] TextMeshProUGUI eqTierAndLevel;
    [SerializeField] RectTransform equipmentStats;
    [Header("Equipment Tooltip")]
    [SerializeField] public GameObject nftTooltip;
    [SerializeField] TextMeshProUGUI nftName;
    [SerializeField] RectTransform nftStats;
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
        nftTooltip.transform.position = Input.mousePosition;
    }
    public void ShowEquipmentTooltip(EquipmentSO equipment){
        if (!equipmentTooltip.activeSelf)
        {
            equipmentTooltip.SetActive(true);
            equipmentTooltip.transform.position = Input.mousePosition;
        }
        eqName.SetText(equipment.itemName);
        eqName.color = Utilities.GetColorForCulture(equipment.culture);
        eqTierAndLevel.SetText($"Tier: {equipment.tier}                        Level: {equipment.level}");
        if(equipment.equipmentType == EquipmentEnum.Armor){
            StatPrefab stat = Instantiate(statPrefab, Vector3.zero, Quaternion.identity);
            stat.transform.SetParent(equipmentStats);
            stat.transform.localScale = Vector3.one;
            stat.SetArmor(Utilities.ConvertToOneDecimal(equipment.baseArmor));
        }
        if(equipment.equipmentType == EquipmentEnum.Weapon){
            StatPrefab stat = Instantiate(statPrefab, Vector3.zero, Quaternion.identity);
            stat.transform.SetParent(equipmentStats);
            stat.transform.localScale = Vector3.one;
            stat.SetDamage(Utilities.ConvertToOneDecimal(equipment.baseDamage));
            if(equipment.weaponType != WeaponType.SpearJavelin){
                StatPrefab stat2 = Instantiate(statPrefab, Vector3.zero, Quaternion.identity);
                stat2.transform.SetParent(equipmentStats);
                stat2.transform.localScale = Vector3.one;
                stat2.SetRange(Utilities.ConvertToOneDecimal(equipment.attackRange));
            }
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(equipmentStats);
    }
    public void HideEquipmentTooltip(){
        equipmentTooltip.SetActive(false);
        Utilities.ClearContent(equipmentStats);
    }
    public void ShowNFTTooltip(NFTSO nft){
        if (!nftTooltip.activeSelf)
        {
            nftTooltip.SetActive(true);
            nftTooltip.transform.position = Input.mousePosition;
        }
        eqName.SetText(nft.nftName);
        eqName.color = Utilities.GetColorForCulture(nft.culture);
            foreach(StatBuff buff in nft.buffList){
                switch(buff.buffType){
                    case BuffType.Health:
                        StatPrefab healthStat = Instantiate(statPrefab, Vector3.zero, Quaternion.identity);
                        healthStat.transform.SetParent(nftStats);
                        healthStat.transform.localScale = Vector3.one;
                        healthStat.SetDamage(Utilities.ConvertToOneDecimal(buff.value));        
                    break;
                    case BuffType.Armor:
                        StatPrefab armorStat = Instantiate(statPrefab, Vector3.zero, Quaternion.identity);
                        armorStat.transform.SetParent(nftStats);
                        armorStat.transform.localScale = Vector3.one;
                        armorStat.SetArmor(Utilities.ConvertToOneDecimal(buff.value));        
                    break;
                    case BuffType.Stamina:
                        StatPrefab staminaStat = Instantiate(statPrefab, Vector3.zero, Quaternion.identity);
                        staminaStat.transform.SetParent(nftStats);
                        staminaStat.transform.localScale = Vector3.one;
                        staminaStat.SetStamina(Utilities.ConvertToOneDecimal(buff.value));        
                    break;
                    case BuffType.Speed:
                        StatPrefab speedStat = Instantiate(statPrefab, Vector3.zero, Quaternion.identity);
                        speedStat.transform.SetParent(nftStats);
                        speedStat.transform.localScale = Vector3.one;
                        speedStat.SetSpeed(Utilities.ConvertToOneDecimal(buff.value));        
                    break;
                }
            }
        LayoutRebuilder.ForceRebuildLayoutImmediate(nftStats);
    }
    public void HideNFTTooltip(){
        nftTooltip.SetActive(false);
        Utilities.ClearContent(nftStats);
    }
}
