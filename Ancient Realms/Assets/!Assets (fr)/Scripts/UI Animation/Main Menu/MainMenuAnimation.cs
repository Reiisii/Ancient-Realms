using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MainMenuAnimation : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] GameObject MainMenuGO;
    [SerializeField] RectTransform MainMenu;
    [SerializeField] float panelDuration; 
    [SerializeField] float defaultPanelPosX; 
    [SerializeField] float newPanelPosX;
    [SerializeField] EaseTypes panelEaseType;
    private void OnEnable(){
        MainMenu.DOAnchorPosX(newPanelPosX, panelDuration).SetEase((Ease)panelEaseType).OnComplete(() => UIManager.EnableAllButtons(MainMenuGO));
    }
    private void OnDisable(){
        MainMenu.DOAnchorPosX(defaultPanelPosX, panelDuration).SetEase((Ease)panelEaseType).OnComplete(() => UIManager.DisableAllButtons(MainMenuGO));
    }
}
