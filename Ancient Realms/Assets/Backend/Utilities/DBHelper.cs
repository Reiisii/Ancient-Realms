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
        public static bool IsPlayerLoggedOn(string sessionKey)
        {
            string query = $"FOR s IN u_sessions FILTER s.sessionData.authenticatedPlayerId = '{sessionKey}' RETURN s";
            var existingPlayer = DB.Query(query).FirstAs<PlayerData>();
            if(existingPlayer != null){
                return true;
            }else{
                return false;
            }
        }
        public static void SetSession(string sessionKey, string entityID)
        {
            if(!Session.Has(sessionKey)){
                Session.Set("authenticatedPlayer", entityID);
            }
        }
        public static void Forgot(string sessionKey)
        {
            if(Session.Has(sessionKey)){
                Session.Forget(sessionKey);
            }
        }
    }
}
