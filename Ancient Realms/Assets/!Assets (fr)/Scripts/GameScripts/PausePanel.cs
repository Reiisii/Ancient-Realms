using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    public void BackToMenu(){
        Time.timeScale = 1f;
        PauseManager.GetInstance().missionPausePanel.SetActive(false);
        PauseManager.GetInstance().pausePanel.SetActive(false);
        PlayerUIManager.GetInstance().BackToMainMenu();
    }
    public async void ExitMission(){
        MissionManager.GetInstance().EndMission();

        PauseManager.GetInstance().missionPausePanel.SetActive(false);
        await PlayerUIManager.GetInstance().ClosePlayerUI();
        PlayerUIManager.GetInstance().LastLocation();
    }
    public void BackToGame(){
        Time.timeScale = 1f;
        if(MissionManager.GetInstance().inMission){
            PauseManager.GetInstance().missionPausePanel.SetActive(false);
        }else{
            PauseManager.GetInstance().pausePanel.SetActive(false);
        }
    }
}
