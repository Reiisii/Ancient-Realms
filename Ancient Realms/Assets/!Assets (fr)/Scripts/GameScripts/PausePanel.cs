using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    public async void BackToMenu(){
        Time.timeScale = 1f;
        await PlayerUIManager.GetInstance().ClosePlayerUI();
        await PlayerUIManager.GetInstance().OpenDarkenUI();
        Play.GetInstance().PlayMainMenu();
    }
    public void BackToGame(){
        Time.timeScale = 1f;
        PauseManager.GetInstance().OpenPause();
    }
}
