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
    [SerializeField]
    GameObject popPanel;
    [SerializeField]
    GameObject compliancePanel;
    [SerializeField]
    Button link;
    [SerializeField]
    Button accept;
    [SerializeField]
    Toggle toggle;
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
        AccountManager.Instance.loadingPanel.SetActive(true);

        bool notExisting = await AccountManager.Instance.NotExisting(account.PublicKey.ToString());
        if(notExisting){
            connectionMenu.GetComponent<RectTransform>().DOAnchorPosY(-940, 0.8f).SetEase(Ease.InOutSine).OnComplete(() => {
                connectionMenu.SetActive(false);
            });
            AccountManager.Instance.loadingPanel.GetComponent<FadeAnimation>().Close();
            compliancePanel.SetActive(true);
            toggle.interactable = false;
            UIManager.DisableAllButtons(compliancePanel);
            compliancePanel.GetComponent<RectTransform>().DOAnchorPosY(0f, 0.8f).SetEase(Ease.InOutSine).OnComplete(() => {
                link.interactable = true;
                toggle.interactable = true;
            });
        }else{
            InitializeLogin(account);
        }
        
        // await AccountManager.Instance.InitializeLogin(account.PublicKey.ToString());
        // PlayerData playerData = AccountManager.Instance.playerData;
        // if(playerData != null){
        //     masterSlider.value = Mathf.Clamp(playerData.gameData.settings.masterVolume, masterSlider.minValue, masterSlider.maxValue);
        //     musicSlider.value = Mathf.Clamp(playerData.gameData.settings.musicVolume, musicSlider.minValue, musicSlider.maxValue);
        //     soundFXSlider.value = Mathf.Clamp(playerData.gameData.settings.soundFXVolume, soundFXSlider.minValue, soundFXSlider.maxValue);
        // }
        // AccountManager.Instance.loadingPanel.GetComponent<FadeAnimation>().Close();
        // UIManager.EnableAllButtons(connectionMenu);
        // Web3.Instance.Logout();
        // AccountManager.Instance.EntityId = "";
        // popPanel.SetActive(true);   
    }
    public async void CreateAccountLogin(){
        AccountManager.Instance.loadingPanel.SetActive(true);
        await AccountManager.Instance.InitializeLogin(Web3.Wallet.Account.PublicKey.ToString());
        compliancePanel.GetComponent<RectTransform>().DOAnchorPosY(-1124f, 0.8f).SetEase(Ease.InOutSine).OnComplete(() => {
                compliancePanel.SetActive(false);
        });
    }
    public async void InitializeLogin(string pubkey){

        await AccountManager.Instance.InitializeLogin(pubkey);
        PlayerData playerData = AccountManager.Instance.playerData;
        if(playerData != null){
            masterSlider.value = Mathf.Clamp(playerData.gameData.settings.masterVolume, masterSlider.minValue, masterSlider.maxValue);
            musicSlider.value = Mathf.Clamp(playerData.gameData.settings.musicVolume, musicSlider.minValue, musicSlider.maxValue);
            soundFXSlider.value = Mathf.Clamp(playerData.gameData.settings.soundFXVolume, soundFXSlider.minValue, soundFXSlider.maxValue);
        }

    }
    public async void InitializeLogin(Account account){
        await AccountManager.Instance.InitializeLogin(account.PublicKey.ToString());
        PlayerData playerData = AccountManager.Instance.playerData;
        if(playerData != null){
            masterSlider.value = Mathf.Clamp(playerData.gameData.settings.masterVolume, masterSlider.minValue, masterSlider.maxValue);
            musicSlider.value = Mathf.Clamp(playerData.gameData.settings.musicVolume, musicSlider.minValue, musicSlider.maxValue);
            soundFXSlider.value = Mathf.Clamp(playerData.gameData.settings.soundFXVolume, soundFXSlider.minValue, soundFXSlider.maxValue);
        }
    }
    private void OnLogout(){
        Debug.Log("Logout");
    }
}