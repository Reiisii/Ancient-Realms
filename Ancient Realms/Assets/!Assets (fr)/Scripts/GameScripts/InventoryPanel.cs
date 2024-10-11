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
    [Header("Equipment Sprite")]
    [SerializeField] Sprite defaultHelmSprite;
    [SerializeField] Sprite defaultChestSprite;
    [SerializeField] Sprite defaultWaistSprite;
    [SerializeField] Sprite defaultFootSprite;
    [SerializeField] Sprite defaultMainSprite;
    [SerializeField] Sprite defaultShieldSprite;
    [SerializeField] Sprite defaultJavelinSprite;
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
    [SerializeField] ItemPrefab itemPrefab;
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

    public void LoadPlayerData(PlayerStats player){
        player.equippedItems.Clear();
        GameData gameData = player.localPlayerData.gameData;
        SortEquipments(player.inventory);
        playerName.SetText(gameData.playerName);
        InitializeInventory();
    }
    private void Update(){
        PlayerStats player = PlayerStats.GetInstance();
        GameData gameData = player.localPlayerData.gameData;
        List<EquipmentSO> equippedItems = player.equippedItems;
        if(equippedItems[0] != null) helmSlot.sprite = equippedItems[0].image;
        if(equippedItems[1] != null)chestSlot.sprite = equippedItems[1].image;
        if(equippedItems[2] != null)waistSlot.sprite = equippedItems[2].image;
        if(equippedItems[3] != null)footSlot.sprite = equippedItems[3].image;
        if(equippedItems[4] != null)mainSlot.sprite = equippedItems[4].image;
        if(equippedItems[5] != null)shieldSlot.sprite = equippedItems[5].image;
        if(equippedItems[6] != null)javelinSlot.sprite = equippedItems[6].image;
        denarii.SetText(Utilities.FormatNumber(gameData.denarii));
        hpText.SetText(Utilities.FormatNumber((int) player.maxHP).ToString());
        staminaText.SetText(Utilities.FormatNumber((int) player.maxStamina).ToString());
        armorText.SetText(Utilities.FormatNumber((int) player.armor).ToString());
        damageText.SetText(Utilities.FormatNumber((int) equippedItems[4].baseDamage).ToString());
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
    public void InitializeInventory(){
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
        }else if(currentTab == InventoryTab.Items || currentTab == InventoryTab.QuestItem){
            foreach(EquipmentSO equipmentSO in equipmentToDisplay)
            {
                    ItemPrefab itmPrefab = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                    itmPrefab.transform.SetParent(inventoryRectTransform);
                    itmPrefab.transform.localScale = Vector3.one;
                    itmPrefab.SetData(equipmentSO);
            }
            nftGO.SetActive(false);
            inventoryGO.SetActive(true);
        }else{
            foreach(EquipmentSO equipmentSO in equipmentToDisplay)
            {
                    EquipmentPrefab eqPrefab = Instantiate(equipmentPrefab, Vector3.zero, Quaternion.identity);
                    eqPrefab.transform.SetParent(inventoryRectTransform);
                    eqPrefab.transform.localScale = Vector3.one;
                    eqPrefab.SetData(equipmentSO, gameObject);
            }
            nftGO.SetActive(false);
            inventoryGO.SetActive(true);
        }
    }
    public void UnequipArmor(string equipment){
        PlayerStats player = PlayerStats.GetInstance();
        List<EquipmentSO> equippedItems = player.equippedItems;

        switch(equipment)
        {
            case "helm":
                EquipmentSO helmToInv = equippedItems[0];
                ItemData itemDataHelm = new ItemData(helmToInv.equipmentId, helmToInv.tier, helmToInv.level, helmToInv.stackCount);
                player.localPlayerData.gameData.inventory.items.Add(itemDataHelm);
                equippedItems[0] = null;  // Clear from equippedItems list
                ClearSlot(ArmorType.Helmet);
            break;
            
            case "chest":
                EquipmentSO chestToInv = equippedItems[1];
                ItemData itmDataChest = new ItemData(chestToInv.equipmentId, chestToInv.tier, chestToInv.level, chestToInv.stackCount);
                player.localPlayerData.gameData.inventory.items.Add(itmDataChest);
                equippedItems[1] = null;  // Clear from equippedItems list
                ClearSlot(ArmorType.Chest);
            break;
            
            case "waist":
                EquipmentSO waitToInv = equippedItems[2];
                ItemData itemDataWaist = new ItemData(waitToInv.equipmentId, waitToInv.tier, waitToInv.level, waitToInv.stackCount);
                player.localPlayerData.gameData.inventory.items.Add(itemDataWaist);
                equippedItems[2] = null;  // Clear from equippedItems list
                ClearSlot(ArmorType.Waist);
            break;
            
            case "foot":
                EquipmentSO footToInv = equippedItems[3];
                ItemData itemDataFoot = new ItemData(footToInv.equipmentId, footToInv.tier, footToInv.level, footToInv.stackCount);
                player.localPlayerData.gameData.inventory.items.Add(itemDataFoot);
                equippedItems[3] = null;  // Clear from equippedItems list
                ClearSlot(ArmorType.Foot);
            break;
        }

        player.equippedItems = equippedItems;  // Update the equippedItems list
        RefreshInventory();
        LoadPlayerData(player);
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
        try{
        for(int i = 0; i < nftTotal; i++){
            if(accountNft[i].metaplexData.data.offchainData.attributes.Count > 3){
                if(accountNft[i].metaplexData.data.offchainData.attributes[4].value.Equals("Eagle's Shadow")){
                    InventoryNFT nft = Instantiate(inventoryNFT, Vector3.zero, Quaternion.identity);
                    NFTSO nftData = AccountManager.Instance.nfts.FirstOrDefault(nft => nft.id == int.Parse(accountNft[i].metaplexData.data.offchainData.attributes[3].value));
                    nft.transform.SetParent(nftRectTransform);
                    nft.transform.localScale = new Vector3(1, 1, 1);
                    nft.setNFT(accountNft[i], nftData);
                }
                
            }
        }
        }catch(Exception err){
            return;
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
        LoadPlayerData(PlayerStats.GetInstance());
    }
    public void ClearSlot(ArmorType armor)
    {
        GameData gameData = PlayerStats.GetInstance().localPlayerData.gameData;
        switch(armor){
            case ArmorType.Helmet:
                gameData.equippedData.helmSlot = null;
                helmSlot.sprite = defaultHelmSprite;
                PlayerStats.GetInstance().isDataDirty = true;
            break;
            case ArmorType.Chest:
                gameData.equippedData.chestSlot = null;
                chestSlot.sprite = defaultChestSprite;
                PlayerStats.GetInstance().isDataDirty = true;
            break;
            case ArmorType.Waist:
                gameData.equippedData.waistSlot = null;
                waistSlot.sprite = defaultWaistSprite;
                PlayerStats.GetInstance().isDataDirty = true;
            break;
            case ArmorType.Foot:
                gameData.equippedData.footSlot = null;
                footSlot.sprite = defaultFootSprite;
                PlayerStats.GetInstance().isDataDirty = true;
            break;

        }
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
        LoadPlayerData(PlayerStats.GetInstance());
    }
    public void ClearContent(RectTransform cPanel)
    {
        foreach (Transform child in cPanel)
        {
            Destroy(child.gameObject);
        }
    }
    public void RefreshInventory()
    {
        foreach (Transform child in inventoryRectTransform)
        {
            Destroy(child.gameObject);
        }
    }
    [ContextMenu("AddItem")]
    public void AddItem(){
        GameData gameData = PlayerStats.GetInstance().localPlayerData.gameData;
        ItemData item1 = new ItemData(11, 3, 2, 1);
        ItemData item2 = new ItemData(12, 1, 1, 1);
        ItemData item3 = new ItemData(0, 2, 4, 1);
        ItemData item4 = new ItemData(5, 4, 5, 1);
        ItemData item5 = new ItemData(23, 5, 5, 1);
        ItemData item6 = new ItemData(18, 4, 1, 1);
        ItemData item7 = new ItemData(22, 0, 0, 1);
        ItemData item8 = new ItemData(21, 0, 0, 1);
        gameData.inventory.items.Add(item1);
        gameData.inventory.items.Add(item2);
        gameData.inventory.items.Add(item3);
        gameData.inventory.items.Add(item4);
        gameData.inventory.items.Add(item5);
        gameData.inventory.items.Add(item6);
        gameData.inventory.items.Add(item7);
        gameData.inventory.items.Add(item8);
        PlayerStats.GetInstance().SaveInventoryToServer();
    }
}
