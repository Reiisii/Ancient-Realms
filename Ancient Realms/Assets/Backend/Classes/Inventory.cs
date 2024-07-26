using System;
using System.Collections.Generic;
namespace ESDatabase.Classes
{
    [Serializable]
    public class Inventory
    {
        public int slots {get;set;}
        public List<ItemData> items {get; set;}

        public Inventory(int slotCount, List<ItemData> items){
            this.slots = slotCount;
            this.items = items;
        }
    }
}
