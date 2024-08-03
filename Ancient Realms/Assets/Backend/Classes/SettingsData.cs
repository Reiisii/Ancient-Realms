using System;
using Unisave;

namespace ESDatabase.Classes
{
    [Serializable]
    public class SettingsData
    {
        [Fillable] public float masterVolume { get; set; }
        [Fillable] public float musicVolume { get; set; }
        [Fillable] public float soundFXVolume { get; set; } 

        public SettingsData()
        {
            masterVolume = 100;
            musicVolume = 100;
            soundFXVolume = 100; 
        }
    }
}
