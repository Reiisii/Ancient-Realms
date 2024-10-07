using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StartSmithingGame : MonoBehaviour
{
    [SerializeField] private GameObject interiorGrid;
    [SerializeField] private GameObject exteriorGrid;
    private bool playerInRange;
    async void Update()
    {
        if(playerInRange && PlayerController.GetInstance().playerActionMap.enabled){
            if(PlayerController.GetInstance().GetInteractPressed()){
                if(SmithingGameManager.GetInstance().inMiniGame){
                    SmithingGameManager.GetInstance().EndGame();
                }else{
                    SmithingGameManager.GetInstance().StartGame();
                }
                await Open();
                if(!interiorGrid.activeSelf){
                    interiorGrid.SetActive(true);
                    exteriorGrid.SetActive(false);
                    PlayerUIManager.GetInstance().locationText.SetText("End Shift?");
                }else{
                    interiorGrid.SetActive(false);
                    exteriorGrid.SetActive(true);
                    PlayerUIManager.GetInstance().locationText.SetText("Jan Janius Smithing Game");
                }
                
                await Close();
                
            }
        }
        if(playerInRange && PlayerController.GetInstance().playerActionMap.enabled && SmithingGameManager.GetInstance().inMiniGame){
                if(PlayerController.GetInstance().GetInteractPressed()){
                    SmithingGameManager.GetInstance().inMiniGame = false;
                    PlayerController.GetInstance().playerActionMap.Disable();
                    await Open();
                    interiorGrid.SetActive(false);
                    exteriorGrid.SetActive(true);
                    
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

        }
    }

    private void OnTriggerExit2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            playerInRange = false;
            if(interiorGrid != null && interiorGrid.activeSelf){
                PlayerUIManager.GetInstance().locationText.SetText("End Shift?");
                PlayerUIManager.GetInstance().locationPlaque.GetComponent<LogoutAnimation>().Close();
            }else{
                PlayerUIManager.GetInstance().locationText.SetText("Jan Janius Smithing Game");
                PlayerUIManager.GetInstance().locationPlaque.GetComponent<LogoutAnimation>().Close();
            }
        }
    }
}
