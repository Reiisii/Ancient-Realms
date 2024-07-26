using System;
using System.Collections;
using System.Collections.Generic;
using Unisave;
using Unisave.Facets;
using Unisave.Facades;
using Unisave.Authentication.Middleware;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using ESDatabase.Classes;
public class AccountCreate : Facet
{
    public string CreateAccount(string pubkey)
    {
        string query = $"FOR p IN PlayerTable FILTER p.publicKey == '{pubkey}' RETURN p";
        
        var existingPlayer = DB.Query(query).FirstAs<PlayerEntity>();

        bool isExisting = CreateAccountAndUpdate(pubkey, existingPlayer);

        return isExisting == true ? "Existing Account! updating login date" : "New Account! Created Account Data";
    }
    public static bool CreateAccountAndUpdate(string pubkey, PlayerEntity existingPlayer)
    {
        
        if (existingPlayer != null)
        {
            existingPlayer.lastLoginAt = DateTime.UtcNow;
            existingPlayer.Save();
            return true;
        }
        else
        {
            GameData data = new GameData();
            var player = new PlayerEntity(pubkey, DateTime.UtcNow, data);
            player.Save();
            return false;
        }
    }
}