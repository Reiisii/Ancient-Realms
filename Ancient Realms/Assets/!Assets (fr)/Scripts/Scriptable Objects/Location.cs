using UnityEngine;
[CreateAssetMenu(fileName = "New Location", menuName = "SO/Location")]
public class LocationSO : ScriptableObject
{
    [Header("Map Data")]
    public int id;
    public string locationName;
    public string description;
    public CultureEnum culture;
    public string SceneName;
    public Sprite image;
    [Header("Map Settings")]
    public bool canAccessInventory = true;
    public bool canAccessJournal = true;
    public bool canAccessMap = true;
    public bool canAccessCombatMode = true;
    public bool toggleStamina = true;
}
