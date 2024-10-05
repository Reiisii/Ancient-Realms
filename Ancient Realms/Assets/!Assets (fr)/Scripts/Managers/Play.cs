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
    [SerializeField] GameObject ChapterSelect;
    [SerializeField] GameObject mainMenu;
    private static Play Instance;
    
    private void Awake(){
        if (Instance == null)
        {
            // If not, set this as the instance and make it persistent
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Found more than one Play Manager in the scene");
            Destroy(gameObject);
        }
    }
    public static Play GetInstance(){
        return Instance;
    }
    public void PlayUILoader()
    {
        canvasGroup.gameObject.SetActive(true);
        ChapterSelect.GetComponent<ChapterSelectAnimation>().Close();
        canvasGroup.DOFade(1, 1f).SetEase(Ease.OutSine).SetUpdate(true).OnComplete(()=>{        
            SceneManager.LoadSceneAsync("Player UI Loader", LoadSceneMode.Additive).completed += OnSceneLoaded;
        });  
    }
    public void PlayMainMenu()
    {       
        DOTween.Clear(true);
        SceneManager.UnloadSceneAsync(AccountManager.Instance.playerData.gameData.lastLocationVisited).completed += (operation) => {
            SceneManager.UnloadSceneAsync("Player UI Loader").completed += (operation) => {
                oldSceneCanvas.enabled = true; // Re-enable the old canvas
                canvasGroup.DOFade(0, 0.8f).SetEase(Ease.OutSine).SetUpdate(true).OnComplete(() => {
                canvasGroup.gameObject.SetActive(false); // Ensure the canvas group is inactive to prevent blocking
                mainMenu.SetActive(true);
            });
            };
        };
 
    }
    private void OnSceneLoaded(AsyncOperation operation)
    {
        // DOTween.Clear(true);
        oldSceneCanvas.enabled = false;
    }
}
