using System;
using System.Collections.Generic;
using Unisave;
namespace ESDatabase.Classes
{
    [Serializable]
    public class QuestData{
        [Fillable] public string questID {get;set;}
        [Fillable] public bool isActive {get;set;}
        [Fillable] public bool completed {get;set;}
        [Fillable] public bool isRewarded {get;set;}
        [Fillable] public int currentGoal {get;set;}
        [Fillable] public string currentKnot {get;set;}
        [Fillable] public List<GoalData> goals {get;set;}
        public QuestData() {}
    }
}
