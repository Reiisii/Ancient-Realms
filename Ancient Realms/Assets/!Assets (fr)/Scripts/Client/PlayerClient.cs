using System;
using System.Collections;
using System.Collections.Generic;
using Unisave;
using Unisave.Entities;
using Unisave.Facades;
using Unisave.Broadcasting;
using UnityEngine;
using UnityEngine.UI;
using ESDatabase.Messages;

public class PlayerClient : UnisaveBroadcastingClient
{
    private async void OnEnable()
    {
        var subscription = await OnFacet<DatabaseService>
            .CallAsync<ChannelSubscription>(
                nameof(DatabaseService.JoinOnlineChannel)
            );
        
        // customize the message routing    
        FromSubscription(subscription)
            .Forward<ChatMessage>(ChatMessageReceived)
            .Forward<NewExistingSession>(NewExistingSession)
            .Forward<PlayerJoinMessage>(PlayerJoined)
            .ElseLogWarning();
    }

    void ChatMessageReceived(ChatMessage msg)
    {
        Debug.Log($"[{msg.playerName}]: {msg.message}");
    }
    void NewExistingSession(NewExistingSession msg)
    {
        AccountManager.Instance.CheckSession(msg.message);
    }
    void PlayerJoined(PlayerJoinMessage msg)
    {
        // "John joined the room"
        Debug.Log($"{msg.playerName} joined the room");
    }
    protected override void OnConnectionLost()
    {
        // display a spinner, prevent user actions
        Debug.Log("Connection lost, reconnecting...");
    }

    protected override void OnConnectionRegained()
    {
        // everything is back to normal
        Debug.Log("Connection established again.");
    }

}
