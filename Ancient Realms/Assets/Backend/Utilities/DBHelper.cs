using System;
using System.Threading.Tasks;
using ESDatabase.Classes;
using ESDatabase.Entities;
using Unisave.Facades;

namespace ESDatabase.Utilities
{
    [Serializable]
    public class DBHelper
    {
        public static string IsPlayerExisting(string pubkey, PlayerData existingPlayer)
        {

            if (existingPlayer != null)
            {
                existingPlayer.lastLoginAt = DateTime.UtcNow;
                existingPlayer.Save();
                return existingPlayer.EntityId;
            }
            else
            {
                GameData data = new GameData();
                var player = new PlayerData(pubkey, DateTime.UtcNow, data);
                player.Save();
                return player.EntityId;
            }
        }
    }
}
