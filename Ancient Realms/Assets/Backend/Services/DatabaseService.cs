using System;
using System.Collections;
using System.Collections.Generic;
using Unisave;
using Unisave.Facets;
using Unisave.Facades;
using ESDatabase.Utilities;
using System.Threading.Tasks;
using ESDatabase.Entities;
public class DatabaseService : Facet
{
    public string InitializeLogin(string pubkey)
    {
        string query = $"FOR p IN PlayerTable FILTER p.publicKey == '{pubkey}' RETURN p";

        var existingPlayer = DB.Query(query).FirstAs<PlayerData>();

        bool isExisting = DBHelper.IsPlayerExisting(pubkey, existingPlayer);

        return isExisting == true ? existingPlayer.EntityId : existingPlayer.EntityId;
    }
    public PlayerData GetPlayerById(string entityID)
    {
        PlayerData player = DB.Find<PlayerData>(entityID);

        return player;
    }
}