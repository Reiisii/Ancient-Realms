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
    [SerializeField] private GameObject districtPanel;
    [SerializeField] private GameObject interiorGrid;
    [SerializeField] private GameObject exteriorGrid;
    [SerializeField] private bool isInterior = true;
    [SerializeField] private TextMeshProUGUI text;
    [Header("Loading")]
    [SerializeField] GameObject panelGO;
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
                PlayerStats.GetInstance().gameObject.transform.position = new Vector3(x, y, currentPosition.z);
                if(isInterior){
                    interiorGrid.SetActive(true);
                    exteriorGrid.SetActive(false);
                    text.SetText(districtName);
                }else{
                    interiorGrid.SetActive(false);
                    exteriorGrid.SetActive(true);
                    text.SetText(floorName);
                }
                
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
        PlayerController.GetInstance().playerActionMap.Enable();
    }
    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            playerInRange = true;
            if(interiorGrid.activeSelf){
                text.SetText(floorName);
                districtPanel.SetActive(true);
            }else{
                text.SetText(floorName);
                districtPanel.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            playerInRange = false;
            if(interiorGrid.activeSelf){
                text.SetText(districtName);
                districtPanel.GetComponent<LogoutAnimation>().Close();
            }else{
                text.SetText(floorName);
                districtPanel.GetComponent<LogoutAnimation>().Close();
            }
        }
    }
}
