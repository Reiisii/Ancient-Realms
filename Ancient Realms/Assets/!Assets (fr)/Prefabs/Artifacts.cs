using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using ESDatabase.Entities;
using Solana.Unity.SDK.Nft;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Artifacts : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI artName;
    [SerializeField] GameObject panel;
    [SerializeField] Image bgImageColor;
    public ArtifactsSO artifact;
    void Start(){
        image.color = Color.black;
        artName.SetText("???");
        Color color = Utilities.GetColorForCulture(artifact.culture);
        bgImageColor.color = color;
    }
    void Update(){
        PlayerData playerData = AccountManager.Instance.playerData;
        if(playerData != null){
            if(playerData.gameData.artifacts.FirstOrDefault(a => a.artifactID == artifact.id) != null){
                image.color = Color.white;
                image.sprite = artifact.image;
                artName.SetText(artifact.artifactName);
            }else{
                image.color = Color.black;
                artName.SetText("???");
            }
        }
        Color color = Utilities.GetColorForCulture(artifact.culture);
        bgImageColor.color = color;
    }
    public void OnItemClick()
    {
        PlayerData playerData = AccountManager.Instance.playerData;
        if(playerData != null){
            if(playerData.gameData.artifacts.FirstOrDefault(a => a.artifactID == artifact.id) == null){
                return;
            }
        
            bool visible = artName.text.Equals("???");
            EncycHandler.Instance.ShowItemDetails(artifact, !visible);
            UIManager.DisableAllButtons(panel);
            panel.GetComponent<RectTransform>().DOAnchorPosY(-1050, 0.5f).SetEase(Ease.InOutSine).OnComplete(() => {
            panel.SetActive(false);
            });
        }
    }
    public void setGameObject(GameObject pnl){
        panel = pnl;
    }
    public void setData(ArtifactsSO artData){
        artifact = artData;
    }
}