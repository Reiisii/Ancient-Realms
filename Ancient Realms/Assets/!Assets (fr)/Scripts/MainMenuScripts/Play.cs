using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
            SceneManager.LoadSceneAsync(1);
        });  
    }
    public void PlayRome()
    {
        DOTween.Clear(true);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        // Disable the canvas of the old scene
        if (oldSceneCanvas != null)
        {
            oldSceneCanvas.enabled = false;
        }

        // Activate the new scene
        asyncLoad.allowSceneActivation = true;

    }
    public void PlayTransaction()
    {
        DOTween.Clear(true);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        // Disable the canvas of the old scene
        if (oldSceneCanvas != null)
        {
            oldSceneCanvas.enabled = false;
        }

        // Activate the new scene
        asyncLoad.allowSceneActivation = true;

    }
    public void PlayMap()
    {
        DOTween.Clear(true);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(4, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        // Disable the canvas of the old scene
        if (oldSceneCanvas != null)
        {
            oldSceneCanvas.enabled = false;
        }

        // Activate the new scene
        asyncLoad.allowSceneActivation = true;

    }
}
