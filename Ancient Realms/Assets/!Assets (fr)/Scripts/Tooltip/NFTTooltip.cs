using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NFTToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public InventoryNFT nftSO;
    [SerializeField] private bool isHovering = false;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isHovering)
        {
            isHovering = true;
            TooltipManager.GetInstance().ShowNFTTooltip(nftSO.nftSO);
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
    private void OnDisable(){
        isHovering = false;
        TooltipManager.GetInstance().HideNFTTooltip();
    }
    private void OnDestroy(){
        isHovering = false;
        TooltipManager.GetInstance().HideNFTTooltip();
    }
}
