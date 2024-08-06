using System;
using Unisave;


namespace ESDatabase.Classes
{
    [Serializable]
    public class ArtifactsData
    {
        [Fillable] public int artifactID {get;set;}
        [Fillable] public DateTime acquiredDate {get;set;}
    }
}

