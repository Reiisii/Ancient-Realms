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
    [Header("Panel")]
    [SerializeField] private GameObject Panel;
    [SerializeField] private TextMeshProUGUI locationName;
    [SerializeField] private string locationScene;
    [SerializeField] private Image locationImage;
    private bool playerInRange;

    void Start()
    {
        locationName.SetText(location.locationName);
        locationImage.sprite = location.image;
        locationScene = location.SceneName;
    }

    private void Awake(){
        Panel.SetActive(false);
        playerInRange = false;
    }
    private void Update(){
        if(playerInRange){
            Panel.SetActive(true);
            if(PlayerController.GetInstance().GetInteractPressed()){
                ChangeScene();
            }
        }else{
            Panel.SetActive(false);
        }
    }
    public async void ChangeScene(){
        PlayerUIManager.GetInstance().TransitionMapUI();
        await PlayerUIManager.GetInstance().OpenLoadingUI();
        PlayerController.GetInstance().mapActionMap.Disable();
        LocationSettingsManager.GetInstance().LoadSettings(locationScene);
        SceneManager.UnloadSceneAsync(PlayerStats.GetInstance().localPlayerData.gameData.lastLocationVisited).completed += (operation) => {
            PlayerUIManager.GetInstance().backgroundGO.SetActive(false);
            SceneManager.LoadSceneAsync(locationScene, LoadSceneMode.Additive).completed += async (operation) => {
                PlayerStats.GetInstance().localPlayerData.gameData.lastLocationVisited = locationScene;
                PlayerStats.GetInstance().isDataDirty = true;
                await PlayerUIManager.GetInstance().CloseLoadingUI();
                await PlayerUIManager.GetInstance().OpenPlayerUI();
                
            };
        };
    }
    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            playerInRange = false;
        }
    }
}
