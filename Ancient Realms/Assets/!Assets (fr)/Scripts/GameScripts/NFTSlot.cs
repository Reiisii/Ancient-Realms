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
            image.sprite = nftSO.image;
        }
    }
    public void SetNFT(NFTSO so, Nft nftData){
        nftSO = so;
        nft = nftData;
    }

    public void OnClickNFT(){
        GameData gameData = PlayerStats.GetInstance().localPlayerData.gameData;
        if (InventoryManager.GetInstance().invPanel.nftSelected && InventoryManager.GetInstance().invPanel.selectedNFT != null && (nftSO == null || nftSO != null)) {

            NFTData chainData = new NFTData();
            chainData.mint = InventoryManager.GetInstance().invPanel.selectedNFT.metaplexData.data.mint;
            chainData.nftID = InventoryManager.GetInstance().invPanel.selectedNFTSO.id;
            nft = InventoryManager.GetInstance().invPanel.selectedNFT;
            nftSO = InventoryManager.GetInstance().invPanel.selectedNFTSO;
            gameData.equippedNFT[slotNo] = chainData;
            image.sprite = InventoryManager.GetInstance().invPanel.selectedNFTSO.image;
            PlayerStats.GetInstance().isDataDirty = true;
            isSelected = false;
            InventoryManager.GetInstance().invPanel.DeselectAllNFT();
            InventoryManager.GetInstance().invPanel.nftSelected = false;
            InventoryManager.GetInstance().invPanel.selectedNFT = null;
            InventoryManager.GetInstance().invPanel.selectedNFTSO = null;
            PlayerStats.GetInstance().InitializeEquipments();
            InventoryManager.GetInstance().invPanel.InitializeInventory();
            AudioManager.GetInstance().PlayAudio(SoundType.NFTEquip, 1f);
            return;
        }
        if(!isSelected && nftSO == null){
            PlayerUIManager.GetInstance().SpawnMessage(MType.Info, "Select an NFT to Equip first!");
            return;
        }
        if (isSelected && nftSO != null) {
            UnequipNFT();

        } else if (!isSelected) {
            // If not selected, select this NFT
            isSelected = true;
        }
        
    }
    public void UnequipNFT(){
        GameData gameData = PlayerStats.GetInstance().localPlayerData.gameData;
        nftSO = null;
        nft = null;
        isSelected = false;
        image.sprite = defaultSprite;
        gameData.equippedNFT[slotNo] = null;
        PlayerStats.GetInstance().isDataDirty = true;
        
        PlayerStats.GetInstance().InitializeEquipments();
        AudioManager.GetInstance().PlayAudio(SoundType.NFTEquip, 1f);
        InventoryManager.GetInstance().invPanel.InitializeInventory();
    }
}
