using System;
using System.Collections.Generic;
using Unisave;
namespace ESDatabase.Classes
{
    [Serializable]
    public class Inventory
    {
        
        [Fillable] public int slots {get;set;}
        [Fillable] public List<ItemData> items {get; set;}
        public Inventory(){
            this.slots = 20;
            items = new List<ItemData>();
        }
    }
}
