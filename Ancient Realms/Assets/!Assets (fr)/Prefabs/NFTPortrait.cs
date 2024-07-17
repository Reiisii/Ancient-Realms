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
    nftData nft;
    public void OnItemClick()
    {
        StartCoroutine(EncycHandler.Instance.ShowItemDetails(nft));
        UIManager.DisableAllButtons(accountPanel);
        accountPanel.GetComponent<RectTransform>().DOAnchorPosY(-1050, 0.5f).SetEase(Ease.InOutSine).OnComplete(() => {
           accountPanel.SetActive(false);
        });
    }
    public void setImage(Texture2D nftImage){
        Sprite sprites = Sprite.Create(nftImage, new Rect(0, 0, nftImage.width, nftImage.height), Vector2.one * 0.5f);

        // Set the sprite to the Image component
        image.sprite = sprites;
    }
    public void setGameObject(GameObject panel){
        accountPanel = panel;
    }
    public void setName(string name){
        nftName.SetText(name);
    }
    public void setNFT(nftData nftData){
        nft = nftData;
    }

}