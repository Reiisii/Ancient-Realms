using System.Collections;
using System.Collections.Generic;
using Solana.Unity.SDK.Nft;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryNFT : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] public Image background;
    private InventoryPanel inventoryPanel;
    
    // [SerializeField] TextMeshProUGUI nftName;
    public bool isSelected = false;
    public string mintKey;
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

    public void OnClickNFT(){   
        if(!isSelected){
            InventoryManager.GetInstance().invPanel.DeselectAllNFT();
            Select();
        }else{
            Deselect();
        }
    }
    public void Select(){
        InventoryManager.GetInstance().invPanel.nftSelected = true;
        InventoryManager.GetInstance().invPanel.selectedNFT = nft;
        InventoryManager.GetInstance().invPanel.selectedNFTSO = nftSO;
        InventoryManager.GetInstance().invPanel.slot1.isSelected = false;
        InventoryManager.GetInstance().invPanel.slot2.isSelected = false;
        InventoryManager.GetInstance().invPanel.slot3.isSelected = false;
        isSelected = true;
        background.color = Utilities.HexToColor("#FDFF00");
    }

    public void Deselect(){
        isSelected = false;
        background.color = Utilities.HexToColor("#FFFFFF");
    }
    public void setNFT(Nft nftData, NFTSO nftd){
        mintKey = nftData.metaplexData.data.mint;
        nft = nftData;
        nftSO = nftd;
        InitializeNFTDisplay();
    }
}
