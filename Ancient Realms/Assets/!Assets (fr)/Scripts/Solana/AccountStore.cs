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