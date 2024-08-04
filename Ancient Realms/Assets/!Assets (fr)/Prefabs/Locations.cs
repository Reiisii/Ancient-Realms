using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Solana.Unity.SDK.Nft;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Locations : MonoBehaviour
{    
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI locationName;
    [SerializeField] GameObject panel;
    [SerializeField] Image ribbonColor;
    public LocationSO location;
    void Start(){
        image.sprite = location.image;
        locationName.SetText(location.locationName);
        Color color = Utilities.GetColorForCulture(location.culture);
        Debug.Log(location.culture);
        ribbonColor.color = color;
    }
    public void OnItemClick()
    {
        EncycHandler.Instance.ShowItemDetails(location);
        UIManager.DisableAllButtons(panel);
        panel.GetComponent<RectTransform>().DOAnchorPosY(-1050, 0.5f).SetEase(Ease.InOutSine).OnComplete(() => {
           panel.SetActive(false);
        });
    }
    public void setImage(Texture2D nftImage){
        Sprite sprites = Sprite.Create(nftImage, new Rect(0, 0, nftImage.width, nftImage.height), Vector2.one * 0.5f);

        // Set the sprite to the Image component
        image.sprite = sprites;
    }
    public void setGameObject(GameObject pnl){
        panel = pnl;
    }
    public void setData(LocationSO locationData){
        location = locationData;
    }
}
