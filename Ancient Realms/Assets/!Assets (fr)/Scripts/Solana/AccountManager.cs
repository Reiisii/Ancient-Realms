using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Solana.Unity.Wallet;
using Solana.Unity.SDK.Nft;
using Unisave.Facets;
using Solana.Unity.SDK;
using UnityEngine.SceneManagement;

public class AccountManager : MonoBehaviour
{

    [SerializeField]
    GameObject mainMenu;
    [SerializeField]
    GameObject connectionMenu;
    public static AccountManager Instance { get; private set;}

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
        Debug.Log("AccountManager instance cleared");
        mainMenu.SetActive(false);
        connectionMenu.SetActive(true);
    }
    public static void ClearInstance()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
            Instance = null;
        }
    }
    public static void CreateAccount(string pubkey)
    {
        FacetClient.CallFacet((AccountCreate facet) => facet.CreateAccount(pubkey))
        .Then(response => 
        {
            Debug.Log("Account created or updated with EntityId: " + response);
        })
        .Catch(error => 
        {
            Debug.LogError("Failed to create or update account: " + error);
        });
    }

    public void CopyPubKey(){
        GUIUtility.systemCopyBuffer = Web3.Wallet.Account.PublicKey.ToString();
    }
}