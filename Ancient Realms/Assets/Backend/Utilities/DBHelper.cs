using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ESDatabase.Classes;
using ESDatabase.Entities;
using Unisave.Facades;

namespace ESDatabase.Utilities
{
    [Serializable]
    public class DBHelper
    {
        public static bool IsPlayerExisting(string pubkey, PlayerData existingPlayer)
        {

            if (existingPlayer != null)
            {
                existingPlayer.lastLoginAt = DateTime.UtcNow;
                existingPlayer.Save();
                return false;
            }
            else
            {
                GameData data = new GameData();
                var player = new PlayerData(pubkey, DateTime.UtcNow, data);
                player.Save();
                return false;
            }
        }
    }
}
