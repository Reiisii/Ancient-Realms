using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocationSettingsManager : MonoBehaviour
{
    private static LocationSettingsManager Instance;
    List<LocationSO> locationArray;
    public LocationSO lastLocationVisited;
    public LocationSO locationSettings;
    private void Awake(){
        locationArray = Resources.LoadAll<LocationSO>("LocationSO").ToList();
        locationSettings = locationArray.Where(q => q.SceneName.Equals(AccountManager.Instance.playerData.gameData.lastLocationVisited)).FirstOrDefault();
        if (Instance == null)
        {
            // If not, set this as the instance and make it persistent
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Found more than one Location Settings Manager in the scene");
            Destroy(gameObject);
        }
    }
    public static LocationSettingsManager GetInstance(){
        return Instance;
    }
    public void LoadSettings(string location){
        if(locationSettings != null){
            lastLocationVisited = locationSettings;
        }else{
            lastLocationVisited = locationArray.Where(q => q.SceneName.Equals(location)).FirstOrDefault();;
        }
        locationSettings = locationArray.Where(q => q.SceneName.Equals(location)).FirstOrDefault();
    }
    public void LoadSettings(LocationSO location){
        if(locationSettings != null){
            lastLocationVisited = locationSettings;
        }else{
            lastLocationVisited = location;
        }
        locationSettings = location;
    }
}
