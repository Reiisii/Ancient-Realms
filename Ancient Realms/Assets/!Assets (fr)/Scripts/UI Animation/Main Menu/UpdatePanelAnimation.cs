using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UpdatePanelAnimation : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] GameObject UpdatePanelGO;
    [SerializeField] RectTransform UpdatePanel;
    [SerializeField] float panelDuration; 
    [SerializeField] float defaultPanelPosX; 
    [SerializeField] float newPanelPosX;
    [SerializeField] EaseTypes panelEaseType;
    private void OnEnable(){
        UpdatePanel.DOAnchorPosX(newPanelPosX, panelDuration).SetEase((Ease)panelEaseType).OnComplete(() => UIManager.EnableAllButtons(UpdatePanelGO));
    }
    private void OnDisable(){
        UpdatePanel.DOAnchorPosX(defaultPanelPosX, panelDuration).SetEase((Ease)panelEaseType).OnComplete(() => UIManager.DisableAllButtons(UpdatePanelGO));
    }
}
