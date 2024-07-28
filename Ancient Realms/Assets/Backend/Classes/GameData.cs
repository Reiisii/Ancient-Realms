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
                public Inventory inventory {get;set;}
                public List<QuestData> quests {get; set;}
                public SettingsData settings {get;set;}
                public GameData(){
                        this.playerName = "Unnamed Legionnaire";
                        this.denarii = 0;
                        this.level = 0;
                        this.xp = 0;
                        this.rank = "Tiro";
                        this.inventory = new Inventory();
                        this.quests = new List<QuestData>();
                        this.settings = new SettingsData();
                }
        }
}