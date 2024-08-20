using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    [SerializeField] public Canvas oldSceneCanvas;

    public void PlayPrologue()
    {
        DOTween.Clear(true);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        // Disable the canvas of the old scene
        if (oldSceneCanvas != null)
        {
            oldSceneCanvas.enabled = false;
        }

        // Activate the new scene
        asyncLoad.allowSceneActivation = true;

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
