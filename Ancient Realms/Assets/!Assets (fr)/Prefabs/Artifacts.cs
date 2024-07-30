using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        image.sprite = artifact.image;
        artName.SetText(artifact.artifactName);
        Color color = Utilities.GetColorForCulture(artifact.culture);
        bgImageColor.color = color;
    }
    public void OnItemClick()
    {
        EncycHandler.Instance.ShowItemDetails(artifact);
        UIManager.DisableAllButtons(panel);
        panel.GetComponent<RectTransform>().DOAnchorPosY(-1050, 0.5f).SetEase(Ease.InOutSine).OnComplete(() => {
           panel.SetActive(false);
        });
    }
    public void setGameObject(GameObject pnl){
        panel = pnl;
    }
    public void setData(ArtifactsSO artData){
        artifact = artData;
    }
}