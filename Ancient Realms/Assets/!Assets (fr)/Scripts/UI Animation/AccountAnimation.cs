using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AccountAnimation : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] GameObject PanelGO;
    [SerializeField] RectTransform Panel;
    [SerializeField] float panelDuration; 
    [SerializeField] float defaultPanelPosY; 
    [SerializeField] float newPanelPosY;
    [SerializeField] EaseTypes panelEaseType;
    private void OnEnable(){
        AccountModal accModal = gameObject.GetComponent<AccountModal>();
        if(accModal != null){
            accModal.isAnimating = true;
        }

        UIManager.DisableAllButtons(PanelGO);
        Panel.DOAnchorPosY(newPanelPosY, panelDuration).SetEase((Ease)panelEaseType).OnComplete(() => {
            UIManager.EnableAllButtons(PanelGO);
            accModal.isAnimating = false;
        });
        // Panel.DOAnchorPosX(newPanelPosY, panelDuration).SetEase((Ease)panelEaseType).OnComplete(() => UIManager.EnableAllButtons(PanelGO));
    }
    public void Close(){
         AccountModal accModal = gameObject.GetComponent<AccountModal>();
        if(accModal != null){
            accModal.isAnimating = true;
        }
        UIManager.DisableAllButtons(PanelGO);
        Panel.DOAnchorPosY(defaultPanelPosY, panelDuration).SetEase((Ease)panelEaseType).OnComplete(() => {
           PanelGO.SetActive(false);
           UIManager.DisableAllButtons(PanelGO);
           accModal.isAnimating = false;
        });
        // Panel.DOAnchorPosX(defaultPanelPosY, panelDuration).SetEase((Ease)panelEaseType).OnComplete(() => UIManager.EnableAllButtons(PanelGO));
    }
}
