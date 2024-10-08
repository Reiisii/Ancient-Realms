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
using ESDatabase.Classes;
using System.Linq;

public class AccountManager : MonoBehaviour
{

    [SerializeField]
    GameObject mainMenu;
    [SerializeField]
    GameObject connectionMenu;
    [SerializeField]
    GameObject logoutPanel;
    public static AccountManager Instance { get; private set;}
    public PlayerData playerData;
    [SerializeField] public GameObject loadingPanel;
    [SerializeField] public GameObject updateGO;
    [Header("Assets")]
    public List<QuestSO> quests;
    public List<ArtifactsSO> achievements;
    public List<EquipmentSO> equipments;
    public List<NFTSO> nfts;
    public string EntityId;
    public string UIDInstance;
    public PriceData priceData; 
    // Currently logged-in account
    private void Awake(){
        if (Instance == null)
        {
            quests = Resources.LoadAll<QuestSO>("QuestSO").ToList();
            achievements = Resources.LoadAll<ArtifactsSO>("ArtifactSO").ToList();
            equipments = Resources.LoadAll<EquipmentSO>("EquipmentSO").ToList();
            nfts = Resources.LoadAll<NFTSO>("NFTSO").ToList();
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
            Instance.UIDInstance = "";
            mainMenu.SetActive(false);
            connectionMenu.SetActive(true);
        })
        .Catch(error => 
        {
            Debug.LogError("Failed to clear session and logout: " + error);
            Instance.loadingPanel.GetComponent<FadeAnimation>().Close();
        });
    }
    public async void ForceLogout()
    {
        Instance.loadingPanel.SetActive(true);
        await FacetClient.CallFacet((DatabaseService facet) => facet.Logout())
        .Then(()=> 
        {
            AccountManager.Instance.gameObject.GetComponent<PlayerClient>().enabled = false;
            Web3.Instance.Logout();
            Instance.EntityId = "";
            Instance.UIDInstance = "";
            UIManager.DisableAllButtons(Instance.mainMenu);
            Instance.mainMenu.GetComponent<MainMenuAnimation>().Close();
            Instance.mainMenu.GetComponent<UpdatePanelAnimation>().Close();
            Instance.mainMenu.GetComponent<LogoAnimation>().Close();
            connectionMenu.SetActive(true);
            Instance.loadingPanel.GetComponent<FadeAnimation>().Close();
        })
        .Catch(error => 
        {
            Debug.LogError("Failed to clear session and logout: " + error);
            Instance.loadingPanel.GetComponent<FadeAnimation>().Close();
        });
    }
    public async Task InitializeLogin(string pubkey)
    {
        string generatedUID = Utilities.GenerateUuid();
        await FacetClient.CallFacet((DatabaseService facet) => facet.InitializeLogin(pubkey, generatedUID))
        .Then(response => 
        {
                    playerData = response;
                    Instance.EntityId = response.EntityId;
                    Instance.UIDInstance = generatedUID;
                    Instance.gameObject.GetComponent<PlayerClient>().enabled = true;
                    Instance.mainMenu.SetActive(true);
                    UIManager.DisableAllButtons(Instance.connectionMenu);
                    Instance.connectionMenu.GetComponent<RectTransform>().DOAnchorPosY(-940, 0.8f).SetEase(Ease.InOutSine).OnComplete(() => {
                    Instance.connectionMenu.SetActive(false);
                    Instance.loadingPanel.GetComponent<FadeAnimation>().Close();
                    FacetClient.CallFacet((DatabaseService facet) => facet.GetDevBlog())
                    .Then(response => 
                    {
                        Instance.updateGO.GetComponent<DevBlog>().ProcessRSS(response);
                    });
                    if(priceData.date != null && Utilities.CheckIfLateBy10Minutes(priceData.date)){
                        FacetClient.CallFacet((SolanaExchangeService facet) => facet.GetPrice())
                        .Then(response => 
                        {
                            priceData.price = response.price;
                            priceData.date = response.date;
                            Utilities.CheckIfLateBy10Minutes(priceData.date);
                        })
                        .Catch(error => 
                        {
                            Debug.LogError("Failed to fetch Price: " + error);
                        });
                    }else if(priceData.date == null){
                        FacetClient.CallFacet((SolanaExchangeService facet) => facet.GetPrice())
                        .Then(response => 
                        {
                            priceData = response;
                        })
                        .Catch(error => 
                        {
                            Debug.LogError("Failed to fetch Price: " + error);
                        });
                    }
                    
            });
        })
        .Catch(error => 
        {
            Debug.LogError("Failed to Initialize Account: " + error);
            Instance.loadingPanel.GetComponent<FadeAnimation>().Close();
        });
        
    }
    public void GetPrice(){
        FacetClient.CallFacet((SolanaExchangeService facet) => facet.GetPrice())
                        .Then(response => 
                        {
                            priceData.price = response.price;
                            priceData.date = response.date;
                            MintingManager.GetInstance().isFetching = false;
                        })
                        .Catch(error => 
                        {
                            Debug.LogError("Failed to fetch Price: " + error);
                        });
    }
    public async void CheckSession(string message){
        PlayerData newPlayer = await GetPlayer();
        PlayerData oldPlayer = playerData;

        if(oldPlayer.token.Equals(newPlayer.token)){
            Debug.Log("New login but you are safe");
        }else{
            ForceLogout();
            Debug.Log(message);
        }
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