using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CreditsAnimation : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] GameObject CreditsGO;
    [SerializeField] RectTransform Credits;
    [SerializeField] float panelDuration; 
    [SerializeField] float defaultPanelPosY; 
    [SerializeField] float newPanelPosY;
    [SerializeField] EaseTypes panelEaseType;
    private void OnEnable(){
        Credits.DOAnchorPosY(newPanelPosY, panelDuration).SetEase((Ease)panelEaseType).OnComplete(() => UIManager.EnableAllButtons(CreditsGO));
    }
    private void OnDisable(){
        Credits.DOAnchorPosY(defaultPanelPosY, panelDuration).SetEase((Ease)panelEaseType).OnComplete(() => UIManager.DisableAllButtons(CreditsGO));
    }
    public void Close(){
        UIManager.DisableAllButtons(CreditsGO);
        Credits.DOAnchorPosY(defaultPanelPosY, panelDuration).SetEase((Ease)panelEaseType).OnComplete(() => {
           CreditsGO.SetActive(false);
        });
        // Panel.DOAnchorPosX(defaultPanelPosY, panelDuration).SetEase((Ease)panelEaseType).OnComplete(() => UIManager.EnableAllButtons(PanelGO));
    }
}
