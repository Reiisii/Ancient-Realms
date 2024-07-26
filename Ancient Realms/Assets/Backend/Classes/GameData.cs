using System;
using System.Collections.Generic;

namespace ESDatabase.Classes
{
        [Serializable]
        public class GameData{
                public string playerName {get; set;}
                public int denarii {get; set;}
                public int level { get; set;}
                public string rank { get; set;}
                public int xp {get; set;}
                public Inventory inventory;
                public GameData(){
                        this.playerName = "Unnamed Legionnaire";
                        this.denarii = 0;
                        this.level = 0;
                        this.xp = 0;
                        this.rank = "Tiro";
                        this.inventory = new Inventory(15, new List<ItemData>());
                }
                // Add custom fields to the entity:
                //
                //      public string nickname;
                //      public int coins = 1_000;
                //      public DateTime premiumUntil = DateTime.UtcNow;
                //      public DateTime bannedUntil = DateTime.UtcNow;
                //
        }
}