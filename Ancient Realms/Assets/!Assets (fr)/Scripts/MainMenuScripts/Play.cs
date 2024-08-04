using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    [SerializeField] public Canvas oldSceneCanvas;

    public void PlayGame()
    {
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
}
