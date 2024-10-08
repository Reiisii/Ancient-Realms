using System.Collections;
using System.Collections.Generic;
using Solana.Unity.SDK.Nft;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryNFT : MonoBehaviour
{
    [SerializeField] Image image;
    // [SerializeField] TextMeshProUGUI nftName;
    public Nft nft;
    public NFTSO nftSO;
    public void InitializeNFTDisplay()
    {
        if (nftSO != null)
        {
            // nftName.SetText(nftSO.nftName);
            image.sprite = nftSO.image;
        }
        else
        {
            Debug.LogError("NFTSO is null, cannot display NFT data.");
        }
    }
    public void setNFT(Nft nftData, NFTSO nftd){
        nft = nftData;
        nftSO = nftd;
        InitializeNFTDisplay();
    }

}
