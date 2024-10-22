using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using ESDatabase.Classes;
using ESDatabase.Entities;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUIManager : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] public Canvas canvas;
    [Header("Background")]
    [SerializeField] public GameObject backgroundGO;
    [SerializeField] public CanvasGroup backgroundCanvasGroup;
    [Header("Player UI")]
    [SerializeField] public GameObject playerUI;
    [SerializeField] public CanvasGroup playerCanvasGroup;
    [SerializeField] public GameObject questPointer;
    [Header("Minting UI")]
    [SerializeField] public GameObject mintingUI;
    [Header("Premium UI")]
    [SerializeField] public GameObject premiumUI;
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
    [Header("Notification Manager")]
    [SerializeField] public NotificationQueue notification;
    [Header("Settings")]
    [SerializeField] float fadeDuration;    
    [SerializeField] EaseTypes fadeEaseType;
    [Header("Trivia")]
    [SerializeField] TextMeshProUGUI triviaTitle;
    [SerializeField] TextMeshProUGUI triviaDescription;
    [Header("Popup Message")]
    [SerializeField] GameObject popupParent;
    [Header("Smithing")]
    [SerializeField] GameObject smithingUI;
    [SerializeField] GameObject smithing;
    [Header("Toggle Quest")]
    [SerializeField] GameObject hideButton;
    [SerializeField] GameObject unHideButton;
    [SerializeField] GameObject activeQuestPanel;
    [Header("Time")]
    [SerializeField] public TimeController time;
    [Header("Prefabs")]
    [SerializeField] PopupMessageManager popupPrefab;

    public List<TriviaSO> triviaList;
    TriviaSO trivia;
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
        triviaList = Resources.LoadAll<TriviaSO>("TriviaSO").ToList();;
    }
    private async void Start(){
        await OpenLoadingUI();
        PlayerData playerData = AccountManager.Instance.playerData;

        SceneManager.LoadSceneAsync(playerData.gameData.lastLocationVisited, LoadSceneMode.Additive).completed += OnSceneLoaded;
        CheckActiveQuest();
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
    public void TogglePlayerUI(){
        playerUI.SetActive(!playerUI.activeSelf);
        playerCanvasGroup.interactable = !playerCanvasGroup.interactable;
    }
    public void ToggleMintingUI(){
        mintingUI.SetActive(!mintingUI.activeSelf);
        if(PlayerController.GetInstance() != null){
            if(mintingUI.activeSelf){
                PlayerController.GetInstance().playerActionMap.Disable();
                PlayerController.GetInstance().mintingActionMap.Enable();
            }else{
                PlayerController.GetInstance().playerActionMap.Enable();
                PlayerController.GetInstance().mintingActionMap.Disable();
            }
        }
    }
    public void TogglePremiumShop(){
        if(PlayerController.GetInstance() != null){
            if(!PlayerController.GetInstance().canAccessInventory) {
                PlayerUIManager.GetInstance().SpawnMessage(MType.Error, "You can't access premium shop in a combat location.");
                return;
            }
            premiumUI.SetActive(!premiumUI.activeSelf);
            if(premiumUI.activeSelf){
                PlayerController.GetInstance().playerActionMap.Disable();
                PlayerController.GetInstance().shopActionMap.Enable();
            }else{
                PlayerController.GetInstance().playerActionMap.Enable();
                PlayerController.GetInstance().shopActionMap.Disable();
            }
        }
    }
    public async Task ClosePlayerUI(){
        playerCanvasGroup.interactable = false;
        await playerCanvasGroup.DOFade(0, fadeDuration).SetEase((Ease)fadeEaseType).SetUpdate(true).AsyncWaitForCompletion();
        playerUI.SetActive(false);
    }
    public async Task OpenLoadingUI(){
        AudioManager.GetInstance().PlayMusic(MusicType.Loading, 0.6f, 0.5f);
        trivia = Utilities.GetRandomNumberFromList(triviaList);
        triviaTitle.SetText(trivia.triviaTitle);
        triviaDescription.SetText(trivia.triviaDescription);
        await OpenDarkenUI();
        loadingScreen.SetActive(true);
        await CloseDarkenUI();
    }
    public async Task CloseLoadingUI(){
        await OpenDarkenUI();
        loadingScreen.SetActive(false);
        await CloseDarkenUI();
    }
    public async Task OpenDarkenUI(){
        fadeGO.SetActive(true);
        await fade.DOFade(1, fadeDuration).SetEase((Ease)fadeEaseType).SetUpdate(true).AsyncWaitForCompletion();
    }
    public async Task CloseDarkenUI(){
        await fade.DOFade(0, fadeDuration).SetEase((Ease)fadeEaseType).SetUpdate(true).AsyncWaitForCompletion();
        fadeGO.SetActive(false);
    }
    public void OpenBackgroundUI(){
        backgroundGO.SetActive(true);
        backgroundCanvasGroup.alpha = 1f;
    }
    public async Task CloseBackgroundUI(){
        await backgroundCanvasGroup.DOFade(0, fadeDuration).SetEase((Ease)fadeEaseType).SetUpdate(true).AsyncWaitForCompletion();
        backgroundGO.SetActive(false);
    }
    public void OpenMapUI(){
        TogglePlayerUI();
        mapGO.SetActive(true);
        mapCanvasGroup.interactable = false;
        mapCanvasGroup.alpha = 1f;
        worldMap.SetActive(true);
        // await mapCanvasGroup.DOFade(1, fadeDuration).SetEase((Ease)fadeEaseType).SetUpdate(true).AsyncWaitForCompletion();
        mapCanvasGroup.interactable = true;
    }
    public async Task CloseMapUI(){
        mapCanvasGroup.interactable = false;
        mapCanvasGroup.alpha = 0f;
        // await mapCanvasGroup.DOFade(0, fadeDuration).SetEase((Ease)fadeEaseType).SetUpdate(true).AsyncWaitForCompletion();
        mapGO.SetActive(false);
        worldMap.SetActive(false);
        await OpenPlayerUI();
    }
    public void TransitionMapUI(){
        OpenBackgroundUI();
        mapCanvasGroup.interactable = false;
        mapCanvasGroup.alpha = 0f;
        // await mapCanvasGroup.DOFade(0, fadeDuration).SetEase((Ease)fadeEaseType).SetUpdate(true).AsyncWaitForCompletion();
        mapGO.SetActive(false);
        worldMap.SetActive(false);
    }
    public void SpawnMessage(MType mType, string message){
        PopupMessageManager.CreatePopup(popupPrefab, popupParent.transform, mType, message);
    }
    public void TriviaLink(){
        Application.OpenURL(trivia.link);
    }
    [ContextMenu("BackToMainMenu")]
    public async void BackToMainMenu()
    {   
        PauseManager.GetInstance().pausePanel.GetComponent<LogoAnimation>().Close();
        AudioManager.GetInstance().StopAmbience();
        await ClosePlayerUI();
        await OpenDarkenUI();
        OpenBackgroundUI();
        Time.timeScale = 1f;
        await CloseBackgroundUI();
        await OpenDarkenUI();
        await OpenLoadingUI();
        SceneManager.UnloadSceneAsync(AccountManager.Instance.playerData.gameData.lastLocationVisited).completed += async (operation) => {
            await OpenDarkenUI();
            OpenBackgroundUI();
            AudioManager.GetInstance().PlayMusic(MusicType.MainMenu, 1f, 1f);
            Play.GetInstance().PlayMainMenu();
        };
 
    }
    public async Task BackToLogin()
    {
        PlayerStats.GetInstance().stopSaving = true;
        AudioManager.GetInstance().StopAmbience();
        if(PlayerController.GetInstance() != null) PlayerController.GetInstance().playerActionMap.Disable();
        mapGO.SetActive(false);
        worldMap.SetActive(false);
        smithingUI.SetActive(false);
        smithing.SetActive(false);
        await ClosePlayerUI();
        await OpenDarkenUI();
        OpenBackgroundUI();
        Time.timeScale = 1f;
        await CloseBackgroundUI();
        await OpenDarkenUI();
        await OpenLoadingUI();
        SceneManager.UnloadSceneAsync(AccountManager.Instance.playerData.gameData.lastLocationVisited).completed += async (operation) => {
            await OpenDarkenUI();
            OpenBackgroundUI();
            AudioManager.GetInstance().PlayMusic(MusicType.MainMenu, 1f, 1f);
            Play.GetInstance().PlayLogout();
        };
 
    }
    public void CheckActiveQuest()
    {
        if(PlayerStats.GetInstance().localPlayerData.gameData.uiSettings.Contains("activeQuest")){
            activeQuestPanel.SetActive(false);
            hideButton.SetActive(false);
            unHideButton.SetActive(true);
        }else{
            activeQuestPanel.SetActive(true);
            hideButton.SetActive(true);
            unHideButton.SetActive(false);
        }
    }
    public void ToggleActiveQuest()
    {
        if(PlayerStats.GetInstance().localPlayerData.gameData.uiSettings.Contains("activeQuest")){
            activeQuestPanel.SetActive(true);
            PlayerStats.GetInstance().localPlayerData.gameData.uiSettings.Remove("activeQuest");
            PlayerStats.GetInstance().isDataDirty = true;
            hideButton.SetActive(true);
            unHideButton.SetActive(false);
        }else{
            activeQuestPanel.SetActive(false);
            PlayerStats.GetInstance().localPlayerData.gameData.uiSettings.Add("activeQuest");
            PlayerStats.GetInstance().isDataDirty = true;
            hideButton.SetActive(false);
            unHideButton.SetActive(true);
        }
    }
    public void AddToUISettings(string val){
            GameData gameData = PlayerStats.GetInstance().localPlayerData.gameData;
            if(gameData.uiSettings.Contains(val)) return;
            gameData.uiSettings.Add(val);
            PlayerStats.GetInstance().isDataDirty = true;
    }
    private async void OnSceneLoaded(AsyncOperation operation)
    {
        LocationSO location = LocationSettingsManager.GetInstance().locationSettings;
       
        await CloseBackgroundUI();
        await CloseLoadingUI();
        AudioManager.GetInstance().SetAmbience(time.hours < 17 && time.hours > 7, location.background, location.hasWater);
        if(!location.canAccessCombatMode) AudioManager.GetInstance().PlayMusic(MusicType.Town, 0.6f, 1f);
        else if(location.canAccessCombatMode && location.canAccessInventory) AudioManager.GetInstance().PlayMusic(MusicType.Town, 0.6f, 1f);
        else if(location.canAccessCombatMode && !location.canAccessInventory) AudioManager.GetInstance().PlayMusic(MusicType.Combat, 0.6f, 1f);
        else AudioManager.GetInstance().PlayMusic(MusicType.MainMenu, 1f, 1f);
        await OpenPlayerUI();
        DOTween.Clear(true);
    }
}
