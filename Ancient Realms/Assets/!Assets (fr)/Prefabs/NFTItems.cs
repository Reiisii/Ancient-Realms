using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Solana.Unity.SDK.Nft;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NftItems : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI nftName;
    [SerializeField] GameObject accountPanel;
    [SerializeField] Button button;
    public Nft nft;
    public NFTSO nftSO;
    public void InitializeNFTDisplay()
    {
        if (nftSO != null)
        {
            nftName.SetText(nftSO.nftName);
            image.sprite = nftSO.image;
            button.interactable = !accountPanel.GetComponent<AccountModal>().isAnimating;
        }
        else
        {
            Debug.LogError("NFTSO is null, cannot display NFT data.");
        }
    }
    public void OnItemClick()
    {
        NFTPanel.Instance.ShowItemDetails(nft, nftSO);
        UIManager.DisableAllButtons(accountPanel);
        accountPanel.GetComponent<RectTransform>().DOAnchorPosY(962f, 0.8f).SetEase(Ease.InOutSine).OnComplete(() => {
           accountPanel.SetActive(false);
        });
    }
    public void setGameObject(GameObject panel){
        accountPanel = panel;
    }
    public void setNFT(Nft nftData, NFTSO nftd){
        nft = nftData;
        nftSO = nftd;
        InitializeNFTDisplay();
    }

}