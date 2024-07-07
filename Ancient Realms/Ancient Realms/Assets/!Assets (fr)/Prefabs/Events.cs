using System;
using System.Collections;
using System.Collections.Generic;
using Solana.Unity.SDK.Nft;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Events : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI eventsTitle;
    [SerializeField] TextMeshProUGUI eventsDescription;
    
    public void setData(eventsData eventData){
        eventsTitle.SetText(eventData.title);
        eventsDescription.SetText(eventData.description);
    }
}