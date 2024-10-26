using System;
using Unisave;

namespace ESDatabase.Classes
{
    [Serializable]
    public class StatisticsData
    {
        [Fillable] public int kills { get; set; }
        [Fillable] public int denariiTotal { get; set; }
        [Fillable] public int smithingTotal { get; set; } 
        [Fillable] public int mintingTotal { get; set; } 
        [Fillable] public int deathTotal { get; set; }
        [Fillable] public float moveDistanceTotal { get; set; }
        public StatisticsData()
        {
            kills = 0;
            denariiTotal = 0;
            smithingTotal = 0;
            mintingTotal = 0;
            deathTotal = 0;
            moveDistanceTotal = 0;
        }
    }
}
