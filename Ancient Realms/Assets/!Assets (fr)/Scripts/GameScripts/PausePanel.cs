using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    public void BackToMenu(){
        Time.timeScale = 1f;
        Play.GetInstance().PlayMainMenu();
    }
    public void BackToGame(){
        Time.timeScale = 1f;
        PauseManager.GetInstance().OpenPause();
    }
}
