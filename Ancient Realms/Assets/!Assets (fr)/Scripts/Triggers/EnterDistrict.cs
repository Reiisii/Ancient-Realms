
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
    [SerializeField] private TextMeshProUGUI text;
    [Header("Animation")]
    [SerializeField] GameObject panelGO;
    [SerializeField] float fadeDuration;    
    [SerializeField] EaseTypes fadeEaseType;
    
    private bool playerInRange;
    async void Update()
    {
        // Get the current position of the object
        Vector3 currentPosition = PlayerStats.GetInstance().gameObject.transform.position;
        if(playerInRange){
            if(PlayerController.GetInstance().GetInteractPressed()){
                    // Set the new position with the desired x value and retain y and z
                await Open();    
                PlayerStats.GetInstance().gameObject.transform.position = new Vector3(x, currentPosition.y, currentPosition.z);
                await Close();
            }
        }
    }
    private async Task Open(){
        panelGO.SetActive(true);
        await canvasGroup.DOFade(1, 0.5f).SetEase((Ease)fadeEaseType).SetUpdate(true).AsyncWaitForCompletion();
    }
    public async Task Close(){
        await canvasGroup.DOFade(0, fadeDuration).SetEase((Ease)fadeEaseType).SetUpdate(true).OnComplete(() =>{
            panelGO.SetActive(false);
        }).AsyncWaitForCompletion();
    }
    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            playerInRange = true;
            text.SetText(locationName);
            districtPanel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            playerInRange = false;
            text.SetText(locationName);
            districtPanel.SetActive(false);
            
        }
    }
}
