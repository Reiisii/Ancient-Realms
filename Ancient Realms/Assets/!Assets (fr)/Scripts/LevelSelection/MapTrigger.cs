using System.Collections;
using System.Collections.Generic;
using Ink.Parsed;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MapTrigger : MonoBehaviour
{
    [Header("Location Data")]
    [SerializeField] private LocationSO location;
    [Header("Panel")]
    [SerializeField] private GameObject Panel;
    [SerializeField] private TextMeshProUGUI locationName;
    [SerializeField] private SpriteRenderer locationImage;
    private bool playerInRange;

    void Start()
    {
        locationName.SetText(location.locationName);
        locationImage.sprite = location.image;
    }

    private void Awake(){
        Panel.SetActive(false);
        playerInRange = false;
    }
    private void Update(){
        if(playerInRange){
            Debug.Log("Player in Range");
            Panel.SetActive(true);
            
        }else{
            Panel.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            playerInRange = false;
        }
    }
}
