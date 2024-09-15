
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class EnterDistrict : MonoBehaviour
{
    [Header("Location TP")]
    [SerializeField] private float x;
    [SerializeField] private string locationName;
    
    private bool playerInRange;
    async void Update()
    {
        // Get the current position of the object
        Vector3 currentPosition = PlayerController.GetInstance().gameObject.transform.position;
        if(playerInRange){
            if(PlayerController.GetInstance().GetInteractPressed()){
                PlayerController.GetInstance().playerActionMap.Disable();
                await Open();    
                PlayerController.GetInstance().gameObject.transform.position = new Vector3(x, currentPosition.y, currentPosition.z);
                await Close();
            }
            PlayerUIManager.GetInstance().locationText.SetText(locationName);
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
            
            PlayerUIManager.GetInstance().locationPlaque.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            playerInRange = false;
            
            PlayerUIManager.GetInstance().locationPlaque.GetComponent<LogoutAnimation>().Close();
        }
    }
}
