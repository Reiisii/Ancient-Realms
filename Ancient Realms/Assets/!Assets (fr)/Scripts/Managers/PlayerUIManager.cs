using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using ESDatabase.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUIManager : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] CanvasGroup backgroundCanvasGroup;
    [SerializeField] GameObject playerUI;
    [SerializeField] CanvasGroup playerCanvasGroup;
    [SerializeField] GameObject loadingScreen;
    [SerializeField] CanvasGroup loadingCanvasGroup;
    [SerializeField] CanvasGroup fade;
    [Header("Settings")]
    [SerializeField] float fadeDuration;    
    [SerializeField] EaseTypes fadeEaseType;
    private static PlayerUIManager Instance;
    
    private void Awake(){
        if (Instance == null)
        {
            // If not, set this as the instance and make it persistent
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Found more than one Player UI Manager in the scene");
            Destroy(gameObject);
        }
    }
    
    private async void Start(){
        await CloseDarkenUI();
        await OpenLoadingUI();
        PlayerData playerData = AccountManager.Instance.playerData;

        SceneManager.LoadSceneAsync(playerData.gameData.lastLocationVisited, LoadSceneMode.Additive).completed += OnSceneLoaded;
    }

    public static PlayerUIManager GetInstance(){
        return Instance;
    }


    public async Task OpenPlayerUI(){
        playerUI.SetActive(true);
        await playerCanvasGroup.DOFade(1, fadeDuration).SetEase((Ease)fadeEaseType).SetUpdate(true).AsyncWaitForCompletion();
    }
    public async Task ClosePlayerUI(){
        await playerCanvasGroup.DOFade(0, fadeDuration).SetEase((Ease)fadeEaseType).SetUpdate(true).AsyncWaitForCompletion();
    }
    public async Task OpenLoadingUI(){
        await loadingCanvasGroup.DOFade(1, fadeDuration).SetEase((Ease)fadeEaseType).SetUpdate(true).AsyncWaitForCompletion();
    }
    public async Task CloseLoadingUI(){
        await loadingCanvasGroup.DOFade(0, fadeDuration).SetEase((Ease)fadeEaseType).SetUpdate(true).AsyncWaitForCompletion();
    }
    public async Task OpenDarkenUI(){
        await fade.DOFade(1, fadeDuration).SetEase((Ease)fadeEaseType).SetUpdate(true).AsyncWaitForCompletion();
    }
    public async Task CloseDarkenUI(){
        await fade.DOFade(0, fadeDuration).SetEase((Ease)fadeEaseType).SetUpdate(true).AsyncWaitForCompletion();
    }
    public async Task OpenBackgroundUI(){
        await backgroundCanvasGroup.DOFade(1, fadeDuration).SetEase((Ease)fadeEaseType).SetUpdate(true).AsyncWaitForCompletion();
    }
    public async Task CloseBackgroundUI(){
        await backgroundCanvasGroup.DOFade(0, fadeDuration).SetEase((Ease)fadeEaseType).SetUpdate(true).AsyncWaitForCompletion();
    }
    private async void OnSceneLoaded(AsyncOperation operation)
    {
        await CloseBackgroundUI();
        await CloseLoadingUI();
        await OpenPlayerUI();
        DOTween.Clear(true);
    }
}
