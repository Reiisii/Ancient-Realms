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

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
    }

    public void SetSoundFXVolume(float volume)
    {
        soundFXVolume = volume;
    }

    public async void SaveSettings()
    {
        PlayerData playerData = await AccountManager.GetPlayer();

        
        if (oldMasterVolume != masterVolume) playerData.gameData.settings.masterVolume = masterVolume;
        if (oldMusicVolume != musicVolume) playerData.gameData.settings.musicVolume = musicVolume;
        if (oldSoundFXVolume != soundFXVolume) playerData.gameData.settings.soundFXVolume = soundFXVolume; 

        
        if (oldMasterVolume != masterVolume || oldMusicVolume != musicVolume || oldSoundFXVolume != soundFXVolume)
        {
            await AccountManager.SaveData(playerData);
            AudioManager.GetInstance().SetSoundFXVolume(soundFXVolume);
            AudioManager.GetInstance().SetMusicVolume(musicVolume);
            AudioManager.GetInstance().SetMasterVolume(masterVolume);
        }

        oldMasterVolume = masterVolume;
        oldMusicVolume = musicVolume;
        oldSoundFXVolume = soundFXVolume; 

        settingsPanel.Close();
        mainMenuPanel.SetActive(true);

    }
    public void CheckChanges()
    {
        bool changed = false;
        if (oldMasterVolume != masterVolume) changed = true;
        if (oldMusicVolume != musicVolume) changed = true;
        if (oldSoundFXVolume != soundFXVolume) changed = true; 

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
        musicSlider.value = musicVolume;
        masterSlider.value = masterVolume;
        soundFXSlider.value = soundFXVolume; 
    }
}