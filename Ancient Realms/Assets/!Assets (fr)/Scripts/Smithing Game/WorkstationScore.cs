using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkstationScore : MonoBehaviour
{
    public GameObject[] circleColors;
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        ClearCircles();   
    }

    public void ClearCircles(){
        foreach(GameObject score in circleColors){
            score.GetComponent<Image>().color = Color.black;
        }
    }
    public void UpdateScoreCircle(int roundIndex, Color scoreColor)
    {
        if (roundIndex >= 0 && roundIndex < circleColors.Length)
        {
            circleColors[roundIndex].GetComponent<Image>().color = scoreColor;
        }
    }
}
