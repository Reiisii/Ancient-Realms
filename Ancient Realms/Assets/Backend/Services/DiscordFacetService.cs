using System;
using System.Collections;
using System.Collections.Generic;
using Unisave;
using Unisave.Facets;
using Unisave.Facades;
using Unisave.Authentication.Middleware;
using ESDatabase.Entities;
namespace ESDatabase.Services
{
    public class DiscordFacetService : Facet
    {
        public static void SendLoginMessageToDiscord(PlayerData player)
        {
            Http.Post("https://discord.com/api/webhooks/1277608369246703616/ikus8_3zo6jG5qUQEh9psQM6ISEOq7xLnLtN04IqH7uh0k1WldDRyfzlLoLnTakDJWYU", new Dictionary<string, string>()
            {
                ["username"] = "Eagles Shadow",
                ["content"] = "> 📥 **[" + player.gameData.playerName + "]:** just logged in the game server 😆\n> **[TOKEN]:** " + player.token
            });
        }
        public static void SendLogoutMessageToDiscord(PlayerData player)
        {
            Http.Post("https://discord.com/api/webhooks/1277608369246703616/ikus8_3zo6jG5qUQEh9psQM6ISEOq7xLnLtN04IqH7uh0k1WldDRyfzlLoLnTakDJWYU", new Dictionary<string, string>()
            {
                ["username"] = "Eagles Shadow",
                ["content"] ="> 📤 **[" + player.gameData.playerName + "]:** Bro just logged off the game server 💀\n> **[TOKEN]:** " + player.token
            });
        }
    }
}