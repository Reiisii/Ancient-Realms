using System.Collections;
using System.Collections.Generic;
using Ink.Parsed;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapTrigger : MonoBehaviour
{
    [Header("Location Data")]
    [SerializeField] private LocationSO location;
    [Header("Panel")]
    [SerializeField] private GameObject Panel;
    [SerializeField] private TextMeshProUGUI locationName;
    [SerializeField] private string locationScene;
    [SerializeField] private SpriteRenderer locationImage;
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
            Debug.Log("Player in Range");
            Panel.SetActive(true);
            
        }else{
            Panel.SetActive(false);
        }
    }
    public async void ChangeScene(){
        PlayerUIManager.GetInstance().TransitionMapUI();
        await PlayerUIManager.GetInstance().OpenDarkenUI();
        await PlayerUIManager.GetInstance().CloseDarkenUI();
        await PlayerUIManager.GetInstance().OpenLoadingUI();
        SceneManager.UnloadSceneAsync(PlayerStats.GetInstance().localPlayerData.gameData.lastLocationVisited).completed += (operation) => {
            PlayerUIManager.GetInstance().backgroundGO.SetActive(false);
            SceneManager.LoadSceneAsync(locationScene, LoadSceneMode.Additive).completed += async (operation) => {
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
