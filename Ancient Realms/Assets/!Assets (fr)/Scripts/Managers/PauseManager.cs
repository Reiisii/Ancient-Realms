using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    private static PauseManager Instance;
    [Header("Game Object")]
    [SerializeField] public GameObject pausePanel;
    private void Awake(){
        if(Instance != null){
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        Instance = this;
    }
    public static PauseManager GetInstance(){
        return Instance;
    }
    public void OpenPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if(pausePanel.activeSelf == true){
                Time.timeScale = 1f;
                PlayerController.GetInstance().playerActionMap.Enable();
                PlayerController.GetInstance().pauseActionmap.Disable();
                pausePanel.SetActive(false);
            }else{  
                Time.timeScale = 0f;
                PlayerController.GetInstance().playerActionMap.Disable();
                PlayerController.GetInstance().pauseActionmap.Enable();
                pausePanel.SetActive(true);
            }
            
        }
    }
    public void OpenPause()
    {
        if(pausePanel.activeSelf == true){
            Time.timeScale = 1f;
            PlayerController.GetInstance().playerActionMap.Enable();
            PlayerController.GetInstance().pauseActionmap.Disable();
            pausePanel.SetActive(false);
        }else{  
            Time.timeScale = 0f;
            PlayerController.GetInstance().playerActionMap.Disable();
            PlayerController.GetInstance().pauseActionmap.Enable();
            pausePanel.SetActive(true);
        }
    }
}