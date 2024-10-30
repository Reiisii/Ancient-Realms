using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ESDatabase.Classes;
using Solana.Unity.SDK.Nft;
using UnityEngine;
using UnityEngine.UI;

public class NFTSlot : MonoBehaviour
{
    [SerializeField] public int slotNo;
    [SerializeField] NFTSO nftSO;
    [SerializeField] Nft nft;
    [SerializeField] Image image;
    [SerializeField] Sprite defaultSprite;
    public bool isSelected = false;
    public void OnEnable(){
        NFTData nftData = PlayerStats.GetInstance().localPlayerData.gameData.equippedNFT[slotNo];
        if(nftData != null){
            nftSO = AccountManager.Instance.nfts.FirstOrDefault(nft => nft.id.Equals(nftData.nftID));
            nft = Nft.TryLoadNftFromLocal(nftData.mint);

        }
    }
    public void SetNFT(NFTSO so, Nft nftData){
        nftSO = so;
        nft = nftData;
    }

    public void OnClickNFT(){
        
        if(InventoryManager.GetInstance().invPanel.nftSelected && InventoryManager.GetInstance().invPanel.selectedNFT != null && nftSO == null){
            GameData gameData = PlayerStats.GetInstance().localPlayerData.gameData;
            NFTData chainData = new NFTData();
            chainData.mint = InventoryManager.GetInstance().invPanel.selectedNFT.metaplexData.data.mint;
            chainData.nftID = InventoryManager.GetInstance().invPanel.selectedNFTSO.id;
            gameData.equippedNFT[slotNo] = chainData;
            PlayerStats.GetInstance().isDataDirty = true; 
            return;
        }
        if(!isSelected) {
            isSelected = true;
            return;
        }
        
    }
    public void UnequipNFT(){
        nftSO = null;
        nft = null;
        isSelected = false;
        image.sprite = defaultSprite;
    }
}
