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
    private void OnNFTsUpdate(List<Nft> nfts, int total)
    {
        foreach (var nft in nfts)
        {
            Debug.Log("NFT: " + nft.metaplexData.nftImage.name);
        }
        AccountManager.Instance.SetNFTs(nfts, total);
    }
}