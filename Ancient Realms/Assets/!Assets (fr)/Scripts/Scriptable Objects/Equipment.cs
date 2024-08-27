using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Equipment", menuName = "SO/Equipment")]
public class EquipmentSO : ScriptableObject
{
    public int equipmentId;
    public string itemName;
    public string description;
    public int tier;
    public int level;
    public CultureEnum culture;
    public EquipmentEnum equipmentType;
    public WeaponType weaponType;
    public Sprite image;
    public EquipmentSO CreateCopy()
    {
        // Create a new instance of QuestSO
        EquipmentSO newEquipment = ScriptableObject.CreateInstance<EquipmentSO>();
        newEquipment.equipmentId = this.equipmentId;
        newEquipment.itemName = this.itemName;
        newEquipment.description = this.description;
        newEquipment.tier = this.tier;
        newEquipment.level = this.tier;
        newEquipment.culture = this.culture;
        newEquipment.equipmentType = this.equipmentType;
        newEquipment.weaponType = this.weaponType;
        newEquipment.image = this.image;

        return newEquipment;
    }
}
