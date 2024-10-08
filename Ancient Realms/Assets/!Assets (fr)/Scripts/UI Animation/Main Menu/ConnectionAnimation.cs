using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    public async Task Close(){
        UIManager.DisableAllButtons(LogoGO);
        await Logo.DOAnchorPosY(defaultPanelPosY, panelDuration).SetEase((Ease)panelEaseType).OnComplete(() => {
            UIManager.EnableAllButtons(LogoGO);
            gameObject.SetActive(false);
        }).AsyncWaitForCompletion();
    }
}
