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
    public static AccountManager Instance { get; private set;}
    [SerializeField]
    GameObject loadingPanel;
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
    public void Logout()
    {
        Web3.Instance.Logout();
        Instance.EntityId = "";
        Debug.Log("AccountManager instance cleared");
        mainMenu.SetActive(false);
        connectionMenu.SetActive(true);
    }
    public async static void InitializeLogin(string pubkey)
    {
        Instance.loadingPanel.SetActive(true);
        await FacetClient.CallFacet((DatabaseService facet) => facet.InitializeLogin(pubkey))
        .Then(response => 
        {
            Instance.EntityId = response;
            Instance.mainMenu.SetActive(true);
            UIManager.DisableAllButtons(Instance.connectionMenu);
            Instance.connectionMenu.GetComponent<RectTransform>().DOAnchorPosY(-940, 0.8f).SetEase(Ease.InOutSine).OnComplete(() => {
                Instance.connectionMenu.SetActive(false);
                Instance.loadingPanel.GetComponent<FadeAnimation>().Close();
            });
            
        })
        .Catch(error => 
        {
            Debug.LogError("Failed to create or update account: " + error);
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