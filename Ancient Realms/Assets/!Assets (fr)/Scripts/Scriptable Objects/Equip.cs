using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Equip", menuName = "SO/Equip")]
public class EquipSO : ScriptableObject
{
    public int id;
    public Sprite icon;
    public Sprite front;
    public Sprite back;
}
