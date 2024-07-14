using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LocationsPanelAnimation : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] GameObject PanelGO;
    [SerializeField] RectTransform Panel;
    [SerializeField] float panelDuration; 
    [SerializeField] float defaultPanelPosY; 
    [SerializeField] float newPanelPosY;
    [SerializeField] EaseTypes panelEaseType;
    private void OnEnable(){
        Panel.DOAnchorPosY(newPanelPosY, panelDuration).SetEase((Ease)panelEaseType).OnComplete(() => UIManager.EnableAllButtons(PanelGO));
        // Panel.DOAnchorPosX(newPanelPosY, panelDuration).SetEase((Ease)panelEaseType).OnComplete(() => UIManager.EnableAllButtons(PanelGO));
    }
    public void Close(){
        UIManager.DisableAllButtons(PanelGO);
        Panel.DOAnchorPosY(defaultPanelPosY, panelDuration).SetEase((Ease)panelEaseType).OnComplete(() => {
           PanelGO.SetActive(false);
        });
        // Panel.DOAnchorPosX(defaultPanelPosY, panelDuration).SetEase((Ease)panelEaseType).OnComplete(() => UIManager.EnableAllButtons(PanelGO));
    }
}
