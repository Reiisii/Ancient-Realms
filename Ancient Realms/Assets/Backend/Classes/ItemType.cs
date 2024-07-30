using System;
using Unisave;


namespace ESDatabase.Classes
{
    [Serializable]
    public class ItemType
    {
        [Fillable] public int typeID {get;set;}
        [Fillable] public string typeName {get;set;}
    }
}

