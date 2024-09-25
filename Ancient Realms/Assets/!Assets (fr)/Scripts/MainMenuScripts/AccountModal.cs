using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Solana.Unity.SDK.Nft;
using Solana.Unity.Wallet;
using TMPro;
using DG.Tweening;
using Solana.Unity.SDK;
using Unity.VisualScripting;
using System.Linq;
using System;
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
    private NFTSO[] nftArray;
    // private IEnumerator coroutine;
    void Awake(){
        nftArray = Resources.LoadAll<NFTSO>("NFTSO").OrderBy(nft => nft.id).ToArray();
    }
    void Start()
    {
        // InitializeAccount();
    }
    private void OnEnable()
    {
        Web3.OnBalanceChange += OnBalanceChange;
        Web3.OnNFTsUpdate += OnNFTsUpdate;

        // Initialize account data
        InitializeAccount();
        InitializeNFT();
    }

    private void OnDisable()
    {
        // Unregister event listeners
        Web3.OnBalanceChange -= OnBalanceChange;
        Web3.OnNFTsUpdate -= OnNFTsUpdate;
        ClearContent();
    }

    public void InitializeAccount(){
        account = Web3.Wallet.Account;
        PubKeyDisplay.SetText(account.PublicKey.ToString());
        BalanceDisplay.SetText(accountBalance.ToString());
    }
    public void InitializeNFT(){
        ClearContent(contentPanel);
        if (accountNft == null) return;
        if (accountNft.Count < 1) return;
        for(int i = 0; i < nftTotal; i++){
            if(accountNft[i].metaplexData.data.offchainData.attributes.Count > 3){
                if(accountNft[i].metaplexData.data.offchainData.attributes[4].value.Equals("Eagle's Shadow")){
                    NftItems nft = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                    NFTSO nftData = nftArray.FirstOrDefault(nft => nft.id == int.Parse(accountNft[i].metaplexData.data.offchainData.attributes[3].value));
                    if (nftData != null) {
                        Debug.Log(nftData.nftName);
                    } else {
                        Debug.LogWarning($"No matching NFT found for ID: {accountNft[i].metaplexData.data.offchainData.attributes[3].value}");
                    }
                    nft.transform.SetParent(contentPanel);
                    nft.transform.localScale = new Vector3(1, 1, 1);
                    nft.setGameObject(accountPanel);
                    nft.setNFT(accountNft[i], nftData);
                }
                
            }
        }
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