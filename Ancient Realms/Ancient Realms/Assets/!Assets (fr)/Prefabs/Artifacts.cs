using System;
using System.Collections;
using System.Collections.Generic;
using Solana.Unity.SDK.Nft;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Artifacts : MonoBehaviour
{
    artifactsData artifact;
    
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI artName;
    
    public void OnItemClick()
    {
        StartCoroutine(EncycHandler.Instance.ShowItemDetails(artifact));
    }
    public void setImage(Texture2D nftImage){
        Sprite sprites = Sprite.Create(nftImage, new Rect(0, 0, nftImage.width, nftImage.height), Vector2.one * 0.5f);

        // Set the sprite to the Image component
        image.sprite = sprites;
    }
    public void setName(string name){
        artName.SetText(name);
    }
    public void setData(artifactsData artData){
        artifact = artData;
    }
}