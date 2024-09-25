using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using ESDatabase.Classes;
using Solana.Unity.SDK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MintingManager : MonoBehaviour
{
    [Header("Population Panel")]
    [SerializeField] RectTransform NFTPanel;
    public CultureEnum currentCulture;
    public List<NFTSO> nftList;
    public List<NFTSO> filteredNFTList;
    [Header("Price Information")]
    PriceData priceData;
    [SerializeField] TextMeshProUGUI priceUpdateDate;
    [SerializeField] TextMeshProUGUI solPrice;
    [SerializeField] TextMeshProUGUI culture;
    [SerializeField] TextMeshProUGUI solBalance;
    [SerializeField] Button button;
    [Header("Minting Settings")]
    private Dictionary<RarityEnum, float> rarityWeights;
    [Header("NFT Rarity Prefabs")]
    [SerializeField] NFTDisplay commonNFT;
    [SerializeField] NFTDisplay uncommonNFT;
    [SerializeField] NFTDisplay rareNFT;
    [SerializeField] NFTDisplay epicNFT;
    [SerializeField] NFTDisplay legendaryNFT;
    public int attempts = 0;
    private double previousSolBalance = 0;
    private static MintingManager Instance;
    public bool isFetching = false;
    private void Awake(){
        if (Instance == null)
        {
            // If not, set this as the instance and make it persistent
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Found more than one Minting Manager in the scene");
            Destroy(gameObject);
        }
        rarityWeights = new Dictionary<RarityEnum, float>()
        {
            { RarityEnum.Legendary, 0.01f },
            { RarityEnum.Epic, 0.5f },
            { RarityEnum.Rare, 4.49f },
            { RarityEnum.Uncommon, 20f },
            { RarityEnum.Common, 75f }
        };
        nftList = Resources.LoadAll<NFTSO>("NFTSO").ToList();
        GetNFTsByCulture(currentCulture);
    }
    
    private void OnEnable(){
        
        priceData = AccountManager.Instance.priceData;
        Web3.OnBalanceChange += OnBalanceChange;
        PopulateNFT();
    }
    private void OnDisable()
    {
        Web3.OnBalanceChange -= OnBalanceChange;
    }
    private void Update(){
        if(Utilities.CheckIfLateBy10Minutes(AccountManager.Instance.priceData.date) && !isFetching){
            solPrice.SetText("Fetching...");
            priceUpdateDate.SetText("Fetching...");
            button.interactable = false;
            isFetching = true;
            AccountManager.Instance.GetPrice();
        }else if(isFetching){
            solPrice.SetText("Fetching...");
            priceUpdateDate.SetText("Fetching...");
            button.interactable = false;
        }else{
            solPrice.SetText(Utilities.FormatSolana((double) priceData.price));
            TimeSpan timeRemaining = priceData.date.ToLocalTime().AddMinutes(10) - DateTime.Now;

            priceUpdateDate.SetText(Utilities.FormatTimeRemaining(timeRemaining).ToString());
            if(priceData.date !=null){
                button.interactable = true;
            }else{
                button.interactable = false;
            }
        }
    }
    public static MintingManager GetInstance(){
        return Instance;
    }
    public async void Mint(){
        attempts++;
        RarityEnum randomRarity = GetRandomRarity();
        List<NFTSO> filteredNFTs = GetNFTsByRarityAndCulture(randomRarity);
        if (filteredNFTs.Count > 0)
        {
            if(randomRarity == RarityEnum.Legendary){
                Debug.Log($"Attained Legendary at: {attempts} pulls");
            }
            NFTSO nft = filteredNFTs[0];
            button.interactable = false;
            await SolanaUtility.MintNFT(nft, priceData.price);
            button.interactable = true;
        }
        else
        {
            Debug.Log("No NFTs found matching the criteria.");
        }
    }
    private RarityEnum GetRandomRarity()
    {
        float totalWeight = 0f;

        // Calculate total weight of all rarities
        foreach (var weight in rarityWeights.Values)
        {
            totalWeight += weight;
        }

        // Get a random number within the total weight
        float randomValue = UnityEngine.Random.Range(0f, totalWeight);

        // Determine which rarity corresponds to the random value
        foreach (var pair in rarityWeights)
        {
            if (randomValue < pair.Value)
            {
                return pair.Key;
            }
            randomValue -= pair.Value;
        }

        // Fallback to Common rarity
        return RarityEnum.Common;
    }
    private List<NFTSO> GetNFTsByCulture(CultureEnum selectedCulture)
    {
        List<NFTSO> filteredList = new List<NFTSO>();

        foreach (NFTSO nft in nftList)
        {
            if (nft.culture == selectedCulture)
            {
                filteredList.Add(nft);
            }
        }

        return filteredList;
    }
    private List<NFTSO> GetNFTsByRarityAndCulture(RarityEnum selectedRarity)
    {
        List<NFTSO> filteredList = new List<NFTSO>();

        foreach (NFTSO nft in nftList)
        {
            if (nft.rarity == selectedRarity && nft.culture == currentCulture)
            {
                filteredList.Add(nft);
            }
        }

        return filteredList;
    }
    private void AnimateSolChange(double startValue, double endValue)
    {
        DOTween.To(() => startValue, x =>
        {
            startValue = x;
            solBalance.SetText(Utilities.FormatSolana(startValue));
        }, endValue, 1f).SetUpdate(true).SetEase(Ease.Linear);
    }
    private void OnBalanceChange(double sb)
    {
            double oldBalance = previousSolBalance;
            previousSolBalance = sb;

            AnimateSolChange(oldBalance, sb);
    }
    public void PopulateNFT(){
        ClearContent(NFTPanel);
        filteredNFTList = GetNFTsByCulture(currentCulture);
        foreach(NFTSO nft in filteredNFTList)
        {
            switch(nft.rarity){
                case RarityEnum.Common:
                    NFTDisplay common = Instantiate(commonNFT, Vector3.zero, Quaternion.identity);
                    common.transform.SetParent(NFTPanel);
                    common.transform.localScale = Vector3.one;
                    common.SetData(nft);
                break;
                case RarityEnum.Uncommon:
                    NFTDisplay uncommon = Instantiate(uncommonNFT, Vector3.zero, Quaternion.identity);
                    uncommon.transform.SetParent(NFTPanel);
                    uncommon.transform.localScale = Vector3.one;
                    uncommon.SetData(nft);
                break;
                case RarityEnum.Rare:
                    NFTDisplay rare = Instantiate(rareNFT, Vector3.zero, Quaternion.identity);
                    rare.transform.SetParent(NFTPanel);
                    rare.transform.localScale = Vector3.one;
                    rare.SetData(nft);
                break;
                case RarityEnum.Epic:
                    NFTDisplay epic = Instantiate(epicNFT, Vector3.zero, Quaternion.identity);
                    epic.transform.SetParent(NFTPanel);
                    epic.transform.localScale = Vector3.one;
                    epic.SetData(nft);
                break;
                case RarityEnum.Legendary:
                    NFTDisplay legendary = Instantiate(legendaryNFT, Vector3.zero, Quaternion.identity);
                    legendary.transform.SetParent(NFTPanel);
                    legendary.transform.localScale = Vector3.one;
                    legendary.SetData(nft);
                break;
            }
        }
    }
    public void ClearContent(RectTransform cPanel)
    {
        foreach (Transform child in cPanel)
        {
            Destroy(child.gameObject);
        }
    }
}
