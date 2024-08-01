using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
[CreateAssetMenu(fileName = "New Location", menuName = "SO/Location")]
public class LocationSO : ScriptableObject
{
    public int id;
    public string locationName;
    public string description;
    public CultureEnum culture;
    public string SceneName;
    public Sprite image;
}
