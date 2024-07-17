using System.Collections;
using System.Collections.Generic;
using Solana.Unity.SDK;
using Solana.Unity.SDK.Nft;
using UnityEngine;

public class AccountNFT : MonoBehaviour
{
    private void OnEnable(){
        Web3.OnNFTsUpdate += OnNFTsUpdate;
    }
    private void OnDisable(){
        Web3.OnNFTsUpdate -= OnNFTsUpdate;
    }
    public void OnNFTsUpdate(List<Nft> nfts, int total)
    {
        // DO SOMETHING
    }
}