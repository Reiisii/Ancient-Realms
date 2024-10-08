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
using ESDatabase.Classes;
public class DatabaseService : Facet
{
    public LoginResponse InitializeLoginWithoutPrice(string pubkey, string UID)
    {
        LoginResponse loginResponse = new LoginResponse();
        PlayerData player = DB.TakeAll<PlayerData>().Get().FirstOrDefault(data => data.publicKey == pubkey);
        string isExisting = DBHelper.IsPlayerExisting(pubkey, player);
        if(CheckSession(pubkey)){
            BroadcastExistingPlayerSession(player.EntityId);
        }        
        PlayerData copiedPlayer = DB.TakeAll<PlayerData>().Get().FirstOrDefault(data => data.EntityId == isExisting);
        copiedPlayer.token = UID;
        copiedPlayer.Save();
        Auth.Login(copiedPlayer);
        DiscordFacetService.SendLoginMessageToDiscord(copiedPlayer);
        loginResponse.playerData = copiedPlayer;
        loginResponse.devBlog = DiscordFacetService.GetDevBlog();
        return loginResponse;
    }
    public LoginResponse InitializeLoginWithPrice(string pubkey, string UID)
    {
        LoginResponse loginResponse = new LoginResponse();
        PlayerData player = DB.TakeAll<PlayerData>().Get().FirstOrDefault(data => data.publicKey == pubkey);
        string isExisting = DBHelper.IsPlayerExisting(pubkey, player);
        if(CheckSession(pubkey)){
            BroadcastExistingPlayerSession(player.EntityId);
        }        
        PlayerData copiedPlayer = DB.TakeAll<PlayerData>().Get().FirstOrDefault(data => data.EntityId == isExisting);
        copiedPlayer.token = UID;
        copiedPlayer.Save();
        Auth.Login(copiedPlayer);
        DiscordFacetService.SendLoginMessageToDiscord(copiedPlayer);
        loginResponse.playerData = copiedPlayer;
        loginResponse.priceData = GetPrice();
        loginResponse.devBlog = DiscordFacetService.GetDevBlog();
        return loginResponse;
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
            .JoinRoom(p.EntityId)
            .CreateSubscription();
        
        // new player in the room broadcast
        Broadcast.Channel<OnlineChannel>()
            .JoinRoom(p.EntityId)
            .Send(new PlayerJoinMessage {
                playerName = p.token
            });

        return subscription;
    }
    private void BroadcastExistingPlayerSession(string playerId)
    {
        Broadcast.Channel<OnlineChannel>()
            .ForPlayer(playerId)
            .Send(new NewExistingSession {
                message = "New client logged into your account."
            });
    }
    public void Logout()
    {
        PlayerData p = Auth.GetPlayer<PlayerData>();
        DiscordFacetService.SendLogoutMessageToDiscord(p);
        Auth.Logout();

    }
    public string GetDevBlog(){
       return DiscordFacetService.GetDevBlog();
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
    public PriceData GetPrice()
    {
        string usdToSolUrl = "http://23.88.54.33:3443/nft-price"; // Use a crypto price API
        decimal solPriceInUSD = decimal.Parse(Http.Get(usdToSolUrl)["data"].AsString);
        DateTime fetchedDate = DateTime.Parse(Http.Get(usdToSolUrl)["lastUpdate"].AsString);
        PriceData priceData = new PriceData(){
            price = solPriceInUSD,
            date = fetchedDate
        };
        return priceData;
    }
}