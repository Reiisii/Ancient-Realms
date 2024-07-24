using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject VisualCue;
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;
    [SerializeField] private PlayerController playerController;
    private bool playerInRange;
    private void Awake(){
        VisualCue.SetActive(false);
        playerInRange = false;
    }
    private void Update(){
        if(playerInRange){
            Debug.Log("Player in Range");
            VisualCue.SetActive(true);
            if(playerController.GetInteractPressed()){
                Debug.Log(inkJSON.text);
            }
        }else{
            VisualCue.SetActive(false);
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
