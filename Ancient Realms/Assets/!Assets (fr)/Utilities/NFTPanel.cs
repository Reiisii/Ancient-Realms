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

    public void ShowItemDetails(Nft nft, NFTSO nftSO)
    {
        itemDetailPanel.SetActive(true);
        itemNameText.SetText(nftSO.nftName);
        descriptionText.SetText(nftSO.description);
        rarityText.SetText(nftSO.rarity.ToString());
        acquiredDate.SetText(nft.metaplexData.data.offchainData.attributes[1].value);
        itemImage.sprite = nftSO.image;
    }
}