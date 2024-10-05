using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopButtonScript : MonoBehaviour
{
    // Start is called before the first frame update


    // Update is called once per frame
    public void Click(string val){
        if(PlayerController.GetInstance() != null){
            if(PlayerController.GetInstance().playerActionMap.enabled){
                switch(val){
                    case "inventory":
                        InventoryManager.GetInstance().OpenInventory();
                    break;
                    case "journal":
                        QuestManager.GetInstance().OpenJournal();
                    break;
                    case "map":
                        MapManager.GetInstance().OpenMap();
                    break;
                    case "pause":
                        PauseManager.GetInstance().OpenPause();
                    break;
                }
            }
            
        }
    }
}
