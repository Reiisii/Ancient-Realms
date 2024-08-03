using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ESDatabase.Entities;
using Solana.Unity.SDK;
using Solana.Unity.SDK.Nft;
using Solana.Unity.Wallet;
using Unisave.Facets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class AccountStore : MonoBehaviour
{
    [SerializeField]
    GameObject mainMenu;

    [SerializeField]
    GameObject connectionMenu;
    [Header("Slider")]
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundFXSlider;
    
    private void OnEnable(){
        Web3.OnLogin += OnLogin;
        Web3.OnLogout += OnLogout;
    }
    private void OnDisable(){
        Web3.OnLogin -= OnLogin;
        Web3.OnLogout -= OnLogout;
    }
    private async void OnLogin(Account account){
        AccountManager.InitializeLogin(account.PublicKey.ToString());
        PlayerData playerData = await AccountManager.GetPlayerByPublicKey(account.PublicKey.ToString());
        masterSlider.value = Mathf.Clamp(playerData.gameData.settings.masterVolume, masterSlider.minValue, masterSlider.maxValue);
        musicSlider.value = Mathf.Clamp(playerData.gameData.settings.musicVolume, musicSlider.minValue, musicSlider.maxValue);
        soundFXSlider.value = Mathf.Clamp(playerData.gameData.settings.soundFXVolume, soundFXSlider.minValue, soundFXSlider.maxValue);
    }
    private void OnLogout(){
        Debug.Log("Logout");
    }
}