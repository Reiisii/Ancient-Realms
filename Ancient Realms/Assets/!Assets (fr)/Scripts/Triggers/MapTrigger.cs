using System.Collections;
using System.Collections.Generic;
using Ink.Parsed;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapTrigger : MonoBehaviour
{
    [Header("Location Data")]
    [SerializeField] private LocationSO location;
    [SerializeField] private MissionSO mission;
    private bool playerInRange;

    private void Awake(){
        playerInRange = false;
    }
    private void Update(){
        if(playerInRange){
            if(mission != null){
                PlayerUIManager.GetInstance().OpenMission(mission);
            }else{
                PlayerUIManager.GetInstance().OpenLocation(location);
            }
            if(PlayerController.GetInstance().mapActionMap.enabled){
                if(PlayerController.GetInstance().GetInteractPressed()){
                    ChangeScene();
                }
            }
        }else{
            PlayerUIManager.GetInstance().CloseMissionLocation();
        }
    }
    private void OnDisable(){
        PlayerUIManager.GetInstance().CloseMissionLocation();
    }
    public async void ChangeScene(){
        string prevLoc = LocationSettingsManager.GetInstance().locationSettings.SceneName;
        AudioManager.GetInstance().StopAmbience();
        PlayerStats.GetInstance().isCombatMode = false;
        PlayerUIManager.GetInstance().TransitionMapUI();
        PlayerStats.GetInstance().ReplenishStats();
        await PlayerUIManager.GetInstance().ClosePlayerUI();
        await PlayerUIManager.GetInstance().OpenLoadingUI();
        PlayerController.GetInstance().mapActionMap.Disable();
        LocationSettingsManager.GetInstance().LoadSettings(location.locationName);
        SceneManager.UnloadSceneAsync(prevLoc).completed += (operation) => {
            PlayerUIManager.GetInstance().backgroundGO.SetActive(false);
            SceneManager.LoadSceneAsync(location.SceneName, LoadSceneMode.Additive).completed += async (operation) => {
                LocationSO loadedLocation = LocationSettingsManager.GetInstance().locationSettings;
                if(loadedLocation.canAccessCombatMode && !loadedLocation.canAccessInventory) AudioManager.GetInstance().PlayMusic(MusicType.Combat, 0.6f, 1f); 
                else AudioManager.GetInstance().PlayMusic(MusicType.Town, 1f, 1f);
                AudioManager.GetInstance().SetAmbience(PlayerUIManager.GetInstance().time.hours < 17 && PlayerUIManager.GetInstance().time.hours > 7, loadedLocation.background, loadedLocation.hasWater);
                if(!loadedLocation.instanceMission){
                    PlayerStats.GetInstance().localPlayerData.gameData.lastLocationVisited = location.SceneName;
                    PlayerStats.GetInstance().localPlayerData.gameData.isInterior = false;
                    PlayerStats.GetInstance().localPlayerData.gameData.lastX = loadedLocation.locations[0].location.x;
                    PlayerStats.GetInstance().localPlayerData.gameData.lastY = loadedLocation.locations[0].location.y;
                    PlayerStats.GetInstance().isDataDirty = true;
                }else{
                    PlayerStats.GetInstance().stopSaving = true;
                }
                await PlayerUIManager.GetInstance().CloseLoadingUI();
                await PlayerUIManager.GetInstance().OpenPlayerUI();
                
            };
        };
    }
    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag == "MapP"){
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider){
        if(collider.gameObject.tag == "MapP"){
            playerInRange = false;
        }
    }
}
