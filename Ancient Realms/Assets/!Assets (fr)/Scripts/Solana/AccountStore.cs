using System.Collections;
using System.Collections.Generic;
using Solana.Unity.SDK;
using Solana.Unity.SDK.Nft;
using Solana.Unity.Wallet;
using Unisave.Facets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AccountStore : MonoBehaviour
{
    [SerializeField]
    GameObject mainMenu;

    [SerializeField]
    GameObject connectionMenu;
    void Start(){
        InitializeWeb3();
    }
    private void InitializeWeb3()
    {
        if (Web3.Instance == null)
        {
            // Assuming Web3 is attached to a GameObject called "Solana"
            GameObject solanaObject = GameObject.Find("Solana");
            if (solanaObject != null)
            {
                Web3 web3Component = solanaObject.GetComponent<Web3>();
                if (web3Component != null)
                {
                    Web3.Instance = web3Component;
                    Debug.Log("Web3 instance re-initialized.");
                }
                else
                {
                    Debug.LogError("Web3 component not found on Solana GameObject.");
                }
            }
            else
            {
                Debug.LogError("Solana GameObject not found.");
            }
        }
    }

    private void OnEnable(){
        Web3.OnLogin += OnLogin;
        Web3.OnLogout += OnLogout;
    }
    private void OnDisable(){
        Web3.OnLogin -= OnLogin;
        Web3.OnLogout -= OnLogout;
    }
    private void OnLogin(Account account){
        AccountManager.CreateAccount(account.PublicKey.ToString());
        mainMenu.SetActive(true);
        connectionMenu.SetActive(false);
    }
    private void OnLogout(){
        Debug.Log("Logout");
    }
}