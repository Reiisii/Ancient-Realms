using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using ESDatabase.Entities;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUIManager : MonoBehaviour
{
    [Header("Background")]
    [SerializeField] public GameObject backgroundGO;
    [SerializeField] public CanvasGroup backgroundCanvasGroup;
    [Header("Player UI")]
    [SerializeField] public GameObject playerUI;
    [SerializeField] public CanvasGroup playerCanvasGroup;
    [Header("Loading Screen")]
    [SerializeField] public GameObject loadingScreen;
    [SerializeField] public CanvasGroup loadingCanvasGroup;
    [Header("Fade")]
    [SerializeField] public CanvasGroup fade;
    [SerializeField] public GameObject fadeGO;
    [Header("Map")]
    [SerializeField] public CanvasGroup mapCanvasGroup;
    [SerializeField] public GameObject mapGO;
    [SerializeField] public GameObject worldMap;
    [Header("Location Plaque")]
    [SerializeField] public GameObject locationPlaque;
    [SerializeField] public TextMeshProUGUI locationText;
    [Header("Achievement Plaque")]
    [SerializeField] public GameObject achievementPlaque;
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
        playerCanvasGroup.interactable = false;
        await playerCanvasGroup.DOFade(1, fadeDuration).SetEase((Ease)fadeEaseType).SetUpdate(true).AsyncWaitForCompletion();
        playerCanvasGroup.interactable = true;
    }
    public async Task ClosePlayerUI(){
        playerCanvasGroup.interactable = false;
        await playerCanvasGroup.DOFade(0, fadeDuration).SetEase((Ease)fadeEaseType).SetUpdate(true).AsyncWaitForCompletion();
        playerUI.SetActive(false);
    }
    public async Task OpenLoadingUI(){
        loadingScreen.SetActive(true);
        await loadingCanvasGroup.DOFade(1, fadeDuration).SetEase((Ease)fadeEaseType).SetUpdate(true).AsyncWaitForCompletion();
    }
    public async Task CloseLoadingUI(){
        await loadingCanvasGroup.DOFade(0, fadeDuration).SetEase((Ease)fadeEaseType).SetUpdate(true).AsyncWaitForCompletion();
        loadingScreen.SetActive(false);
    }
    public async Task OpenDarkenUI(){
        fadeGO.SetActive(true);
        await fade.DOFade(1, fadeDuration).SetEase((Ease)fadeEaseType).SetUpdate(true).AsyncWaitForCompletion();
    }
    public async Task CloseDarkenUI(){
        await fade.DOFade(0, fadeDuration).SetEase((Ease)fadeEaseType).SetUpdate(true).AsyncWaitForCompletion();
        fadeGO.SetActive(false);
    }
    public async Task OpenBackgroundUI(){
        backgroundGO.SetActive(true);
        await backgroundCanvasGroup.DOFade(1, fadeDuration).SetEase((Ease)fadeEaseType).SetUpdate(true).AsyncWaitForCompletion();
    }
    public async Task CloseBackgroundUI(){
        await backgroundCanvasGroup.DOFade(0, fadeDuration).SetEase((Ease)fadeEaseType).SetUpdate(true).AsyncWaitForCompletion();
        backgroundGO.SetActive(false);
    }
    public async Task OpenMapUI(){
        mapGO.SetActive(true);
        mapCanvasGroup.interactable = false;
        worldMap.SetActive(true);
        await mapCanvasGroup.DOFade(1, fadeDuration).SetEase((Ease)fadeEaseType).SetUpdate(true).AsyncWaitForCompletion();
        mapCanvasGroup.interactable = true;
    }
    public async Task CloseMapUI(){
        mapCanvasGroup.interactable = false;
        await mapCanvasGroup.DOFade(0, fadeDuration).SetEase((Ease)fadeEaseType).SetUpdate(true).AsyncWaitForCompletion();
        mapGO.SetActive(false);
        worldMap.SetActive(false);
    }
    private async void OnSceneLoaded(AsyncOperation operation)
    {
        await CloseBackgroundUI();
        await CloseLoadingUI();
        await OpenPlayerUI();
        DOTween.Clear(true);
    }
}
