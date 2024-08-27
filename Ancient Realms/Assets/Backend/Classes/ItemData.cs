using System;
using Unisave;

namespace ESDatabase.Classes
{
    [Serializable]
    public class ItemData
    {

        [Fillable] public int equipmentId {get; set;}
        [Fillable] public int tier {get;set;}
        [Fillable] public int level {get;set;}
        [Fillable] public int stackAmount {get;set;}
        public ItemData(int eID, int t, int l, int amount){
            this.equipmentId = eID;
            this.tier = t;
            this.level = l;
            this.stackAmount = amount;
        }
    }
}

