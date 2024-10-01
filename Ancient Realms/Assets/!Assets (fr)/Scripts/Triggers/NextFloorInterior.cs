using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class NextFloorInterior : MonoBehaviour
{
    [Header("Coordinate")]
    [SerializeField] private float x;
    [SerializeField] private float y;
    
    [Header("Building Data")]
    [SerializeField] private string floorName;
    [SerializeField] private string districtName;
    [SerializeField] private GameObject interiorGrid;
    [SerializeField] private GameObject exteriorGrid;
    [SerializeField] private bool isInterior = true;
    
    private bool playerInRange;
    async void Update()
    {
        // Get the current position of the object
        Vector3 currentPosition = PlayerController.GetInstance().gameObject.transform.position;
        if(playerInRange){
            if(PlayerController.GetInstance().GetInteractPressed() && PlayerController.GetInstance().playerActionMap.enabled){
                PlayerController.GetInstance().playerActionMap.Disable();
                await Open();
                PlayerController.GetInstance().gameObject.transform.position = new Vector3(x, y, currentPosition.z);
                if(isInterior){
                    interiorGrid.SetActive(true);
                    exteriorGrid.SetActive(false);
                    PlayerUIManager.GetInstance().locationText.SetText(districtName);
                }else{
                    interiorGrid.SetActive(false);
                    exteriorGrid.SetActive(true);
                    PlayerUIManager.GetInstance().locationText.SetText(floorName);
                }
                
                await Close();
            }
        }
    }
    private async Task Open(){
        PlayerUIManager.GetInstance().locationPlaque.SetActive(true);
        await PlayerUIManager.GetInstance().OpenDarkenUI();    
    }
    public async Task Close(){
        await PlayerUIManager.GetInstance().CloseDarkenUI(); 
        PlayerUIManager.GetInstance().locationPlaque.SetActive(false);
        PlayerController.GetInstance().playerActionMap.Enable();
    }
    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            playerInRange = true;
            if(interiorGrid.activeSelf){
                PlayerUIManager.GetInstance().locationText.SetText(floorName);
                PlayerUIManager.GetInstance().locationPlaque.SetActive(true);
            }else{
                PlayerUIManager.GetInstance().locationText.SetText(floorName);
                PlayerUIManager.GetInstance().locationPlaque.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            playerInRange = false;
            if(interiorGrid.activeSelf){
                PlayerUIManager.GetInstance().locationText.SetText(districtName);
                PlayerUIManager.GetInstance().locationPlaque.GetComponent<LogoutAnimation>().Close();
            }else{
                PlayerUIManager.GetInstance().locationText.SetText(floorName);
                PlayerUIManager.GetInstance().locationPlaque.GetComponent<LogoutAnimation>().Close();
            }
        }
    }
}
