using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;

public class EncycHandler : MonoBehaviour
{
    public static EncycHandler Instance;
    [SerializeField] GameObject EncycPanel;
    [SerializeField] GameObject dataPanel;
    [SerializeField] TextMeshProUGUI panelName;
    [SerializeField] Image dataImage;
    
    public TextMeshProUGUI dataNameText;
    public TextMeshProUGUI descriptionText;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public IEnumerator ShowItemDetails(characterData character)
    {
        EncycPanel.SetActive(false);
        dataPanel.SetActive(true);
        dataNameText.SetText(character.firstName + " " + character.lastName);
        panelName.SetText("Character");
        descriptionText.SetText(character.description);
        yield return StartCoroutine(LoadImage(character.imagePath));
    }

    public IEnumerator ShowItemDetails(locationData locations) 
    {
        EncycPanel.SetActive(false);
        dataPanel.SetActive(true);
        dataNameText.SetText(locations.name);
        panelName.SetText("Locations");
        descriptionText.SetText(locations.description);
        yield return StartCoroutine(LoadImage(locations.imagePath));
    }

    public IEnumerator ShowItemDetails(equipmentData equipments)
    {
        EncycPanel.SetActive(false);
        dataPanel.SetActive(true);
        dataNameText.SetText(equipments.name);
        panelName.SetText("Equipments");
        descriptionText.SetText(equipments.description);
        yield return StartCoroutine(LoadImage(equipments.imagePath));
    }

    public IEnumerator ShowItemDetails(artifactsData artifacts)
    {
        EncycPanel.SetActive(false);
        dataPanel.SetActive(true);
        dataNameText.SetText(artifacts.name);
        panelName.SetText("Artifacts");
        descriptionText.SetText(artifacts.description);
        yield return StartCoroutine(LoadImage(artifacts.imagePath));
    }

    public void ClosePanel()
    {
        Instance.HideItemDetails();
    }
    public void HideItemDetails()
    {
        dataPanel.SetActive(false);
    }
    public void setImage(Texture2D img){
        Sprite sprites = Sprite.Create(img, new Rect(0, 0, img.width, img.height), Vector2.one * 0.5f);

        // Set the sprite to the Image component
        dataImage.sprite = sprites;
    }

    IEnumerator LoadImage(string imagePath)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(imagePath))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                setImage(texture);
            }
            else
            {
                Debug.LogError("Failed to load image: " + www.error);
            }
        }
    }
}
