using System.Collections;
using System.Collections.Generic;
using ESDatabase.Entities;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    // Audio Mixer
    [Header("Panel")]
    [SerializeField] SettingsAnimation settingsPanel;
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject confirmationPanel;

    [Header("Slider")]
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundFXSlider; 

    public float masterVolume, oldMasterVolume;
    public float musicVolume, oldMusicVolume;
    public float soundFXVolume, oldSoundFXVolume; 

    void Start(){
        oldMusicVolume = musicSlider.value;
        oldMasterVolume = masterSlider.value;
        oldSoundFXVolume = soundFXSlider.value; 
    }

    public async void SaveSettings()
    {
        PlayerData playerData = await AccountManager.Instance.GetPlayer();

        
        playerData.gameData.settings.masterVolume = masterSlider.value;
        playerData.gameData.settings.musicVolume = musicSlider.value;
        playerData.gameData.settings.soundFXVolume = soundFXSlider.value; 

        
        if (oldMasterVolume != masterVolume || oldMusicVolume != musicVolume || oldSoundFXVolume != soundFXVolume)
        {
            await AccountManager.SaveData(playerData);
            AudioManager.GetInstance().SetSoundFXVolume(soundFXVolume);
            AudioManager.GetInstance().SetMusicVolume(musicVolume);
            AudioManager.GetInstance().SetMasterVolume(masterVolume);
            oldMasterVolume = masterSlider.value;
            oldMusicVolume = musicSlider.value;
            oldSoundFXVolume = soundFXSlider.value;
            settingsPanel.Close();
            mainMenuPanel.SetActive(true);
        }
    }
    public void CheckChanges()
    {
        bool changed = false;
        if (oldMasterVolume != masterSlider.value) changed = true;
        if (oldMusicVolume != musicSlider.value) changed = true;
        if (oldSoundFXVolume != soundFXSlider.value) changed = true; 

        if (changed == true)
        {
            confirmationPanel.SetActive(true);
            settingsPanel.Close();
        }
        else
        {
            settingsPanel.Close();
            mainMenuPanel.SetActive(true);
        }
    }

    public void Reset(){
        musicSlider.value = oldMusicVolume;
        masterSlider.value = oldMasterVolume;
        soundFXSlider.value = oldSoundFXVolume;
    }
    public void Retain(){
        musicSlider.value = musicSlider.value;
        masterSlider.value = masterSlider.value;
        soundFXSlider.value = soundFXSlider.value; 
    }
}