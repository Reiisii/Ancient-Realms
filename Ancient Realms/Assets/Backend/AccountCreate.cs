using System;
using System.Collections;
using System.Collections.Generic;
using Unisave;
using Unisave.Facets;
using Unisave.Facades;
using Unisave.Authentication.Middleware;
using System.Security.Cryptography.X509Certificates;

public class AccountCreate : Facet
{
    public string CreateAccount(string pubkey)
    {
        string query = $"FOR p IN PlayerTable FILTER p.publicKey == '{pubkey}' RETURN p";
        
        var existingPlayer = DB.Query(query).FirstAs<PlayerEntity>();


        if (existingPlayer != null)
        {
            existingPlayer.lastLoginAt = DateTime.UtcNow;
            existingPlayer.Save();
            return existingPlayer.EntityId;
        }
        else
        {
            var player = new PlayerEntity
            {
                publicKey = pubkey,
                lastLoginAt = DateTime.UtcNow
            };
            player.Save();
            return player.EntityId;
        }
    }

}