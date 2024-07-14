using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.UI;
public class MainMenuAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public static MainMenuAnimation Instance { get; private set;}
    [Header("Game Objects")]
    [SerializeField]
    private GameObject mainGO;
    [SerializeField]
    private GameObject updatesGO;
    [SerializeField]
    private GameObject logoGO;
    [Header("Mainmenu")]
    [SerializeField]
    private RectTransform mainMenu; // Reference to the main menu RectTransform
    [SerializeField]
    private float mainmenuDuration; 
    [SerializeField]
    private float mainmenuDistance; 
    
    [Header("Updates Menu")]
    [SerializeField]
    private RectTransform UpdatesMenu;
    [SerializeField]
    private float updateDuration; 
    [SerializeField]
    private float updateDistance; 
    [Header("Logo")]
    [SerializeField]
    private RectTransform Logo;
    [SerializeField]
    private float logoDuration; 
    [SerializeField]
    private float logoDistance; 
    private static Vector2 mainMenuStartPos;
    private static Vector2 logoStartPos;
    private static Vector2 updatesStartPos;
    
    
    

    // Currently logged-in account
    private void Awake()
    {
        // Ensure only one instance of AccountManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
        if (mainMenu != null)
        {
            mainMenuStartPos = mainMenu.anchoredPosition;
        }
        if (Logo != null)
        {
            logoStartPos = Logo.anchoredPosition;
        }
        if (UpdatesMenu != null)
        {
            updatesStartPos = UpdatesMenu.anchoredPosition;
        }
    }
    public static void triggerAnimation(){
        if (Instance == null || Instance.mainMenu == null)
        {
            Debug.LogError("MainMenuAnimation instance or mainMenu is not initialized.");
            return;
        }
        

        Vector3 menuPosition = Instance.mainMenu.position + Vector3.right * Instance.mainmenuDistance;
        Vector3 updatePosition = Instance.UpdatesMenu.position + Vector3.left * Instance.updateDistance;
        Vector3 logoPosition = Instance.Logo.position + Vector3.down * Instance.logoDistance;
        Instance.mainMenu.DOMove(menuPosition, Instance.mainmenuDuration).SetEase(Ease.InOutSine).OnComplete(() => EnableAllButtons());
        Instance.UpdatesMenu.DOMove(updatePosition, Instance.updateDuration).SetEase(Ease.InOutSine);
        Instance.Logo.DOMove(logoPosition, Instance.logoDuration).SetEase(Ease.InOutSine);;
    }
    private static void EnableAllButtons()
    {
        // Example: Enable all buttons in the mainMenu GameObject
        Button[] buttons = Instance.mainMenu.GetComponentsInChildren<Button>(true);
        foreach (Button button in buttons)
        {
            button.enabled = true;
        }
    }
    private static void DisableAllButtons()
    {
        Button[] buttons = Instance.mainMenu.GetComponentsInChildren<Button>(true);
        foreach (Button button in buttons)
        {
            button.enabled = false;
        }
    }
    public static void ResetAndDisable()
    {
        if (Instance == null || Instance.mainMenu == null)
        {
            Debug.LogError("MainMenuAnimation instance or mainMenu is not initialized.");
            return;
        }
        DisableAllButtons();
        DisableGameObject();
        Instance.mainMenu.anchoredPosition = mainMenuStartPos;
        Instance.UpdatesMenu.anchoredPosition = updatesStartPos;
        Instance.Logo.anchoredPosition = logoStartPos;
        Instance.enabled = false;
    }
    public static void Enable()
    {
        if (Instance == null || Instance.mainMenu == null)
        {
            Debug.LogError("MainMenuAnimation instance or mainMenu is not initialized.");
            return;
        }
        Instance.enabled = true;
        EnableGameObject();
        triggerAnimation();
    }
    public static void EnableGameObject(){
        if (Instance == null || Instance.updatesGO == null || Instance.logoGO  == null || Instance.mainGO  == null)
        {
            Debug.LogError("MainMenuAnimation instance or mainMenu is not initialized.");
            return;
        }
        Instance.mainGO.SetActive(true);
        Instance.logoGO.SetActive(true);
        Instance.updatesGO.SetActive(true);
    }
    private static void DisableGameObject(){
        if (Instance == null || Instance.updatesGO == null || Instance.logoGO  == null || Instance.mainGO  == null)
        {
            Debug.LogError("MainMenuAnimation instance or mainMenu is not initialized.");
            return;
        }
        Instance.mainGO.SetActive(false);
        Instance.logoGO.SetActive(false);
        Instance.updatesGO.SetActive(false);
    }
}
