using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Solana.Unity.Wallet;
using Solana.Unity.SDK.Nft;
using Unisave.Facets;
using Solana.Unity.SDK;
using UnityEngine.SceneManagement;
using Unisave.Facades;
using DG.Tweening;
using ESDatabase.Entities;
using System.Threading.Tasks;
using System;

public class AccountManager : MonoBehaviour
{

    [SerializeField]
    GameObject mainMenu;
    [SerializeField]
    GameObject connectionMenu;
    [SerializeField]
    GameObject logoutPanel;
    public static AccountManager Instance { get; private set;}
    public static PlayerData playerData;
    [SerializeField]
    public GameObject loadingPanel;
    public string EntityId;
    // Currently logged-in account
    private void Awake(){
        if (Instance == null)
        {
            // If not, set this as the instance and make it persistent
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Found more than one Account Manager in the scene");
            Destroy(gameObject);
        }

    }
    public async void Logout()
    {
        Instance.loadingPanel.SetActive(true);
        await FacetClient.CallFacet((DatabaseService facet) => facet.Logout())
        .Then(()=> 
        {
            AccountManager.Instance.gameObject.GetComponent<PlayerClient>().enabled = false;
            UIManager.DisableAllButtons(Instance.logoutPanel);
            Instance.logoutPanel.GetComponent<RectTransform>().DOAnchorPosY(735, 0.8f).SetEase(Ease.InOutSine).OnComplete(() => {
                    Instance.connectionMenu.SetActive(true);
                    Instance.loadingPanel.GetComponent<FadeAnimation>().Close();
            });
            Web3.Instance.Logout();
            Instance.EntityId = "";
            mainMenu.SetActive(false);
            connectionMenu.SetActive(true);
        })
        .Catch(error => 
        {
            Debug.LogError("Failed to clear session and logout: " + error);
            Instance.loadingPanel.GetComponent<FadeAnimation>().Close();
        });
    }
    public async static Task InitializeLogin(string pubkey)
    {
        await FacetClient.CallFacet((DatabaseService facet) => facet.InitializeLogin(pubkey))
        .Then(async response => 
        {
                    PlayerData playerData = await GetPlayerByPublicKey(pubkey);
                    Instance.EntityId = response;
                    Instance.mainMenu.SetActive(true);
                    UIManager.DisableAllButtons(Instance.connectionMenu);
                    Instance.connectionMenu.GetComponent<RectTransform>().DOAnchorPosY(-940, 0.8f).SetEase(Ease.InOutSine).OnComplete(() => {
                        Instance.connectionMenu.SetActive(false);
                        Instance.loadingPanel.GetComponent<FadeAnimation>().Close();
                    AccountManager.Instance.gameObject.GetComponent<PlayerClient>().enabled = true;
            });
        })
        .Catch(error => 
        {
            Debug.LogError("Failed to Initialize Account: " + error);
            Instance.loadingPanel.GetComponent<FadeAnimation>().Close();
        });
    }
    public async Task<PlayerData> GetPlayer()
    {
        PlayerData player = null;
        await FacetClient.CallFacet((DatabaseService facet) => facet.GetPlayerById(Instance.EntityId))
        .Then(response => 
        {
            Instance.loadingPanel.GetComponent<FadeAnimation>().Close();
            player = response;
        })
        .Catch(error => 
        {
            Debug.LogError("Failed to fetch player data: " + error);
            Instance.loadingPanel.GetComponent<FadeAnimation>().Close();
        });
        return player;
    }
    public async Task<PlayerData> GetPlayerData()
    {
        PlayerData player = null;
        await FacetClient.CallFacet((DatabaseService facet) => facet.GetPlayerById(Instance.EntityId))
        .Then(response => 
        {
            player = response;
        })
        .Catch(error => 
        {
            Debug.LogError("Failed to fetch player data: " + error);
        });
        return player;
    }
    public async static Task<PlayerData> GetPlayer(string id)
    {
        Instance.loadingPanel.SetActive(true);
        PlayerData player = null;
        await FacetClient.CallFacet((DatabaseService facet) => facet.GetPlayerById(id))
        .Then(response => 
        {
            Instance.loadingPanel.GetComponent<FadeAnimation>().Close();
            player = response;
        })
        .Catch(error => 
        {
            Debug.LogError("Failed to fetch player data: " + error);
            Instance.loadingPanel.GetComponent<FadeAnimation>().Close();
        });
        return player;
    }
    public async static Task<PlayerData> GetPlayerByPublicKey(string id)
    {
        Instance.loadingPanel.SetActive(true);
        PlayerData player = null;
        await FacetClient.CallFacet((DatabaseService facet) => facet.GetPlayerByPublicKey(id))
        .Then(response => 
        {
            Instance.loadingPanel.GetComponent<FadeAnimation>().Close();
            player = response;
        })
        .Catch(error => 
        {
            Debug.LogError("Failed to fetch player data: " + error);
            Instance.loadingPanel.GetComponent<FadeAnimation>().Close();
        });
        return player;
    }
    public async static Task SaveData(PlayerData player)
    {

        await FacetClient.CallFacet((DatabaseService facet) => facet.SaveData(player))
        .Then(response =>
        {
            
            Debug.Log(response);
        })
        .Catch(error =>
        {
            Debug.LogError("Failed to save player data: " + error);
        });
    }

}