using System;
using System.Collections.Generic;
using Unisave;
namespace ESDatabase.Classes
{
    [Serializable]
    public class EquippedData
    {
        
        [Fillable] public ItemData helmSlot {get;set;}
        [Fillable] public ItemData chestSlot {get;set;}
        [Fillable] public ItemData waistSlot {get;set;}
        [Fillable] public ItemData footSlot {get;set;}
        [Fillable] public ItemData mainSlot {get;set;}
        [Fillable] public ItemData shieldSlot {get;set;}
        [Fillable] public ItemData javelinSlot {get;set;}
        [Fillable] public ItemData bandageSlot {get;set;}
        public EquippedData(){
            this.helmSlot = new ItemData(0, 1, 0, 1);
            this.chestSlot = new ItemData(5, 1, 0, 1);
            this.waistSlot = new ItemData(23, 1, 0, 1);
            this.footSlot = new ItemData(18, 1, 0, 1);
            this.mainSlot = new ItemData(11, 1, 0, 1);
            this.shieldSlot = new ItemData(8, 1, 0, 1);
            this.javelinSlot = new ItemData(12, 1, 0, 1);
            this.bandageSlot = null;
        }
    }
}
