using System;
using System.Collections;
using System.Collections.Generic;
using Unisave;
using Unisave.Facets;
using Unisave.Facades;
using ESDatabase.Utilities;
using System.Threading.Tasks;
using ESDatabase.Entities;
using System.Linq;
using Unisave.Broadcasting;
using ESDatabase.Messages;
using ESDatabase.Services;
public class DatabaseService : Facet
{
    public string InitializeLogin(string pubkey)
    {
        List<PlayerData> playerList = DB.TakeAll<PlayerData>().Get();
        PlayerData player = playerList.FirstOrDefault(data => data.publicKey == pubkey);
        string isExisting = DBHelper.IsPlayerExisting(pubkey, player);
        DiscordFacetService.SendLoginMessageToDiscord(player.gameData.playerName + $"({player.EntityId})");
        if(CheckSession(pubkey)){
            BroadcastExistingPlayerSession(player.EntityId);
        }
        Auth.Login(player);
        return isExisting;
    }
    public bool CheckSession(string pubkey)
    {
        List<PlayerData> playerList = DB.TakeAll<PlayerData>().Get();
        PlayerData player = playerList.FirstOrDefault(d => d.publicKey == pubkey);

        string query = $"FOR s IN u_sessions FILTER s.sessionData.authenticatedPlayerId == '{player.EntityId}' RETURN s.sessionData.authenticatedPlayerId";
        List<string> data = DB.Query(query).GetAs<string>();
        if(data.Count > 0){
            if(data[0].Equals(player.EntityId)){
                return true;
            }else{
                return false;
            }
        }else{
             return false;
        }
    }
    public ChannelSubscription JoinOnlineChannel()
    {
        PlayerData p = Auth.GetPlayer<PlayerData>();
        var subscription = Broadcast
            .Channel<OnlineChannel>()
            .WithoutParameters()
            .CreateSubscription();
        
        // new player in the room broadcast
        Broadcast.Channel<OnlineChannel>()
            .WithoutParameters()
            .Send(new PlayerJoinMessage {
                playerName = p.EntityId
            });

        return subscription;
    }
    private void BroadcastExistingPlayerSession(string playerId)
    {
        var playerChannel = Broadcast.Channel<OnlineChannel>().ForPlayer(playerId);
        playerChannel.Send(new NewExistingSession
        {
            message = "New client logged into your account."
        });
    }
    public void Logout()
    {
        PlayerData p = Auth.GetPlayer<PlayerData>();
        DiscordFacetService.SendLogoutMessageToDiscord(p.gameData.playerName + $"({p.EntityId})");
        Auth.Logout();

    }
    public PlayerData GetPlayerById(string entityID)
    {
        PlayerData player = DB.Find<PlayerData>(entityID);

        return player;
    }
    public PlayerData GetPlayerByPublicKey(string pubkey)
    {
        List<PlayerData> playerList = DB.TakeAll<PlayerData>().Get();
        PlayerData player = playerList.FirstOrDefault(data => data.publicKey == pubkey);
        
        return player;
    }
    public string SaveData(PlayerData givenPlayer)
    {
        List<PlayerData> playerList = DB.TakeAll<PlayerData>().Get();
        PlayerData player = playerList.FirstOrDefault(data => data.publicKey == givenPlayer.publicKey);
        try{
            player.FillWith(givenPlayer);
            player.Save();
            return "Saving Success";
        }catch(Exception err){
            Log.Error("Error", err);
            return "Failed to save";
        }
    }
}