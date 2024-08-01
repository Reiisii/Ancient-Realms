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
public class DatabaseService : Facet
{
    public string InitializeLogin(string pubkey)
    {
        List<PlayerData> playerList = DB.TakeAll<PlayerData>().Get();
        PlayerData player = playerList.FirstOrDefault(data => data.publicKey == pubkey);
        string isExisting = DBHelper.IsPlayerExisting(pubkey, player);
        return isExisting;
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