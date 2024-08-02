using System.Collections;
using System.Collections.Generic;
using ESDatabase.Entities;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
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
    public AudioMixer audioMixer;
    public float masterVolume, oldMasterVolume;
    public float musicVolume, oldMusicVolume;

    void Start(){
        oldMusicVolume = musicSlider.value;
        oldMasterVolume = masterSlider.value;
    }
    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
    }
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
    }
    public async void SaveSettings()
    {
        PlayerData playerData = await AccountManager.GetPlayer();

        // Modify the entity
        if(oldMasterVolume != masterVolume) playerData.gameData.settings.masterVolume = masterVolume;
        if(oldMusicVolume != musicVolume) playerData.gameData.settings.musicVolume = musicVolume;
        // Save the modified entity
        if(oldMasterVolume != masterVolume || oldMusicVolume != musicVolume) await AccountManager.SaveData(playerData);
        oldMasterVolume = masterVolume;
        oldMusicVolume = musicVolume;
        settingsPanel.Close();
        mainMenuPanel.SetActive(true);
        audioMixer.SetFloat("volume", masterVolume);
    }
    public void CheckChanges()
    {
        bool changed = false;
        if(oldMasterVolume != masterVolume) changed = true;
        if(oldMusicVolume != musicVolume) changed = true;
        
        if(changed == true){
            confirmationPanel.SetActive(true);
            settingsPanel.Close();
        }else{
            settingsPanel.Close();
            mainMenuPanel.SetActive(true);
        }
    }
    public void Reset(){
        musicSlider.value = oldMusicVolume;
        masterSlider.value = oldMasterVolume;
    }
    public void Retain(){
        musicSlider.value = musicVolume;
        masterSlider.value = masterVolume;
    }
}
