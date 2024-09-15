
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class EnterBuilding : MonoBehaviour
{
    [Header("Building Data")]
    [SerializeField] private string buildingName;
    [SerializeField] private string districtName;
    [SerializeField] private GameObject districtPanel;
    [SerializeField] private GameObject interiorGrid;
    [SerializeField] private GameObject exteriorGrid;
    [Header("Loading")]
    [SerializeField] private CanvasGroup canvasGroup;
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
            if(interiorGrid.activeSelf){
                PlayerUIManager.GetInstance().locationText.SetText(districtName);
                districtPanel.SetActive(true);
            }else{
                PlayerUIManager.GetInstance().locationText.SetText(buildingName);
                districtPanel.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            playerInRange = false;
            if(interiorGrid.activeSelf){
                PlayerUIManager.GetInstance().locationText.SetText(districtName);
                districtPanel.GetComponent<LogoutAnimation>().Close();
            }else{
                PlayerUIManager.GetInstance().locationText.SetText(buildingName);
                districtPanel.GetComponent<LogoutAnimation>().Close();
            }
        }
    }
}
