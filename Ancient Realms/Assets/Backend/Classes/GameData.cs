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
                public bool cutscenePlayed {get;set;}
                public int currentEnergy {get;set;}
                public StatisticsData statistics {get;set;}
                public Inventory inventory {get;set;}
                public EquippedData equippedData {get;set;}
                public List<NFTData> equippedNFT;
                public List<QuestData> quests {get; set;}
                public List<ArtifactsData> artifacts {get;set;}
                public List<int> characters {get;set;}
                public List<int> equipments {get;set;}
                public List<int> events {get;set;}
                public List<string> uiSettings {get;set;}
                public List<string> premiumPurchases {get;set;}
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
                        this.cutscenePlayed = false;
                        this.rank = "Tiro";
                        this.statistics = new StatisticsData();
                        this.inventory = new Inventory();
                        this.equippedData = new EquippedData();
                        this.equippedNFT = new List<NFTData>{ null, null, null};
                        this.quests = new List<QuestData>();
                        this.artifacts = new List<ArtifactsData>();
                        this.characters = new List<int>();
                        this.equipments = new List<int> { 0, 5, 23, 18, 11, 8, 12 };
                        this.uiSettings = new List<string>();
                        this.events = new List<int>();
                        this.premiumPurchases = new List<string>();
                        this.settings = new SettingsData();
                }
        }
}