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
                public int maxXP {get; set;}
                public int currentXP {get; set;}
                public int maxEnergy {get;set;}
                public string lastLocationVisited {get;set;}
                public float lastX {get;set;}
                public float lastY {get;set;}
                public bool isInterior {get;set;}
                public int currentEnergy {get;set;}
                public Inventory inventory {get;set;}
                public EquippedData equippedData {get;set;}
                public List<QuestData> quests {get; set;}
                public List<ArtifactsData> artifacts {get;set;}
                public SettingsData settings {get;set;}
                public GameData(){
                        this.playerName = "Unnamed Legionnaire";
                        this.denarii = 0;
                        this.level = 0;
                        this.maxXP = 30;
                        this.currentXP = 0;
                        this.maxEnergy = 90;
                        this.currentEnergy = 90;
                        this.lastLocationVisited = "Training Grounds";
                        this.lastX = 1.46f;
                        this.lastY = -2.96f;
                        this.isInterior = false;
                        this.rank = "Tiro";
                        this.inventory = new Inventory();
                        this.equippedData = new EquippedData();
                        this.quests = new List<QuestData>();
                        this.artifacts = new List<ArtifactsData>();
                        this.settings = new SettingsData();
                }
        }
}