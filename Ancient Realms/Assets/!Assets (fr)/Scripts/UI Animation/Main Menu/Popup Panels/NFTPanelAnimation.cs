using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class NFTPanelAnimation : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] GameObject PanelGO;
    [SerializeField] RectTransform Panel;
    [SerializeField] float panelDuration; 
    [SerializeField] float defaultPanelPosY; 
    [SerializeField] float newPanelPosY;
    [SerializeField] EaseTypes panelEaseType;
    private void OnEnable(){
        UIManager.DisableAllButtons(PanelGO);
        Panel.DOAnchorPosY(newPanelPosY, panelDuration).SetEase((Ease)panelEaseType).OnComplete(() => UIManager.EnableAllButtons(PanelGO));
        // Panel.DOAnchorPosX(newPanelPosY, panelDuration).SetEase((Ease)panelEaseType).OnComplete(() => UIManager.EnableAllButtons(PanelGO));
    }
    public async void Close(){
        UIManager.DisableAllButtons(PanelGO);
        await Panel.DOAnchorPosY(defaultPanelPosY, panelDuration).SetEase((Ease)panelEaseType).OnComplete(() => {
           PanelGO.SetActive(false);
           UIManager.DisableAllButtons(PanelGO);
        }).AsyncWaitForCompletion();
        // Panel.DOAnchorPosX(defaultPanelPosY, panelDuration).SetEase((Ease)panelEaseType).OnComplete(() => UIManager.EnableAllButtons(PanelGO));
    }
}
