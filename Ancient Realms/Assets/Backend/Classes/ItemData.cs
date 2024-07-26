using System;

namespace ESDatabase.Classes
{
    [Serializable]
    public class ItemData
    {
        public int id {get; set;}
        public string name {get; set;}
        public string imagePath {get; set;}
        public ItemType itemType {get;set;}
        public bool stackable {get; set;}
        public bool stack {get;set;}
        public bool isUsable {get; set;}

        public ItemData(int id, string name, string path, ItemType itemType, bool stackable, bool stack, bool isUsable){
            this.id = id;
            this.name = name;
            this.itemType.typeID = itemType.typeID;
            this.itemType.typeName = itemType.typeName;
            this.imagePath = path;
            this.stackable = stackable;
            this.stack = stack;
            this.isUsable = isUsable;
        }
    }
}

