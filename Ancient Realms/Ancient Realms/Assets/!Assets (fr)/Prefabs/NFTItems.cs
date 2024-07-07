using System;
using System.Collections;
using System.Collections.Generic;
using Solana.Unity.SDK.Nft;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NftItems : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI nftName;
    
    Nft nft;
    public void OnItemClick()
    {
        NFTPanel.Instance.ShowItemDetails(nft);
    }
    public void setImage(Texture2D nftImage){
        Sprite sprites = Sprite.Create(nftImage, new Rect(0, 0, nftImage.width, nftImage.height), Vector2.one * 0.5f);

        // Set the sprite to the Image component
        image.sprite = sprites;
    }
    public void setName(string name){
        nftName.SetText(name);
    }
    public void setNFT(Nft nftData){
        nft = nftData;
    }

}