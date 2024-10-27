using System;
using System.Collections;
using System.Collections.Generic;
using ESDatabase.Entities;
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
        eventsTitle.SetText("???");
        eventsDescription.SetText("???");
    }
    void Update(){
        PlayerData playerData = AccountManager.Instance.playerData;
        if(playerData != null){
            if(playerData.gameData.events.Contains(events.eventID)){
                eventsTitle.SetText(events.eventTitle);
                eventsDescription.SetText(events.eventDescription);
            }else{
                eventsTitle.SetText("???");
                eventsDescription.SetText("???");
            }
        }
    }
    public void OnItemClick()
    {
        PlayerData playerData = AccountManager.Instance.playerData;
        if(!playerData.gameData.events.Contains(events.eventID)){
            return;
        }
        Application.OpenURL(events.link);
    }
    public void setData(EventSO eventData){
        events = eventData;
    }
}