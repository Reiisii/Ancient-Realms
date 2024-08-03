using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ConnectionAnimation : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] GameObject LogoGO;
    [SerializeField] RectTransform Logo;
    [SerializeField] float panelDuration; 
    [SerializeField] float defaultPanelPosY; 
    [SerializeField] float newPanelPosY;
    [SerializeField] EaseTypes panelEaseType;
    private void OnEnable(){
        UIManager.DisableAllButtons(LogoGO);
        Logo.DOAnchorPosY(newPanelPosY, panelDuration).SetEase((Ease)panelEaseType).OnComplete(() => UIManager.EnableAllButtons(LogoGO));
    }
    
}
