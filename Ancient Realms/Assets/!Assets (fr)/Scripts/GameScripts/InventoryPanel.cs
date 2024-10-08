using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ESDatabase.Classes;
using Solana.Unity.SDK;
using Solana.Unity.SDK.Nft;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
    [Header("Details")]
    [SerializeField] TextMeshProUGUI playerName;
    [Header("Equipment")]
    [SerializeField] Image helmSlot;
    [SerializeField] Image chestSlot;
    [SerializeField] Image waistSlot;
    [SerializeField] Image footSlot;
    [SerializeField] Image mainSlot;
    [SerializeField] Image shieldSlot;
    [SerializeField] Image javelinSlot;
    [Header("Stats")]
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI staminaText;
    [SerializeField] TextMeshProUGUI armorText;
    [SerializeField] TextMeshProUGUI damageText;
    [Header("Money")]
    [SerializeField] TextMeshProUGUI denarii;
    [Header("Game Objects")]
    [SerializeField] GameObject inventoryGO;
    [SerializeField] GameObject nftGO;
    [Header("Buttons")]
    [SerializeField] Button equipmentBtn;
    [SerializeField] Button weaponsBtn;
    [SerializeField] Button itemsBtn;
    [SerializeField] Button questItemBtn;
    [SerializeField] Button nftsBtn;
    
    [Header("Content Panels")]
    [SerializeField] RectTransform inventoryRectTransform;
    [SerializeField] RectTransform nftRectTransform;
    [Header("Prefabs")]
    [SerializeField] EquipmentPrefab equipmentPrefab;
    [SerializeField] InventoryNFT inventoryNFT;
    [Header("Inventory")]
    public List<EquipmentSO> equipments;
    public List<EquipmentSO> weapons;
    public List<EquipmentSO> questItems;
    public List<EquipmentSO> items;
    [Header("Account Related")]
    List<Nft> accountNft;
    int nftTotal;

    [Header("Script Related")]
    public InventoryTab currentTab;
    private void OnEnable(){
        Web3.OnNFTsUpdate += OnNFTsUpdate;
        LoadPlayerData(PlayerStats.GetInstance());
        InitializeNFT();
    }
    private void OnDisable(){
        Web3.OnNFTsUpdate -= OnNFTsUpdate;
        ClearContent(nftRectTransform);
        ClearContent(inventoryRectTransform);
        equipments.Clear();
        weapons.Clear();
        questItems.Clear();
        items.Clear();
    }

    private void LoadPlayerData(PlayerStats player){
        GameData gameData = player.localPlayerData.gameData;
        SortEquipments(player.inventory);
        PlayerStats.GetInstance().InitializeEquipments(gameData);
        List<EquipmentSO> equippedItems = player.equippedItems;
        playerName.SetText(gameData.playerName);
        helmSlot.sprite = equippedItems[0].image;
        chestSlot.sprite = equippedItems[1].image;
        waistSlot.sprite = equippedItems[2].image;
        footSlot.sprite = equippedItems[3].image;
        mainSlot.sprite = equippedItems[4].image;
        shieldSlot.sprite = equippedItems[5].image;
        javelinSlot.sprite = equippedItems[6].image;
        denarii.SetText(Utilities.FormatNumber(gameData.denarii));
        hpText.SetText(Utilities.FormatNumber((int) player.maxHP).ToString());
        staminaText.SetText(Utilities.FormatNumber((int) player.maxStamina).ToString());
        armorText.SetText(Utilities.FormatNumber((int) player.armor).ToString());
        damageText.SetText(Utilities.FormatNumber((int) equippedItems[4].baseDamage).ToString());
        
        InitializeInventory();
    }
    private void SortEquipments(List<EquipmentSO> inventory){
        foreach(EquipmentSO equipment in inventory){
            switch(equipment.equipmentType){
                case EquipmentEnum.Armor:
                    equipments.Add(equipment);
                break;
                case EquipmentEnum.Shield:
                    equipments.Add(equipment);
                break;
                case EquipmentEnum.Weapon:
                    weapons.Add(equipment);
                break;
                case EquipmentEnum.QuestItem:
                    questItems.Add(equipment);
                break;
                case EquipmentEnum.Consumable:
                    equipments.Add(equipment);
                break;
                case EquipmentEnum.Item:
                    items.Add(equipment);
                break;
            }
        }
    }
    private void InitializeInventory(){
        ClearContent(nftRectTransform);
        ClearContent(inventoryRectTransform);
        List<EquipmentSO> equipmentToDisplay = currentTab switch
        {
            InventoryTab.Equipments => equipments,
            InventoryTab.Weapons => weapons,
            InventoryTab.Items => items,
            InventoryTab.QuestItem => questItems,
            _ => new List<EquipmentSO>()
        };
        switch(currentTab){
            case InventoryTab.Equipments:
                equipmentBtn.interactable = false;
                weaponsBtn.interactable = true;
                //itemsBtn.interactable = true;
                questItemBtn.interactable = true;
                nftsBtn.interactable = true;
            break;
            case InventoryTab.Weapons:
                equipmentBtn.interactable = true;
                weaponsBtn.interactable = false;
                //itemsBtn.interactable = true;
                questItemBtn.interactable = true;
                nftsBtn.interactable = true;
            break;
            case InventoryTab.Items:
                equipmentBtn.interactable = true;
                weaponsBtn.interactable = true;
                //itemsBtn.interactable = false;
                questItemBtn.interactable = true;
                nftsBtn.interactable = true;
            break;
            case InventoryTab.QuestItem:
                equipmentBtn.interactable = true;
                weaponsBtn.interactable = true;
                //itemsBtn.interactable = true;
                questItemBtn.interactable = false;
                nftsBtn.interactable = true;
            break;
            case InventoryTab.NFTs:
                equipmentBtn.interactable = true;
                weaponsBtn.interactable = true;
                //itemsBtn.interactable = true;
                questItemBtn.interactable = true;
                nftsBtn.interactable = false;
            break;
        }
        if(currentTab == InventoryTab.NFTs){
            InitializeNFT();
            nftGO.SetActive(true);
            inventoryGO.SetActive(false);
        }else{
            foreach(EquipmentSO equipmentSO in equipmentToDisplay)
            {
                    EquipmentPrefab eqPrefab = Instantiate(equipmentPrefab, Vector3.zero, Quaternion.identity);
                    eqPrefab.transform.SetParent(inventoryRectTransform);
                    eqPrefab.transform.localScale = Vector3.one;
                    eqPrefab.SetData(equipmentSO);
            }
            nftGO.SetActive(false);
            inventoryGO.SetActive(true);
        }
    }
    private void OnNFTsUpdate(List<Nft> nfts, int total)
    {
        accountNft = nfts;
        nftTotal = total;
        InitializeNFT();
    }
    public void InitializeNFT(){
        ClearContent(nftRectTransform);
        if (accountNft == null) return;
        if (accountNft.Count < 1) return;
        for(int i = 0; i < nftTotal; i++){
            if(accountNft[i].metaplexData.data.offchainData.attributes.Count > 3){
                if(accountNft[i].metaplexData.data.offchainData.attributes[4].value.Equals("Eagle's Shadow")){
                    InventoryNFT nft = Instantiate(inventoryNFT, Vector3.zero, Quaternion.identity);
                    NFTSO nftData = AccountManager.Instance.nfts.FirstOrDefault(nft => nft.id == int.Parse(accountNft[i].metaplexData.data.offchainData.attributes[3].value));
                    if (nftData != null) {
                        Debug.Log(nftData.nftName);
                    } else {
                        Debug.LogWarning($"No matching NFT found for ID: {accountNft[i].metaplexData.data.offchainData.attributes[3].value}");
                    }
                    nft.transform.SetParent(nftRectTransform);
                    nft.transform.localScale = new Vector3(1, 1, 1);
                    nft.setNFT(accountNft[i], nftData);
                }
                
            }
        }
    }
    public void ChangeType(string type)
    {
        currentTab = type switch
        {
            "equipments" => InventoryTab.Equipments,
            "weapons" => InventoryTab.Weapons,
            "items" => InventoryTab.Items,
            "questItems" => InventoryTab.QuestItem,
            "nfts" => InventoryTab.NFTs,
            _ => InventoryTab.Equipments
        };

        ClearContent(inventoryRectTransform);
        InitializeInventory();
    }
    public void ChangeType(InventoryTab tab)
    {
        currentTab = tab switch
        {
            InventoryTab.Equipments => InventoryTab.Equipments,
            InventoryTab.Weapons => InventoryTab.Weapons,
            InventoryTab.Items => InventoryTab.Items,
            InventoryTab.QuestItem => InventoryTab.QuestItem,
            InventoryTab.NFTs => InventoryTab.NFTs,
            _ => InventoryTab.Equipments
        };

        ClearContent(inventoryRectTransform);
        InitializeInventory();
    }
    public void ClearContent(RectTransform cPanel)
    {
        foreach (Transform child in cPanel)
        {
            Destroy(child.gameObject);
        }
    }
    [ContextMenu("AddItem")]
    public void AddItem(){
        GameData gameData = PlayerStats.GetInstance().localPlayerData.gameData;
        ItemData item1 = new ItemData(11, 3, 2, 1);
        ItemData item2 = new ItemData(12, 3, 1, 1);
        gameData.inventory.items.Add(item1);
        gameData.inventory.items.Add(item2);
        PlayerStats.GetInstance().SaveInventoryToServer();
    }
}
