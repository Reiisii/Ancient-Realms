using System;
using System.Collections;
using System.Collections.Generic;
using Solana.Unity.SDK.Nft;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CharacterPortrait : MonoBehaviour
{
    characterData character;
    
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI charName;
    
    public void OnItemClick()
    {
        StartCoroutine(EncycHandler.Instance.ShowItemDetails(character));
    }
    public void setImage(Texture2D nftImage){
        Sprite sprites = Sprite.Create(nftImage, new Rect(0, 0, nftImage.width, nftImage.height), Vector2.one * 0.5f);

        // Set the sprite to the Image component
        image.sprite = sprites;
    }
    public void setName(string name){
        charName.SetText(name);
    }
    public void setData(characterData charData){
        character = charData;
    }
}