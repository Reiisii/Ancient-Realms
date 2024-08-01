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
    public EventSO events;
    void Start(){
        eventsTitle.SetText(events.eventTitle);
        eventsDescription.SetText(events.eventDescription);
    }
    public void setData(EventSO eventData){
        events = eventData;
    }
}