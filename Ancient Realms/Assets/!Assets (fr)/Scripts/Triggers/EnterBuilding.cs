
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class EnterBuilding : MonoBehaviour
{
    [Header("Building Data")]
    [SerializeField] private string buildingName;
    [SerializeField] private string districtName;
    [SerializeField] private GameObject interiorGrid;
    [SerializeField] private GameObject exteriorGrid;
    private bool playerInRange;
    async void Update()
    {
        if(playerInRange && PlayerController.GetInstance().playerActionMap.enabled){
            if(PlayerController.GetInstance().GetInteractPressed()){
                PlayerController.GetInstance().playerActionMap.Disable();
                await Open();
                if(!interiorGrid.activeSelf){
                    interiorGrid.SetActive(true);
                    exteriorGrid.SetActive(false);
                    PlayerUIManager.GetInstance().locationText.SetText(districtName);
                }else{
                    interiorGrid.SetActive(false);
                    exteriorGrid.SetActive(true);
                    PlayerUIManager.GetInstance().locationText.SetText(buildingName);
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
            if(interiorGrid != null && interiorGrid.activeSelf){
                PlayerUIManager.GetInstance().locationText.SetText(districtName);
                PlayerUIManager.GetInstance().locationPlaque.SetActive(true);
            }else{
                PlayerUIManager.GetInstance().locationText.SetText(buildingName);
                PlayerUIManager.GetInstance().locationPlaque.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            playerInRange = false;
            if(interiorGrid != null && interiorGrid.activeSelf){
                PlayerUIManager.GetInstance().locationText.SetText(districtName);
                PlayerUIManager.GetInstance().locationPlaque.GetComponent<LogoutAnimation>().Close();
            }else{
                PlayerUIManager.GetInstance().locationText.SetText(buildingName);
                PlayerUIManager.GetInstance().locationPlaque.GetComponent<LogoutAnimation>().Close();
            }
        }
    }
}
