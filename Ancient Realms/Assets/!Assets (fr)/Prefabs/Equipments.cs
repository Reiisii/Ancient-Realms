using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Solana.Unity.SDK.Nft;
using TMPro;
using ESDatabase.Entities;
using UnityEngine;
using UnityEngine.UI;


public class Equipments : MonoBehaviour
{    
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI eqName;
    [SerializeField] GameObject panel;
    [SerializeField] Image bgImageColor;
    public EquipmentSO equipment;
    void Start(){
        image.color = Color.black;
        eqName.SetText("???");
        Color color = Utilities.GetColorForCulture(equipment.culture);
        bgImageColor.color = color;
    }
    void Update(){
        PlayerData playerData = AccountManager.Instance.playerData;
        if(playerData != null){
            if(playerData.gameData.equipments.Contains(equipment.equipmentId)){
                image.color = Color.white;
                image.sprite = equipment.image;
                eqName.SetText(equipment.itemName);
            }else{
                image.color = Color.black;
                eqName.SetText("???");
            }
        }
        Color color = Utilities.GetColorForCulture(equipment.culture);
        bgImageColor.color = color;
    }
    public void OnItemClick()
    {
        EncycHandler.Instance.ShowItemDetails(equipment);
        UIManager.DisableAllButtons(panel);
        panel.GetComponent<RectTransform>().DOAnchorPosY(-1050, 0.5f).SetEase(Ease.InOutSine).OnComplete(() => {
           panel.SetActive(false);
        });
    }
    public void setGameObject(GameObject pnl){
        panel = pnl;
    }
    public void setImage(Texture2D nftImage){
        Sprite sprites = Sprite.Create(nftImage, new Rect(0, 0, nftImage.width, nftImage.height), Vector2.one * 0.5f);

        // Set the sprite to the Image component
        image.sprite = sprites;
    }
    public void setData(EquipmentSO eqData){
        equipment = eqData;
    }
}