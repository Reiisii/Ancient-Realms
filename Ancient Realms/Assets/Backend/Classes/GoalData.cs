using System;
using System.Collections.Generic;
using Unisave;
namespace ESDatabase.Classes
{
    [Serializable]
    public class GoalData
    {
        [Fillable] public int goalID {get;set;}
        [Fillable] public int requiredAmount {get;set;}
        [Fillable] public int currentAmount {get;set;}
        public GoalData(){
            
        }
        public GoalData(int id, int requiredAmount, int currentAmount){
            this.goalID = id;
            this.requiredAmount = requiredAmount;
            this.currentAmount = currentAmount;
        }
    }
}