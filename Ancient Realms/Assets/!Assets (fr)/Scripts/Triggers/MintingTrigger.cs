using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;

public class MintingTrigger : MonoBehaviour
{
    [Header("Minting Settings")]
    
    [SerializeField] CultureEnum cultureSettings;
    [SerializeField] private string buildingName;
    private bool playerInRange;
    void Start()
    {
        
    }
 void Update()
    {
        if(playerInRange && PlayerController.GetInstance().playerActionMap.enabled){
            if(PlayerController.GetInstance().GetInteractPressed()){
                Open();
                if(PlayerUIManager.GetInstance().locationPlaque.activeSelf){
                    PlayerUIManager.GetInstance().locationText.SetText(buildingName);
                }
            }
        }
    }
    private void Open(){
        PlayerUIManager.GetInstance().mintingUI.SetActive(true);
        PlayerController.GetInstance().playerActionMap.Disable();
        PlayerController.GetInstance().mintingActionMap.Enable();
    }
    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            playerInRange = true;
            if(!PlayerUIManager.GetInstance().locationPlaque.activeSelf){
                    PlayerUIManager.GetInstance().locationText.SetText(buildingName);
                    PlayerUIManager.GetInstance().locationPlaque.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            playerInRange = false;
            if(PlayerUIManager.GetInstance().locationPlaque.activeSelf){
                PlayerUIManager.GetInstance().locationText.SetText(buildingName);
                PlayerUIManager.GetInstance().locationPlaque.GetComponent<LogoutAnimation>().Close();
            }
        }
    }
}
