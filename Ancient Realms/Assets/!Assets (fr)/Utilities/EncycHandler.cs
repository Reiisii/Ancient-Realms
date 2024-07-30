using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;
using DG.Tweening;

public class EncycHandler : MonoBehaviour
{
    public static EncycHandler Instance;
    [Header("Character | Equipment | Artifacts")]
    [SerializeField] GameObject EncycPanelGO;
    [SerializeField] GameObject dataPanel;
    [SerializeField] TextMeshProUGUI panelName;
    [SerializeField] Image dataImage;
    [SerializeField] TextMeshProUGUI dataNameText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI cultureText;
    [Header("Location Panel")]
    [SerializeField] GameObject locationPanel;
    [SerializeField] TextMeshProUGUI locationTitle;
    [SerializeField] Image locationImage;
    [SerializeField] TextMeshProUGUI locationNameText;
    [SerializeField] TextMeshProUGUI locationDescription;

    
    [Header("NFT Panel")]
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
    public void ShowItemDetails(CharacterSO character)
    {
        dataNameText.SetText(character.firstName + " " + character.lastName);
        panelName.SetText("Character");
        cultureText.SetText(Utilities.FormatCultureName(character.culture));
        descriptionText.SetText(character.biography);
        dataImage.sprite = character.image;
        dataPanel.SetActive(true);
    }

    public IEnumerator ShowItemDetails(locationData locations) 
    {
        locationNameText.SetText(locations.name);
        locationTitle.SetText("Location");
        locationDescription.SetText(locations.description);
        yield return StartCoroutine(LoadImageLocation(locations.imagePath));
    }

    public void ShowItemDetails(EquipmentSO equipment)
    {
        dataNameText.SetText(equipment.itemName);
        panelName.SetText("Equipment");
        descriptionText.SetText(equipment.description);
        cultureText.SetText(Utilities.FormatCultureName(equipment.culture));
        dataImage.sprite = equipment.image;
        dataPanel.SetActive(true);
    }

    public void ShowItemDetails(ArtifactsSO artifact)
    {
        dataNameText.SetText(artifact.artifactName);
        panelName.SetText("Equipment");
        descriptionText.SetText(artifact.description);
        cultureText.SetText(Utilities.FormatCultureName(artifact.culture));
        dataImage.sprite = artifact.image;
        dataPanel.SetActive(true);
    }
    public IEnumerator ShowItemDetails(nftData nft)
    {
        nftText.SetText(nft.name);
        nftDescription.SetText(nft.description);
        nftRarity.SetText(nft.rarity);
        yield return StartCoroutine(LoadImageNFT(nft.imagePath));
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
                dataPanel.SetActive(true);
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
                locationPanel.SetActive(true);
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
                nftPanel.SetActive(true);
            }
            else
            {
                Debug.LogError("Failed to load image: " + www.error);
            }
        }
    }
}
