using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject tutorialGO;
    [SerializeField] GameObject backpackToolTip;
    [SerializeField] GameObject journalToolTip;
    [SerializeField] GameObject premiumToolTip;
    [SerializeField] GameObject mapToolTip;
    [SerializeField] GameObject pauseToolTip;
    [SerializeField] GameObject movementToolTip;
    [SerializeField] GameObject combatKeysTooltip;
    [SerializeField] GameObject questToolTip;
    [SerializeField] GameObject playerHPStaminaToolTip;
    public int currentTutorial = 0;
    
    private void Awake(){
        currentTutorial = 0;
        backpackToolTip.SetActive(true);
    }

    private void OnEnable(){
        if(currentTutorial == 0) backpackToolTip.SetActive(true);
        if(PlayerController.GetInstance().playerActionMap.enabled){
            PlayerController.GetInstance().playerActionMap.Disable();
        }
    }

    private void OnDisable(){
        if(!PlayerController.GetInstance().playerActionMap.enabled){
            PlayerController.GetInstance().playerActionMap.Enable();
        }
    }
    public void NextTooltip(){
        currentTutorial++;
        switch(currentTutorial){
            case 0:
                backpackToolTip.SetActive(true);
                journalToolTip.SetActive(false);
                premiumToolTip.SetActive(false);
                mapToolTip.SetActive(false);
                pauseToolTip.SetActive(false);
                movementToolTip.SetActive(false);
                combatKeysTooltip.SetActive(false);
                questToolTip.SetActive(false);
                playerHPStaminaToolTip.SetActive(false);
            break;
            case 1:
                backpackToolTip.SetActive(false);
                journalToolTip.SetActive(true);
                premiumToolTip.SetActive(false);
                mapToolTip.SetActive(false);
                pauseToolTip.SetActive(false);
                movementToolTip.SetActive(false);
                combatKeysTooltip.SetActive(false);
                questToolTip.SetActive(false);
                playerHPStaminaToolTip.SetActive(false);
            break;
            case 2:
                backpackToolTip.SetActive(false);
                journalToolTip.SetActive(false);
                premiumToolTip.SetActive(true);
                mapToolTip.SetActive(false);
                pauseToolTip.SetActive(false);
                movementToolTip.SetActive(false);
                combatKeysTooltip.SetActive(false);
                questToolTip.SetActive(false);
                playerHPStaminaToolTip.SetActive(false);
            break;
            case 3:
                backpackToolTip.SetActive(false);
                journalToolTip.SetActive(false);
                premiumToolTip.SetActive(false);
                mapToolTip.SetActive(true);
                pauseToolTip.SetActive(false);
                movementToolTip.SetActive(false);
                combatKeysTooltip.SetActive(false);
                questToolTip.SetActive(false);
                playerHPStaminaToolTip.SetActive(false);
            break;
            case 4:
                backpackToolTip.SetActive(false);
                journalToolTip.SetActive(false);
                premiumToolTip.SetActive(false);
                mapToolTip.SetActive(false);
                pauseToolTip.SetActive(true);
                movementToolTip.SetActive(false);
                combatKeysTooltip.SetActive(false);
                questToolTip.SetActive(false);
                playerHPStaminaToolTip.SetActive(false);
            break;
            case 5:
                backpackToolTip.SetActive(false);
                journalToolTip.SetActive(false);
                premiumToolTip.SetActive(false);
                mapToolTip.SetActive(false);
                pauseToolTip.SetActive(false);
                movementToolTip.SetActive(true);
                combatKeysTooltip.SetActive(false);
                questToolTip.SetActive(false);
                playerHPStaminaToolTip.SetActive(false);
            break;
            case 6:
                backpackToolTip.SetActive(false);
                journalToolTip.SetActive(false);
                premiumToolTip.SetActive(false);
                mapToolTip.SetActive(false);
                pauseToolTip.SetActive(false);
                movementToolTip.SetActive(false);
                combatKeysTooltip.SetActive(true);
                questToolTip.SetActive(false);
                playerHPStaminaToolTip.SetActive(false);
            break;
            case 7:
                backpackToolTip.SetActive(false);
                journalToolTip.SetActive(false);
                premiumToolTip.SetActive(false);
                mapToolTip.SetActive(false);
                pauseToolTip.SetActive(false);
                movementToolTip.SetActive(false);
                combatKeysTooltip.SetActive(false);
                questToolTip.SetActive(true);
                playerHPStaminaToolTip.SetActive(false);
            break;
            case 8:
                backpackToolTip.SetActive(false);
                journalToolTip.SetActive(false);
                premiumToolTip.SetActive(false);
                mapToolTip.SetActive(false);
                pauseToolTip.SetActive(false);
                movementToolTip.SetActive(false);
                combatKeysTooltip.SetActive(false);
                questToolTip.SetActive(false);
                playerHPStaminaToolTip.SetActive(true);
            break;
            case 9:
                currentTutorial = 0;
                tutorialGO.SetActive(false);
                backpackToolTip.SetActive(false);
                journalToolTip.SetActive(false);
                premiumToolTip.SetActive(false);
                mapToolTip.SetActive(false);
                pauseToolTip.SetActive(false);
                movementToolTip.SetActive(false);
                combatKeysTooltip.SetActive(false);
                questToolTip.SetActive(false);
                playerHPStaminaToolTip.SetActive(false);
            break;
        }
    }
    public void SkipTutorial(){
        currentTutorial = 0;
        tutorialGO.SetActive(false);
        backpackToolTip.SetActive(false);
        journalToolTip.SetActive(false);
        premiumToolTip.SetActive(false);
        mapToolTip.SetActive(false);
        pauseToolTip.SetActive(false);
        movementToolTip.SetActive(false);
        combatKeysTooltip.SetActive(false);
        questToolTip.SetActive(false);
        playerHPStaminaToolTip.SetActive(false);
        PlayerUIManager.GetInstance().AddToUISettings("tutorial");
    }
    public void ToggleShow(bool tutorial){
        Debug.Log(tutorial);
    }
}
