using System;
using System.Collections;
using System.Collections.Generic;
using Solana.Unity.SDK.Nft;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Trivia : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI triviaTitle;
    [SerializeField] TextMeshProUGUI triviaDescription;
    
    public void setData(triviaData trivData){
        triviaTitle.SetText(trivData.title);
        triviaDescription.SetText(trivData.description);
    }
}