using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ChapterSelectAnimation : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] GameObject ChapterSelectGO;
    [SerializeField] RectTransform ChapterSelect;
    [SerializeField] float panelDuration; 
    [SerializeField] float defaultPanelPosY; 
    [SerializeField] float newPanelPosY;
    [SerializeField] EaseTypes panelEaseType;
    private void OnEnable(){
        ChapterSelect.DOAnchorPosY(newPanelPosY, panelDuration).SetEase((Ease)panelEaseType).OnComplete(() => UIManager.EnableAllButtons(ChapterSelectGO));
    }
    private void OnDisable(){
        ChapterSelect.DOAnchorPosY(defaultPanelPosY, panelDuration).SetEase((Ease)panelEaseType).OnComplete(() => UIManager.DisableAllButtons(ChapterSelectGO));
    }
}
