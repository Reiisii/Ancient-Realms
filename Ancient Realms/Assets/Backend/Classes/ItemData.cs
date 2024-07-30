using System;
using Unisave;

namespace ESDatabase.Classes
{
    [Serializable]
    public class ItemData
    {

        [Fillable] public int id {get; set;}
        [Fillable] public string name {get; set;}
        [Fillable] public string imagePath {get; set;}
        [Fillable] public ItemType itemType {get;set;}
        [Fillable] public bool stackable {get; set;}
        [Fillable] public bool stack {get;set;}
        [Fillable] public bool isUsable {get; set;}
        public ItemData(){

        }
    }
}

