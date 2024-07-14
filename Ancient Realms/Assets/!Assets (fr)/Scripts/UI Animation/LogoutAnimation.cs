using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LogoutAnimation : MonoBehaviour
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
    private void OnDisable(){
        Panel.DOAnchorPosY(defaultPanelPosY, panelDuration).SetEase((Ease)panelEaseType).OnComplete(() => UIManager.DisableAllButtons(PanelGO));
        // Panel.DOAnchorPosX(defaultPanelPosY, panelDuration).SetEase((Ease)panelEaseType).OnComplete(() => UIManager.EnableAllButtons(PanelGO));
    }
}
