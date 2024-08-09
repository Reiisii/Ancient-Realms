using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FadeAnimation : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] GameObject panelGO;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] float fadeDuration;    
    [SerializeField] EaseTypes fadeEaseType;
    private void OnEnable(){
        canvasGroup.DOFade(1, fadeDuration).SetEase((Ease)fadeEaseType).SetUpdate(true);
    }
    public void Close(){
        canvasGroup.DOFade(0, fadeDuration).SetEase((Ease)fadeEaseType).SetUpdate(true).OnComplete(() =>{
            panelGO.SetActive(false);
        });
    }
    
}
