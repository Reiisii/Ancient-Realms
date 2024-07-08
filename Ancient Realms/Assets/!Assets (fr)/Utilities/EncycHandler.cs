using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;

public class EncycHandler : MonoBehaviour
{
    public static EncycHandler Instance;
    [Header("Character | Equipment | Artifacts")]
    [SerializeField] GameObject EncycPanel;
    [SerializeField] GameObject dataPanel;
    [SerializeField] TextMeshProUGUI panelName;
    [SerializeField] Image dataImage;
    [SerializeField] TextMeshProUGUI dataNameText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [Header("Location Panel.")]
    [SerializeField] GameObject locationPanel;
    [SerializeField] TextMeshProUGUI locationTitle;
    [SerializeField] Image locationImage;
    [SerializeField] TextMeshProUGUI locationNameText;
    [SerializeField] TextMeshProUGUI locationDescription;

    
    [Header("NFT Panel.")]
    [SerializeField] GameObject nftPanel;
    [SerializeField] Image nftImage;
    [SerializeField] TextMeshProUGUI nftText;
    [SerializeField] TextMeshProUGUI nftDescription;
    [SerializeField] TextMeshProUGUI nftRarity;

    


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
        locationPanel.SetActive(true);
        locationNameText.SetText(locations.name);
        locationTitle.SetText("Location");
        locationDescription.SetText(locations.description);
        yield return StartCoroutine(LoadImageLocation(locations.imagePath));
    }

    public IEnumerator ShowItemDetails(equipmentData equipments)
    {
        EncycPanel.SetActive(false);
        dataPanel.SetActive(true);
        dataNameText.SetText(equipments.name);
        panelName.SetText("Equipment");
        descriptionText.SetText(equipments.description);
        yield return StartCoroutine(LoadImage(equipments.imagePath));
    }

    public IEnumerator ShowItemDetails(artifactsData artifacts)
    {
        EncycPanel.SetActive(false);
        dataPanel.SetActive(true);
        dataNameText.SetText(artifacts.name);
        panelName.SetText("Artifact");
        descriptionText.SetText(artifacts.description);
        yield return StartCoroutine(LoadImage(artifacts.imagePath));
    }
    public IEnumerator ShowItemDetails(nftData nft)
    {
        EncycPanel.SetActive(false);
        nftPanel.SetActive(true);
        nftText.SetText(nft.name);
        nftDescription.SetText(nft.description);
        nftRarity.SetText(nft.rarity);
        yield return StartCoroutine(LoadImageNFT(nft.imagePath));
    }
    public void ClosePanel()
    {
        Instance.HideItemDetails();
    }
    public void HideItemDetails()
    {
        Instance.dataNameText.SetText("");
        Instance.descriptionText.SetText("");
        Instance.dataImage.sprite = null;
        dataPanel.SetActive(false);
    }
    public void setImage(Texture2D img){
        Sprite sprites = Sprite.Create(img, new Rect(0, 0, img.width, img.height), Vector2.one * 0.5f);

        // Set the sprite to the Image component
        dataImage.sprite = sprites;
    }
    public void setImageLocation(Texture2D img){
        Sprite sprites = Sprite.Create(img, new Rect(0, 0, img.width, img.height), Vector2.one * 0.5f);

        // Set the sprite to the Image component
        locationImage.sprite = sprites;
    }
    public void setImageNFT(Texture2D img){
        Sprite sprites = Sprite.Create(img, new Rect(0, 0, img.width, img.height), Vector2.one * 0.5f);

        // Set the sprite to the Image component
        nftImage.sprite = sprites;
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
    IEnumerator LoadImageLocation(string imagePath)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(imagePath))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                setImageLocation(texture);
            }
            else
            {
                Debug.LogError("Failed to load image: " + www.error);
            }
        }
    }
    IEnumerator LoadImageNFT(string imagePath)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(imagePath))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                setImageNFT(texture);
            }
            else
            {
                Debug.LogError("Failed to load image: " + www.error);
            }
        }
    }
}
