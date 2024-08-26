using System;
using ESDatabase.Classes;
using Unisave;
using Unisave.Entities;
namespace ESDatabase.Entities
{
    [EntityCollectionName("PlayerTable")]
    public class PlayerData : Entity
    {
        public string publicKey {get; set;}
        public DateTime lastLoginAt = DateTime.UtcNow;
        public string token {get;set;}
        [Fillable] public GameData gameData {get; set;}
        public PlayerData() { }
        public PlayerData(string pubKey, DateTime loginDate, GameData data){
            this.publicKey = pubKey;
            this.lastLoginAt = loginDate;
            this.gameData = data;
        }
    }
}