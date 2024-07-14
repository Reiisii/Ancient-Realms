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
    [SerializeField] GameObject ParentMenu;
    private void OnEnable(){
        MainMenu.DOAnchorPosX(newPanelPosX, panelDuration).SetEase((Ease)panelEaseType).OnComplete(() => UIManager.EnableAllButtons(MainMenuGO));
    }
    public void Close(){
        UIManager.DisableAllButtons(MainMenuGO);
        MainMenu.DOAnchorPosX(defaultPanelPosX, panelDuration).SetEase((Ease)panelEaseType).OnComplete(() => {
            ParentMenu.SetActive(false);
        });
    }
}
