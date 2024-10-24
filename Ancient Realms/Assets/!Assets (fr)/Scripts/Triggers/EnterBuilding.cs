
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
                    StartCoroutine(AudioManager.GetInstance().FadeOutAmbience(AudioManager.GetInstance().waterAmbience, 0.5f));
                    interiorGrid.SetActive(true);
                    exteriorGrid.SetActive(false);
                    PlayerUIManager.GetInstance().locationText.SetText(districtName);

                }else{
                    StartCoroutine(AudioManager.GetInstance().FadeInAmbience(AudioManager.GetInstance().waterAmbience, 0.4f, 0.5f));
                    interiorGrid.SetActive(false);
                    exteriorGrid.SetActive(true);
                    PlayerUIManager.GetInstance().locationText.SetText(buildingName);
                }
                PlayerController.GetInstance().isInterior = !PlayerController.GetInstance().isInterior;
                await Close();
            }
        }
    }
    private async Task Open(){
        AudioManager.GetInstance().PlayAudio(SoundType.ENTER);
        PlayerUIManager.GetInstance().locationPlaque.SetActive(true);
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
