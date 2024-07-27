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
    private void Awake()
    {
        // Ensure only one instance of AccountManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
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
            });
            Instance.loadingPanel.GetComponent<FadeAnimation>().Close();
            
        })
        .Catch(error => 
        {
            Debug.LogError("Failed to create or update account: " + error);
            Instance.loadingPanel.GetComponent<FadeAnimation>().Close();
        });
    }
    public async static void GetPlayer(string id)
    {
        Instance.loadingPanel.SetActive(true);
        await FacetClient.CallFacet((DatabaseService facet) => facet.GetPlayerById(id))
        .Then(response => 
        {
            Debug.Log("[Public Key]: " + response.publicKey);
            Debug.Log("[Player Name]: " + response.gameData.playerName);
            Debug.Log("[Denarii]: " + response.gameData.denarii);
            Debug.Log("[Level]: " + response.gameData.level);
            Debug.Log("[Xp]: " + response.gameData.xp);
            Debug.Log("[Inventory Slots]: " + response.gameData.inventory.slots);
            Instance.loadingPanel.GetComponent<FadeAnimation>().Close();
        })
        .Catch(error => 
        {
            Debug.LogError("Failed to fetch player data: " + error);
            Instance.loadingPanel.GetComponent<FadeAnimation>().Close();
        });
    }
}