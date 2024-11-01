using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ESDatabase.Classes;
using UnityEngine;
using UnityEngine.EventSystems;
public class SlotNFT : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private int slotNo;
    [SerializeField] private NFTData nft;
    [SerializeField] private bool isHovering = false;
    public void Start(){
        UpdateEquipment();
    }
    public void UpdateEquipment()
    {
        nft = PlayerStats.GetInstance().localPlayerData.gameData.equippedNFT[slotNo];
        if (nft == null && isHovering) {
            isHovering = false;
            TooltipManager.GetInstance().HideNFTTooltip();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isHovering)
        {
            if(nft == null) return;
            isHovering = true;
            NFTSO nFTSO = AccountManager.Instance.nfts.FirstOrDefault(n => n.id.Equals(nft.nftID));
            TooltipManager.GetInstance().ShowNFTTooltip(nFTSO);
        }

    }
    public void OnPointerClick(PointerEventData eventData){
        NFTData nFTData = PlayerStats.GetInstance().localPlayerData.gameData.equippedNFT[slotNo];
        if (nFTData == null && isHovering) {
            isHovering = false;
            TooltipManager.GetInstance().HideNFTTooltip();
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isHovering)
        {
            isHovering = false;
            TooltipManager.GetInstance().HideNFTTooltip();
        }

    }
    private void OnDestroy(){
        if (isHovering)
        {
            isHovering = false;
            TooltipManager.GetInstance().HideNFTTooltip();
        }
    }
}
