using System;
using System.Collections;
using System.Collections.Generic;
using ESDatabase.Classes;
using UnityEngine;
[Serializable]
[CreateAssetMenu(fileName = "New Equipment", menuName = "SO/Equipment")]
public class EquipmentSO : ScriptableObject
{
    [Header("Item Data")]
    public int equipmentId;
    public string itemName;
    public string description;
    [Header("Item Stats")]
    public float baseArmor;
    public float baseDamage;
    public float attackRange;
    public bool isStackable;
    public int stackCount;
    public int tier;
    public int level;
    public CultureEnum culture;
    public EquipmentEnum equipmentType;
    public ArmorType armorType;
    public WeaponType weaponType;
    [Header("Asset")]
    public Sprite image;
    public Sprite front;
    public Sprite back;
    [Header("Temp data")]
    public int dbIndex;
    public EquipmentSO CreateCopy()
    {
        // Create a new instance of QuestSO
        EquipmentSO newEquipment = ScriptableObject.CreateInstance<EquipmentSO>();
        newEquipment.equipmentId = this.equipmentId;
        newEquipment.itemName = this.itemName;
        newEquipment.description = this.description;
        newEquipment.baseArmor = this.baseArmor;
        newEquipment.baseDamage = this.baseDamage;
        newEquipment.attackRange = this.attackRange;
        newEquipment.isStackable = this.isStackable;
        newEquipment.stackCount = this.stackCount;
        newEquipment.tier = this.tier;
        newEquipment.level = this.level;
        newEquipment.culture = this.culture;
        newEquipment.equipmentType = this.equipmentType;
        newEquipment.armorType = this.armorType;
        newEquipment.weaponType = this.weaponType;
        newEquipment.image = this.image;
        newEquipment.front = this.front;  // Copy front sprite
        newEquipment.back = this.back;
        newEquipment.dbIndex = 0;
        return newEquipment;
    }
    public EquipmentSO CreateCopy(ItemData itemData)
    {
        if (itemData == null) {
            Debug.LogError("ItemData is null, cannot create copy.");
            return null;
        }
        // Create a new instance of QuestSO
        EquipmentSO newEquipment = ScriptableObject.CreateInstance<EquipmentSO>();
        newEquipment.equipmentId = this.equipmentId;
        newEquipment.itemName = this.itemName;
        newEquipment.description = this.description;
        if(this.equipmentType == EquipmentEnum.Armor){
            newEquipment.baseArmor = CalculateArmor(itemData.tier, itemData.level, this.baseArmor);
        }else newEquipment.baseArmor = 0f;
        if(this.equipmentType == EquipmentEnum.Weapon){
            newEquipment.baseDamage = CalculateDamage(itemData.tier, itemData.level, this.baseDamage);
        }else newEquipment.baseDamage = 0f;
        newEquipment.attackRange = this.attackRange;
        newEquipment.isStackable = this.isStackable;
        newEquipment.stackCount = itemData.stackAmount;
        newEquipment.tier = itemData.tier;
        newEquipment.level = itemData.level;
        newEquipment.culture = this.culture;
        newEquipment.equipmentType = this.equipmentType;
        newEquipment.armorType = this.armorType;
        newEquipment.weaponType = this.weaponType;
        newEquipment.image = this.image;
        newEquipment.front = this.front;  // Copy front sprite
        newEquipment.back = this.back;
        newEquipment.dbIndex = 0;
        return newEquipment;
    }
    public EquipmentSO CreateCopy(ItemData itemData, int i)
    {
        if (itemData == null) {
            Debug.LogError("ItemData is null, cannot create copy.");
            return null;
        }
        // Create a new instance of QuestSO
        EquipmentSO newEquipment = ScriptableObject.CreateInstance<EquipmentSO>();
        newEquipment.equipmentId = this.equipmentId;
        newEquipment.itemName = this.itemName;
        newEquipment.description = this.description;
        if(this.equipmentType == EquipmentEnum.Armor){
            newEquipment.baseArmor = CalculateArmor(itemData.tier, itemData.level, this.baseArmor);
        }else newEquipment.baseArmor = 0f;
        if(this.equipmentType == EquipmentEnum.Weapon){
            newEquipment.baseDamage = CalculateDamage(itemData.tier, itemData.level, this.baseDamage);
        }else newEquipment.baseDamage = 0f;
        newEquipment.attackRange = this.attackRange;
        newEquipment.isStackable = this.isStackable;
        newEquipment.stackCount = itemData.stackAmount;
        newEquipment.tier = itemData.tier;
        newEquipment.level = itemData.level;
        newEquipment.culture = this.culture;
        newEquipment.equipmentType = this.equipmentType;
        newEquipment.armorType = this.armorType;
        newEquipment.weaponType = this.weaponType;
        newEquipment.image = this.image;
        newEquipment.front = this.front;  // Copy front sprite
        newEquipment.back = this.back;
        newEquipment.dbIndex = i;
        return newEquipment;
    }
    public EquipmentSO CreateCopy(int i, int tier, int level)
    {
        EquipmentSO newEquipment = ScriptableObject.CreateInstance<EquipmentSO>();
        newEquipment.equipmentId = this.equipmentId;
        newEquipment.itemName = this.itemName;
        newEquipment.description = this.description;
        if(this.equipmentType == EquipmentEnum.Armor){
            newEquipment.baseArmor = CalculateArmor(tier, level, this.baseArmor);
        }else newEquipment.baseArmor = 0f;
        if(this.equipmentType == EquipmentEnum.Weapon){
            newEquipment.baseDamage = CalculateDamage(tier, level, this.baseDamage);
        }else newEquipment.baseDamage = 0f;
        newEquipment.attackRange = this.attackRange;
        newEquipment.isStackable = this.isStackable;
        newEquipment.stackCount = 1;
        newEquipment.tier = tier;
        newEquipment.level = level;
        newEquipment.culture = this.culture;
        newEquipment.equipmentType = this.equipmentType;
        newEquipment.armorType = this.armorType;
        newEquipment.weaponType = this.weaponType;
        newEquipment.image = this.image;
        newEquipment.front = this.front;  // Copy front sprite
        newEquipment.back = this.back;
        newEquipment.dbIndex = i;
        return newEquipment;
    }
    public float CalculateDamage(int tier, int level, float baseDamage)
    {
        float bd = baseDamage + (tier * 4f); // Base damage depends on tier, tier 0 has a base damage of 5
        float additionalDamage = level * 5f; // Each level adds 1.5 to damage
        
        // Ensure level is within bounds
        level = Mathf.Clamp(level, 0, 6);

        return bd + additionalDamage;
    }
    public float CalculateArmor(int tier, int level, float baseArmor)
    {
        float ba = baseArmor + (tier * 0.5f); // Base damage depends on tier, tier 0 has a base damage of 5
        float additionalDamage = level * 1.5f; // Each level adds 1.5 to damage
        
        // Ensure level is within bounds
        level = Mathf.Clamp(level, 0, 6);

        return ba + additionalDamage;
    }
}
