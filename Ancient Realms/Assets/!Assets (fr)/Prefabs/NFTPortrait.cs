using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Solana.Unity.SDK.Nft;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NFTPortrait : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI nftName;
    [SerializeField] GameObject accountPanel;
    public NFTSO nft;
    void Start(){
        nftName.SetText(nft.nftName);
        image.sprite = nft.image;
    }
    public void OnItemClick()
    {
        EncycHandler.Instance.ShowItemDetails(nft);
        UIManager.DisableAllButtons(accountPanel);
        accountPanel.GetComponent<RectTransform>().DOAnchorPosY(-1050, 0.5f).SetEase(Ease.InOutSine).OnComplete(() => {
           accountPanel.SetActive(false);
        });
    }
    public void setGameObject(GameObject pnl){
        accountPanel = pnl;
    }
    public void setNFT(NFTSO nftData){
        nft = nftData;
    }

}