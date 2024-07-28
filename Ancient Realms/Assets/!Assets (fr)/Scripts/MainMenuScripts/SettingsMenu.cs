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
    [Header("Slider")]
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    public AudioMixer audioMixer;
    public float masterVolume, oldMasterVolume;
    public float musicVolume, oldMusicVolume;
    private async void OnEnable(){
        PlayerData playerData = await AccountManager.GetPlayer();
        oldMasterVolume = playerData.gameData.settings.masterVolume;
        oldMusicVolume = playerData.gameData.settings.musicVolume;
        masterSlider.value = Mathf.Clamp(playerData.gameData.settings.masterVolume, masterSlider.minValue, masterSlider.maxValue);
        musicSlider.value = Mathf.Clamp(playerData.gameData.settings.musicVolume, musicSlider.minValue, musicSlider.maxValue);
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
        settingsPanel.Close();
        mainMenuPanel.SetActive(true);
        audioMixer.SetFloat("volume", masterVolume);
    }
    public async void CheckChanges()
    {
        bool changed = false;
        if(oldMasterVolume != masterVolume) changed = true;
        if(oldMusicVolume != musicVolume) changed = true;
        
        if(changed == true){
            Debug.Log("Are you sure?");
        }else{
            settingsPanel.Close();
            mainMenuPanel.SetActive(true);
        }
    }
}
