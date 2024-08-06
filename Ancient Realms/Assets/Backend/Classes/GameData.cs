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
                public int maxEnergy {get;set;}
                public string lastLocationVisited {get;set;}
                public string lastDistrict {get;set;}
                public int currentEnergy {get;set;}
                public Inventory inventory {get;set;}
                public List<QuestData> quests {get; set;}
                public List<ArtifactsData> artifacts {get;set;}
                public SettingsData settings {get;set;}
                public GameData(){
                        this.playerName = "Unnamed Legionnaire";
                        this.denarii = 0;
                        this.level = 0;
                        this.xp = 0;
                        this.maxEnergy = 90;
                        this.currentEnergy = 90;
                        this.lastLocationVisited = "";
                        this.lastDistrict = "Main";
                        this.rank = "Tiro";
                        this.inventory = new Inventory();
                        this.quests = new List<QuestData>();
                        this.artifacts = new List<ArtifactsData>();
                        this.settings = new SettingsData();
                }
        }
}