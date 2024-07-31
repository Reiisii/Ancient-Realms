using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Solana.Unity.SDK.Nft;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CharacterPortrait : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI charName;
    [SerializeField] GameObject panel;
    [SerializeField] Image bgImageColor;
    public CharacterSO character {get;set;}

    void Start(){
        image.sprite = character.image;
        charName.SetText(character.lastName.Equals("") ? character.firstName : character.firstName + " " + character.lastName);
        Color color = Utilities.GetColorForCulture(character.culture);
        bgImageColor.color = color;
    }
    public void OnItemClick()
    {
        UIManager.DisableAllButtons(panel);
        panel.GetComponent<RectTransform>().DOAnchorPosY(-1050, 0.5f).SetEase(Ease.InOutSine).OnComplete(() => {
           panel.SetActive(false);
        });
        EncycHandler.Instance.ShowItemDetails(character);
    }
    public void setGameObject(GameObject pnl){
        panel = pnl;
    }
}