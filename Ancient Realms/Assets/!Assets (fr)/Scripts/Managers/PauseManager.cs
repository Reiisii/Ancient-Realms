using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    private static PauseManager Instance;
    [Header("Game Object")]
    [SerializeField] public GameObject pausePanel;
    [SerializeField] public GameObject missionPausePanel;
    private void Awake(){
        if(Instance != null){
            Debug.LogWarning("Found more than one Pause Manager in the scene");
        }
        Instance = this;
    }
    public static PauseManager GetInstance(){
        return Instance;
    }
    public void OpenPause()
    {
        if(MissionManager.GetInstance().inMission){
            if(missionPausePanel.activeSelf == true){
                Time.timeScale = 1f;
                PlayerController.GetInstance().playerActionMap.Enable();
                PlayerController.GetInstance().pauseActionMap.Disable();
                missionPausePanel.SetActive(false);
            }else{  
                Time.timeScale = 0f;
                PlayerController.GetInstance().playerActionMap.Disable();
                PlayerController.GetInstance().pauseActionMap.Enable();
                missionPausePanel.SetActive(true);
            }
        }else{
            if(pausePanel.activeSelf == true){
                Time.timeScale = 1f;
                PlayerController.GetInstance().playerActionMap.Enable();
                PlayerController.GetInstance().pauseActionMap.Disable();
                pausePanel.SetActive(false);
            }else{  
                Time.timeScale = 0f;
                PlayerController.GetInstance().playerActionMap.Disable();
                PlayerController.GetInstance().pauseActionMap.Enable();
                pausePanel.SetActive(true);
            }
        }
    }
}
