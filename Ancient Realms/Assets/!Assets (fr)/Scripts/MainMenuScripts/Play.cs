using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    [SerializeField] public Canvas oldSceneCanvas;
    [SerializeField] CanvasGroup canvasGroup;
    public void PlayPrologue()
    {
        canvasGroup.gameObject.SetActive(true);
        canvasGroup.DOFade(1, 0.8f).SetEase(Ease.OutSine).SetUpdate(true).OnComplete(()=>{
            DOTween.Clear(true);
            SceneManager.LoadScene(1);
        });  
    }
    public void PlayMainMenu()
    {
        DOTween.Clear(true);
        SceneManager.LoadScene(0);
    }
    public void PlayRome()
    {
        DOTween.Clear(true);
        SceneManager.LoadSceneAsync(3); 
    }
    public void PlayTransaction()
    {
        DOTween.Clear(true);
        SceneManager.LoadSceneAsync(2); 

    }
    public void PlayMap()
    {
        DOTween.Clear(true);
        SceneManager.LoadSceneAsync(4); 

    }
}
