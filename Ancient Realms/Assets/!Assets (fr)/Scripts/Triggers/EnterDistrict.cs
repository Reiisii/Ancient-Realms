
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class EnterDistrict : MonoBehaviour
{
    [Header("Location TP")]
    [SerializeField] private float x;
    [SerializeField] private string locationName;
    [SerializeField] private GameObject districtPanel;
    [SerializeField] private CanvasGroup canvasGroup;
    [Header("Animation")]
    [SerializeField] float fadeDuration;    
    [SerializeField] EaseTypes fadeEaseType;
    
    private bool playerInRange;
    async void Update()
    {
        // Get the current position of the object
        Vector3 currentPosition = PlayerStats.GetInstance().gameObject.transform.position;
        if(playerInRange){
            if(PlayerController.GetInstance().GetInteractPressed()){
                PlayerController.GetInstance().playerActionMap.Disable();
                await Open();    
                PlayerStats.GetInstance().gameObject.transform.position = new Vector3(x, currentPosition.y, currentPosition.z);
                await Close();
            }
            PlayerUIManager.GetInstance().locationText.SetText(locationName);
        }
    }
    private async Task Open(){
        PlayerUIManager.GetInstance().locationPlaque.SetActive(true);
        await canvasGroup.DOFade(1, 0.5f).SetEase((Ease)fadeEaseType).SetUpdate(true).AsyncWaitForCompletion();
    }
    public async Task Close(){
        await canvasGroup.DOFade(0, fadeDuration).SetEase((Ease)fadeEaseType).SetUpdate(true).OnComplete(() =>{
            PlayerUIManager.GetInstance().locationPlaque.SetActive(false);
        }).AsyncWaitForCompletion();
        PlayerController.GetInstance().playerActionMap.Enable();
    }
    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            playerInRange = true;
            
            districtPanel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            playerInRange = false;
            
            districtPanel.GetComponent<LogoutAnimation>().Close();
        }
    }
}
