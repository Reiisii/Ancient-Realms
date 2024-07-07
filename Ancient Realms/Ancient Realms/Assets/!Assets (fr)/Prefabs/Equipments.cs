using System;
using System.Collections;
using System.Collections.Generic;
using Solana.Unity.SDK.Nft;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Equipments : MonoBehaviour
{
    equipmentData equipment;
    
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI eqName;
    
    public void OnItemClick()
    {
        StartCoroutine(EncycHandler.Instance.ShowItemDetails(equipment));
    }
    public void setImage(Texture2D nftImage){
        Sprite sprites = Sprite.Create(nftImage, new Rect(0, 0, nftImage.width, nftImage.height), Vector2.one * 0.5f);

        // Set the sprite to the Image component
        image.sprite = sprites;
    }
    public void setName(string name){
        eqName.SetText(name);
    }
    public void setData(equipmentData eqData){
        equipment = eqData;
    }
}