using System.Collections;
using System.Collections.Generic;
using Solana.Unity.SDK;
using UnityEngine;

public class AccountBalance : MonoBehaviour
{
    private void OnEnable(){
        Web3.OnBalanceChange += OnBalanceChange;
    }
    private void OnDisable(){
        Web3.OnBalanceChange -= OnBalanceChange;
    }
    private void OnBalanceChange(double solBalance)
    {
       // DO SOMETHING
    }
}