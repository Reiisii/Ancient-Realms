using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Solana.Unity.SDK.Nft;
using ESDatabase.Entities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class CharacterPortrait : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI charName;
    [SerializeField] GameObject panel;
    [SerializeField] Image bgImageColor;
    public CharacterSO character {get;set;}

    void Start(){
        image.color = Color.black;
        charName.SetText("???");
        Color color = Utilities.GetColorForCulture(character.culture);
        bgImageColor.color = color;
    }
    void Update(){
        PlayerData playerData = AccountManager.Instance.playerData;
        if(playerData != null){
            if(playerData.gameData.characters.Contains(character.id)){
                image.color = Color.white;
                image.sprite = character.image;
                charName.SetText(character.lastName.Equals("") ? character.firstName : character.firstName + " " + character.lastName);
            }else{
                image.color = Color.black;
                charName.SetText("???");
            }
        }
        Color color = Utilities.GetColorForCulture(character.culture);
        bgImageColor.color = color;
    }
    public void OnItemClick()
    {
        PlayerData playerData = AccountManager.Instance.playerData;
        if(!playerData.gameData.characters.Contains(character.id)){
            return;
        }
        UIManager.DisableAllButtons(panel);
        panel.GetComponent<RectTransform>().DOAnchorPosY(-1050, 0.5f).SetEase(Ease.InOutSine).OnComplete(() => {
           panel.SetActive(false);
        });
        EncycHandler.Instance.ShowItemDetails(character);
    }
    public void setGameObject(GameObject pnl){
        panel = pnl;
    }
    public void setData(CharacterSO charr){
        character = charr;
    }
}