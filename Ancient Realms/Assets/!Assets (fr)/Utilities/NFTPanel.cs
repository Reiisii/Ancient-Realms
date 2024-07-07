using Solana.Unity.SDK.Nft;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class NFTPanel : MonoBehaviour
{
public static NFTPanel Instance;

    [SerializeField] GameObject AccountPanel;
    [SerializeField] GameObject itemDetailPanel;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI rarityText;
    public TextMeshProUGUI acquiredDate;
    public Image itemImage;

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

    public void ShowItemDetails(Nft nft)
    {
        AccountPanel.SetActive(false);
        itemDetailPanel.SetActive(true);
        itemNameText.SetText(nft.metaplexData.data.offchainData.name);
        descriptionText.SetText(nft.metaplexData.data.offchainData.description);
        rarityText.SetText(nft.metaplexData.data.offchainData.attributes[0].value);
        acquiredDate.SetText(nft.metaplexData.data.offchainData.attributes[1].value);
        setImage(nft.metaplexData.nftImage.file);
    }
    public void ClosePanel()
    {
        Instance.HideItemDetails();
    }
    public void HideItemDetails()
    {
        itemDetailPanel.SetActive(false);
        itemNameText.SetText("");
        descriptionText.SetText("");
        rarityText.SetText("");
        acquiredDate.SetText("");
    }
    public void setImage(Texture2D nftImage){
        Sprite sprites = Sprite.Create(nftImage, new Rect(0, 0, nftImage.width, nftImage.height), Vector2.one * 0.5f);

        // Set the sprite to the Image component
        itemImage.sprite = sprites;
    }
}