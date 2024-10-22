using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LogoAnimation : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] GameObject LogoGO;
    [SerializeField] RectTransform Logo;
    [SerializeField] float panelDuration; 
    [SerializeField] float defaultPanelPosY; 
    [SerializeField] float newPanelPosY;
    [SerializeField] EaseTypes panelEaseType;
    private void OnEnable(){
        Logo.DOAnchorPosY(newPanelPosY, panelDuration).SetEase((Ease)panelEaseType).SetUpdate(true).OnComplete(() => UIManager.EnableAllButtons(LogoGO));
    }
    public void Close(){
        UIManager.EnableAllButtons(LogoGO);
        Logo.DOAnchorPosY(defaultPanelPosY, panelDuration).SetUpdate(true).SetEase((Ease)panelEaseType);
    }
}
