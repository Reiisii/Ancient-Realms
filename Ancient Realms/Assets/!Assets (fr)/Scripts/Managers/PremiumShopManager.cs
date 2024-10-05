using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PremiumShopManager : MonoBehaviour
{
    private static PremiumShopManager Instance;
    [Header("Game Object")]
    [SerializeField] public GameObject premiumPanel;
    private void Awake(){
        if(Instance != null){
            Debug.LogWarning("Found more than one Premium Shop Manager in the scene");
        }
        Instance = this;
    }
    public static PremiumShopManager GetInstance(){
        return Instance;
    }
    public void OpenPremiumShop()
    {
        if(premiumPanel.activeSelf == true){
            Time.timeScale = 1f;
            PlayerController.GetInstance().playerActionMap.Enable();
            PlayerController.GetInstance().mintingActionMap.Disable();
            premiumPanel.SetActive(false);
        }else{  
            Time.timeScale = 0f;
            PlayerController.GetInstance().playerActionMap.Disable();
            PlayerController.GetInstance().mintingActionMap.Enable();
            premiumPanel.SetActive(true);
        }
    }
}
