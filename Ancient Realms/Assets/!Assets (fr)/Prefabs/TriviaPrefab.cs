using System;
using System.Collections;
using System.Collections.Generic;
using Solana.Unity.SDK.Nft;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TriviaPrefab : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI triviaTitle;
    [SerializeField] TextMeshProUGUI triviaDescription;
    public TriviaSO trivia;
    public void setData(TriviaSO trivData){
        trivia = trivData;
        triviaTitle.SetText(trivData.triviaTitle);
        triviaDescription.SetText(trivData.triviaDescription);
    }
    public void OnItemClick()
    {
        Application.OpenURL(trivia.link);
    }
}