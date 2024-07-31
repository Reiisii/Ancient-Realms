using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] RectTransform characterButton;
    [SerializeField] RectTransform nftCharacterButton;
    [SerializeField] RectTransform triviaButton;
    [SerializeField] RectTransform locationsButton;
    [SerializeField] RectTransform equipmentsButton;
    [SerializeField] RectTransform artifactsButton;
    [SerializeField] RectTransform eventsButton;

    [Header("Image")]
    [SerializeField] Image characterButtonImg;
    [SerializeField] Image nftCharacterButtonImg;
    [SerializeField] Image triviaButtonImg;
    [SerializeField] Image locationsButtonImg;
    [SerializeField] Image equipmentsButtonImg;
    [SerializeField] Image artifactsButtonImg;
    [SerializeField] Image eventsButtonImg;


    [Header("Panel")]
    [SerializeField] GameObject NFTPanel;
    [SerializeField] GameObject CharacterPanel;
    [SerializeField] GameObject triviaPanel;
    [SerializeField] GameObject locationsPanel;
    [SerializeField] GameObject equipmentsPanel;
    [SerializeField] GameObject artifactsPanel;
    [SerializeField] GameObject eventsPanel;

    public void Close(){
        CharacterButtonPressed();
    }
    public void CharacterButtonPressed()
    {
        // PRESSED ACTION
        characterButton.anchoredPosition = new Vector2(-650, characterButton.anchoredPosition.y);
        characterButtonImg.color = Color.white;
        

        // OTHER BUTTONS
        nftCharacterButton.anchoredPosition = new Vector2(-600, nftCharacterButton.anchoredPosition.y);
        nftCharacterButtonImg.color = new Color(.50f, .50f, .50f);

        triviaButton.anchoredPosition = new Vector2(-600, triviaButton.anchoredPosition.y);
        triviaButtonImg.color = new Color(.50f, .50f, .50f);

        locationsButton.anchoredPosition = new Vector2(-600, locationsButton.anchoredPosition.y);
        locationsButtonImg.color = new Color(.50f, .50f, .50f);

        equipmentsButton.anchoredPosition = new Vector2(-600, equipmentsButton.anchoredPosition.y);
        equipmentsButtonImg.color = new Color(.50f, .50f, .50f);

        artifactsButton.anchoredPosition = new Vector2(-600, artifactsButton.anchoredPosition.y);
        artifactsButtonImg.color = new Color(.50f, .50f, .50f);
        
        eventsButton.anchoredPosition = new Vector2(-600, eventsButton.anchoredPosition.y);
        eventsButtonImg.color = new Color(.50f, .50f, .50f);

        handlePanel("character");
    }

        public void nftButtonPressed()
    {
        // PRESSED ACTION
        nftCharacterButton.anchoredPosition = new Vector2(-650, nftCharacterButton.anchoredPosition.y);
        nftCharacterButtonImg.color = Color.white;
        

        // OTHER BUTTONS
        characterButton.anchoredPosition = new Vector2(-600, characterButton.anchoredPosition.y);
        characterButtonImg.color = new Color(.50f, .50f, .50f);

        triviaButton.anchoredPosition = new Vector2(-600, triviaButton.anchoredPosition.y);
        triviaButtonImg.color = new Color(.50f, .50f, .50f);

        locationsButton.anchoredPosition = new Vector2(-600, locationsButton.anchoredPosition.y);
        locationsButtonImg.color = new Color(.50f, .50f, .50f);

        equipmentsButton.anchoredPosition = new Vector2(-600, equipmentsButton.anchoredPosition.y);
        equipmentsButtonImg.color = new Color(.50f, .50f, .50f);

        artifactsButton.anchoredPosition = new Vector2(-600, artifactsButton.anchoredPosition.y);
        artifactsButtonImg.color = new Color(.50f, .50f, .50f);
        
        eventsButton.anchoredPosition = new Vector2(-600, eventsButton.anchoredPosition.y);
        eventsButtonImg.color = new Color(.50f, .50f, .50f);

        handlePanel("nft");
    }

        public void triviaButtonPressed()
    {
        // PRESSED ACTION
        triviaButton.anchoredPosition = new Vector2(-650, triviaButton.anchoredPosition.y);
        triviaButtonImg.color = Color.white;
        

        // OTHER BUTTONS
        nftCharacterButton.anchoredPosition = new Vector2(-600, nftCharacterButton.anchoredPosition.y);
        nftCharacterButtonImg.color = new Color(.50f, .50f, .50f);

        characterButton.anchoredPosition = new Vector2(-600, characterButton.anchoredPosition.y);
        characterButtonImg.color = new Color(.50f, .50f, .50f);

        locationsButton.anchoredPosition = new Vector2(-600, locationsButton.anchoredPosition.y);
        locationsButtonImg.color = new Color(.50f, .50f, .50f);

        equipmentsButton.anchoredPosition = new Vector2(-600, equipmentsButton.anchoredPosition.y);
        equipmentsButtonImg.color = new Color(.50f, .50f, .50f);

        artifactsButton.anchoredPosition = new Vector2(-600, artifactsButton.anchoredPosition.y);
        artifactsButtonImg.color = new Color(.50f, .50f, .50f);
        
        eventsButton.anchoredPosition = new Vector2(-600, eventsButton.anchoredPosition.y);
        eventsButtonImg.color = new Color(.50f, .50f, .50f);

        handlePanel("trivia");
    }
        public void locationsButtonPressed()
    {
        // PRESSED ACTION
        locationsButton.anchoredPosition = new Vector2(-650, locationsButton.anchoredPosition.y);
        locationsButtonImg.color = Color.white;
        

        // OTHER BUTTONS
        nftCharacterButton.anchoredPosition = new Vector2(-600, nftCharacterButton.anchoredPosition.y);
        nftCharacterButtonImg.color = new Color(.50f, .50f, .50f);

        triviaButton.anchoredPosition = new Vector2(-600, triviaButton.anchoredPosition.y);
        triviaButtonImg.color = new Color(.50f, .50f, .50f);

        characterButton.anchoredPosition = new Vector2(-600, characterButton.anchoredPosition.y);
        characterButtonImg.color = new Color(.50f, .50f, .50f);

        equipmentsButton.anchoredPosition = new Vector2(-600, equipmentsButton.anchoredPosition.y);
        equipmentsButtonImg.color = new Color(.50f, .50f, .50f);

        artifactsButton.anchoredPosition = new Vector2(-600, artifactsButton.anchoredPosition.y);
        artifactsButtonImg.color = new Color(.50f, .50f, .50f);
        
        eventsButton.anchoredPosition = new Vector2(-600, eventsButton.anchoredPosition.y);
        eventsButtonImg.color = new Color(.50f, .50f, .50f);

        handlePanel("locations");
    }
        public void equipmentsButtonPressed()
    {
        // PRESSED ACTION
        equipmentsButton.anchoredPosition = new Vector2(-650, equipmentsButton.anchoredPosition.y);
        equipmentsButtonImg.color = Color.white;
        

        // OTHER BUTTONS
        nftCharacterButton.anchoredPosition = new Vector2(-600, nftCharacterButton.anchoredPosition.y);
        nftCharacterButtonImg.color = new Color(.50f, .50f, .50f);

        triviaButton.anchoredPosition = new Vector2(-600, triviaButton.anchoredPosition.y);
        triviaButtonImg.color = new Color(.50f, .50f, .50f);

        locationsButton.anchoredPosition = new Vector2(-600, locationsButton.anchoredPosition.y);
        locationsButtonImg.color = new Color(.50f, .50f, .50f);

        characterButton.anchoredPosition = new Vector2(-600, characterButton.anchoredPosition.y);
        characterButtonImg.color = new Color(.50f, .50f, .50f);

        artifactsButton.anchoredPosition = new Vector2(-600, artifactsButton.anchoredPosition.y);
        artifactsButtonImg.color = new Color(.50f, .50f, .50f);
        
        eventsButton.anchoredPosition = new Vector2(-600, eventsButton.anchoredPosition.y);
        eventsButtonImg.color = new Color(.50f, .50f, .50f);

        handlePanel("equipments");
    }

        public void artifactsButtonPressed()
    {
        // PRESSED ACTION
        artifactsButton.anchoredPosition = new Vector2(-650, artifactsButton.anchoredPosition.y);
        artifactsButtonImg.color = Color.white;
        

        // OTHER BUTTONS
        nftCharacterButton.anchoredPosition = new Vector2(-600, nftCharacterButton.anchoredPosition.y);
        nftCharacterButtonImg.color = new Color(.50f, .50f, .50f);

        triviaButton.anchoredPosition = new Vector2(-600, triviaButton.anchoredPosition.y);
        triviaButtonImg.color = new Color(.50f, .50f, .50f);

        locationsButton.anchoredPosition = new Vector2(-600, locationsButton.anchoredPosition.y);
        locationsButtonImg.color = new Color(.50f, .50f, .50f);

        equipmentsButton.anchoredPosition = new Vector2(-600, equipmentsButton.anchoredPosition.y);
        equipmentsButtonImg.color = new Color(.50f, .50f, .50f);

        characterButton.anchoredPosition = new Vector2(-600, characterButton.anchoredPosition.y);
        characterButtonImg.color = new Color(.50f, .50f, .50f);
        
        eventsButton.anchoredPosition = new Vector2(-600, eventsButton.anchoredPosition.y);
        eventsButtonImg.color = new Color(.50f, .50f, .50f);

        handlePanel("artifacts");
    }

        public void eventsButtonPressed()
    {
        // PRESSED ACTION
        eventsButton.anchoredPosition = new Vector2(-650, eventsButton.anchoredPosition.y);
        eventsButtonImg.color = Color.white;
        

        // OTHER BUTTONS
        nftCharacterButton.anchoredPosition = new Vector2(-600, nftCharacterButton.anchoredPosition.y);
        nftCharacterButtonImg.color = new Color(.50f, .50f, .50f);

        triviaButton.anchoredPosition = new Vector2(-600, triviaButton.anchoredPosition.y);
        triviaButtonImg.color = new Color(.50f, .50f, .50f);

        locationsButton.anchoredPosition = new Vector2(-600, locationsButton.anchoredPosition.y);
        locationsButtonImg.color = new Color(.50f, .50f, .50f);

        equipmentsButton.anchoredPosition = new Vector2(-600, equipmentsButton.anchoredPosition.y);
        equipmentsButtonImg.color = new Color(.50f, .50f, .50f);

        artifactsButton.anchoredPosition = new Vector2(-600, artifactsButton.anchoredPosition.y);
        artifactsButtonImg.color = new Color(.50f, .50f, .50f);
        
        characterButton.anchoredPosition = new Vector2(-600, characterButton.anchoredPosition.y);
        characterButtonImg.color = new Color(.50f, .50f, .50f);

        handlePanel("events");
    }

    public void handlePanel(string panelName)
    {
        switch(panelName) {
            case "character": 
                CharacterPanel.SetActive(true);
                NFTPanel.SetActive(false);
                triviaPanel.SetActive(false);
                locationsPanel.SetActive(false);
                equipmentsPanel.SetActive(false);
                artifactsPanel.SetActive(false);
                eventsPanel.SetActive(false);
            break; 

            case "nft": 
                CharacterPanel.SetActive(false);
                NFTPanel.SetActive(true);
                triviaPanel.SetActive(false);
                locationsPanel.SetActive(false);
                equipmentsPanel.SetActive(false);
                artifactsPanel.SetActive(false);
                eventsPanel.SetActive(false);
            break; 

            case "trivia": 
                CharacterPanel.SetActive(false);
                NFTPanel.SetActive(false);
                triviaPanel.SetActive(true);
                locationsPanel.SetActive(false);
                equipmentsPanel.SetActive(false);
                artifactsPanel.SetActive(false);
                eventsPanel.SetActive(false);
            break;

            case "locations": 
                CharacterPanel.SetActive(false);
                NFTPanel.SetActive(false);
                triviaPanel.SetActive(false);
                locationsPanel.SetActive(true);
                equipmentsPanel.SetActive(false);
                artifactsPanel.SetActive(false);
                eventsPanel.SetActive(false);
            break;

            case "equipments": 
                CharacterPanel.SetActive(false);
                NFTPanel.SetActive(false);
                triviaPanel.SetActive(false);
                locationsPanel.SetActive(false);
                equipmentsPanel.SetActive(true);
                artifactsPanel.SetActive(false);
                eventsPanel.SetActive(false);
            break;

            case "artifacts": 
                CharacterPanel.SetActive(false);
                NFTPanel.SetActive(false);
                triviaPanel.SetActive(false);
                locationsPanel.SetActive(false);
                equipmentsPanel.SetActive(false);
                artifactsPanel.SetActive(true);
                eventsPanel.SetActive(false);
            break;

            case "events": 
                CharacterPanel.SetActive(false);
                NFTPanel.SetActive(false);
                triviaPanel.SetActive(false);
                locationsPanel.SetActive(false);
                equipmentsPanel.SetActive(false);
                artifactsPanel.SetActive(false);
                eventsPanel.SetActive(true);
            break;
        }
    }
}
