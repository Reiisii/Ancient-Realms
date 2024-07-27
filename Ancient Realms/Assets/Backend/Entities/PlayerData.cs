using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using ESDatabase.Classes;
using Unisave;
using Unisave.Entities;
using Unisave.Facades;

namespace ESDatabase.Entities
{
    [EntityCollectionName("PlayerTable")]
    public class PlayerData : Entity
    {
        public string publicKey {get; set;}
        public DateTime lastLoginAt = DateTime.UtcNow;
        public GameData gameData {get; set;}
        public PlayerData() { }
        public PlayerData(string pubKey, DateTime loginDate, GameData data){
            this.publicKey = pubKey;
            this.lastLoginAt = loginDate;
            this.gameData = data;
        }
    }
}