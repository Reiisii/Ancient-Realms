using ESDatabase.Classes;
using Solana.Unity.SDK.Nft;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class NFTPanel : MonoBehaviour
{
public static NFTPanel Instance;

    [SerializeField] GameObject AccountPanel;
    [SerializeField] GameObject itemDetailPanel;
    [SerializeField] GameObject BurnGO;
    [SerializeField] GameObject BurnConfirmGO;
    [SerializeField] GameObject receiptGO;
    [SerializeField] GameObject burnErrorGO;
    [SerializeField] PopAnimation burningErrorPanel;
    [SerializeField] PopAnimation receipt;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI rarityText;
    public TextMeshProUGUI acquiredDate;
    public Image itemImage;
    public Nft selectedNFT;
    public NFTSO selectedNFTSO;

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
        selectedNFT = nft;
        selectedNFTSO = nftSO;
        itemDetailPanel.SetActive(true);
        itemNameText.SetText(nftSO.nftName);
        descriptionText.SetText(nftSO.description);
        rarityText.SetText(nftSO.rarity.ToString());
        acquiredDate.SetText(nft.metaplexData.data.offchainData.attributes[2].value);
        itemImage.sprite = nftSO.image;
    }
    public void ClearSelected()
    {
        selectedNFT = null;
        selectedNFTSO = null;
    }

    public async void BurnNFT(){
        AccountManager.Instance.loadingPanel.SetActive(true);
        string burnResponse = await SolanaUtility.BurnToken(selectedNFT);
        if(burnResponse.Equals("failed") || burnResponse.Equals("error")){
            AccountManager.Instance.loadingPanel.GetComponent<FadeAnimation>().Close();
            burnErrorGO.SetActive(true);
            BurnConfirmGO.GetComponent<LogoutAnimation>().Close();
            BurnGO.GetComponent<FadeAnimation>().Close();
            itemDetailPanel.SetActive(true);
        }else{
            if(AccountManager.Instance.playerData.gameData.equippedNFT[0] != null && AccountManager.Instance.playerData.gameData.equippedNFT[0].mint.Equals(selectedNFT.metaplexData.data.mint)){
                AccountManager.Instance.playerData.gameData.equippedNFT[0] = null;
                await AccountManager.SaveData(AccountManager.Instance.playerData);
            }else if(AccountManager.Instance.playerData.gameData.equippedNFT[1] != null && AccountManager.Instance.playerData.gameData.equippedNFT[1].mint.Equals(selectedNFT.metaplexData.data.mint)){
                AccountManager.Instance.playerData.gameData.equippedNFT[1] = null;
                await AccountManager.SaveData(AccountManager.Instance.playerData);
            }else if(AccountManager.Instance.playerData.gameData.equippedNFT[2] != null && AccountManager.Instance.playerData.gameData.equippedNFT[2].mint.Equals(selectedNFT.metaplexData.data.mint)){
                AccountManager.Instance.playerData.gameData.equippedNFT[2] = null;
                await AccountManager.SaveData(AccountManager.Instance.playerData);
            }
            ClearSelected();
            AccountManager.Instance.playerData.gameData.denarii += 100;
            await AccountManager.SaveData(AccountManager.Instance.playerData);
            receiptGO.GetComponent<BurnReceipt>().receiptURL = burnResponse;
            receiptGO.SetActive(true);
            AccountManager.Instance.loadingPanel.GetComponent<FadeAnimation>().Close();
            BurnConfirmGO.GetComponent<LogoutAnimation>().Close();
            BurnGO.GetComponent<FadeAnimation>().Close();
            AccountPanel.SetActive(true);
        }
    }
}