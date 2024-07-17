using System.Collections;
using System.Collections.Generic;
using Solana.Unity.SDK;
using Solana.Unity.Wallet;
using UnityEngine;

public class AccountStoreTesting : MonoBehaviour
{
    [SerializeField] GameObject btnTransact;
    [SerializeField] GameObject mintTransact;
    private void OnEnable(){
        Web3.OnLogin += OnLogin;
    }
    private void OnDisable(){
        Web3.OnLogin -= OnLogin;
    }
    private void OnLogin(Account account){

        Debug.Log(account.PublicKey.ToString());
        btnTransact.SetActive(true);
        mintTransact.SetActive(true);
    }
}
