using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Equipment", menuName = "SO/Equipment")]
public class EquipmentSO : ScriptableObject
{
    public int id;
    public string itemName;
    public string description;
    public CultureEnum culture;
    public EquipmentEnum equipmentType;
    public WeaponType weaponType;
    public Sprite image;
}
