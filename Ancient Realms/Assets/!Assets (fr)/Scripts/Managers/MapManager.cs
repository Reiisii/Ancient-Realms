using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    private static MapManager Instance;
    
    private void Awake(){
        if (Instance == null)
        {
            // If not, set this as the instance and make it persistent
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Found more than one Map Manager in the scene");
            Destroy(gameObject);
        }
    }
    
    public static MapManager GetInstance(){
        return Instance;
    }

    public async void OpenMap(){
        if(PlayerUIManager.GetInstance().mapGO.activeSelf == true){
            PlayerController.GetInstance().playerActionMap.Enable();
            PlayerController.GetInstance().mapActionMap.Disable();
            await PlayerUIManager.GetInstance().CloseMapUI();
        }else{
            PlayerController.GetInstance().playerActionMap.Disable();
            PlayerController.GetInstance().mapActionMap.Enable();
            PlayerUIManager.GetInstance().OpenMapUI();
        }
    }
}
