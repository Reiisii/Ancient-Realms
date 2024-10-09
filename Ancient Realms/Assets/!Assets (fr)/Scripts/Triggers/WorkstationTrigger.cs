using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkstationTrigger : MonoBehaviour
{
    [SerializeField] WorkStation workstation;
    private bool playerInRange;
    // Update is called once per frame
    void Update()
    {
        if(playerInRange && PlayerController.GetInstance().playerActionMap.enabled && SmithingGameManager.GetInstance().inMiniGame && !SmithingGameManager.GetInstance().inWorkStation){
            if(PlayerController.GetInstance().GetInteractPressed()){
                SmithingGameManager.GetInstance().StartWorkStation(workstation);
            }
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
