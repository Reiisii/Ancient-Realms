using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EncyclopediaAnimation : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] GameObject EncycPanelGO;
    [SerializeField] RectTransform EncycPanel;
    [SerializeField] float panelDuration; 
    [SerializeField] float defaultPanelPosY; 
    [SerializeField] float newPanelPosY;
    [SerializeField] EaseTypes panelEaseType;
    private void OnEnable(){
        EncycPanel.DOAnchorPosY(newPanelPosY, panelDuration).SetEase((Ease)panelEaseType).OnComplete(() => UIManager.EnableAllButtons(EncycPanelGO));
    }
    public void Close(){
        UIManager.DisableAllButtons(EncycPanelGO);
        EncycPanel.DOAnchorPosY(defaultPanelPosY, panelDuration).SetEase((Ease)panelEaseType).OnComplete(() =>{ 
            EncycPanelGO.SetActive(false);
        });
    }
}