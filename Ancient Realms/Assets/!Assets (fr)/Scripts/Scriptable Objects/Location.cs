using System;
using System.Collections.Generic;
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
    public SoundType background;
    public bool hasWater = false;
    public bool canAccessInventory = true;
    public bool canAccessJournal = true;
    public bool canAccessMap = true;
    public bool canAccessCombatMode = true;
    public bool toggleStamina = true;
    public bool visibleEncyc = true;
    public bool hasInterior = false;
    public bool instanceMission = false;
    public List<LocationData> locations;
}
