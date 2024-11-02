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
                await Open();
                if(SmithingGameManager.GetInstance().inMiniGame){
                    SmithingGameManager.GetInstance().EndGame();
                    StartCoroutine(AudioManager.GetInstance().FadeInAmbience(AudioManager.GetInstance().waterAmbience, 0.4f, 0.5f));
                }else{
                    SmithingGameManager.GetInstance().StartGame();
                    StartCoroutine(AudioManager.GetInstance().FadeOutAmbience(AudioManager.GetInstance().waterAmbience, 0.5f));    
                }
                PlayerController.GetInstance().isInterior = !PlayerController.GetInstance().isInterior;
                await Close();
            }
        }
    }
    private async Task Open(){
        AudioManager.GetInstance().PlayAudio(SoundType.ENTER);
        PlayerUIManager.GetInstance().locationPlaque.SetActive(true);
        PlayerController.GetInstance().playerActionMap.Disable();
        await PlayerUIManager.GetInstance().OpenDarkenUI();
    }
    public async Task Close(){
        await PlayerUIManager.GetInstance().CloseDarkenUI();
        PlayerUIManager.GetInstance().locationPlaque.SetActive(false);
        PlayerController.GetInstance().playerActionMap.Enable();
        AudioManager.GetInstance().PlayAudio(SoundType.CLOSE);
    }
    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            playerInRange = true;
            if(interiorGrid != null && interiorGrid.activeSelf){
                PlayerUIManager.GetInstance().locationText.SetText("End Shift?");
                PlayerUIManager.GetInstance().locationPlaque.SetActive(true);
            }else{
                PlayerUIManager.GetInstance().locationText.SetText("Jan Janius Smithing Game");
                PlayerUIManager.GetInstance().locationPlaque.SetActive(true);
            }
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
