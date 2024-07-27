using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Solana.Unity.SDK.Nft;
using Solana.Unity.Wallet;
using TMPro;
using DG.Tweening;
using Solana.Unity.SDK;
using Unity.VisualScripting;
public class AccountModal : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TextMeshProUGUI PubKeyDisplay;
    [SerializeField] TextMeshProUGUI BalanceDisplay;
    [SerializeField] RectTransform contentPanel;
    [SerializeField] NftItems prefab;
    [SerializeField] GameObject accountPanel;
    [SerializeField] GameObject LoadingPanel;
    Account account;
    double accountBalance;
    List<Nft> accountNft;
    int nftTotal;
    // private IEnumerator coroutine;
    void Start()
    {
        InitializeAccount();
    }
    private void OnEnable()
    {
        // Register event listeners
        Web3.OnLogin += OnLogin;
        Web3.OnBalanceChange += OnBalanceChange;
        Web3.OnNFTsUpdate += OnNFTsUpdate;

        // Initialize account data
        InitializeAccount();
        InitializeNFT();
    }

    private void OnDisable()
    {
        // Unregister event listeners
        Web3.OnLogin -= OnLogin;
        Web3.OnBalanceChange -= OnBalanceChange;
        Web3.OnNFTsUpdate -= OnNFTsUpdate;
        ClearContent();
    }

    public void InitializeAccount(){
        account = Web3.Wallet.Account;
        PubKeyDisplay.SetText(account.PublicKey.ToString());
        BalanceDisplay.SetText(accountBalance.ToString());
        AccountManager.GetPlayer(AccountManager.Instance.EntityId);
    }
    public void InitializeNFT(){
        ClearContent(contentPanel);
        if (accountNft == null) return;
        for(int i = 0; i < nftTotal; i++){
            NftItems nft = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            nft.transform.SetParent(contentPanel);
            nft.transform.localScale = new Vector3(1, 1, 1);
            nft.setGameObject(accountPanel);
            nft.setName(accountNft[i].metaplexData.data.offchainData.name);
            nft.setImage(accountNft[i].metaplexData.nftImage.file);
            nft.setNFT(accountNft[i]);
        }
    }

    private void OnLogin(Account account)
    {
        InitializeAccount();
        InitializeNFT();
    }
    private void OnBalanceChange(double solBalance)
    {
        accountBalance = solBalance;
        InitializeAccount();
    }
    private void OnNFTsUpdate(List<Nft> nfts, int total)
    {
        accountNft = nfts;
        nftTotal = total;
        InitializeAccount();
        InitializeNFT();
    }
    public void ClearContent(){
        PubKeyDisplay.SetText("");
        BalanceDisplay.SetText("0.00");
        ClearContent(contentPanel);
    }
    public void ClearContent(RectTransform cPanel)
    {
        foreach (Transform child in cPanel)
        {
            Destroy(child.gameObject);
        }
    }
}