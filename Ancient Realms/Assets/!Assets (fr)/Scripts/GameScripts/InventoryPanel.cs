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
    [Header("Helmet Slot")]
    [SerializeField] Image helmSlot;
    [SerializeField] GameObject helmTierGO;
    [SerializeField] Image helmTierImage;
    [SerializeField] TextMeshProUGUI helmTier;
    [SerializeField] GameObject helmLevelGO;
    [SerializeField] TextMeshProUGUI helmLevel;
    [Header("Chest Slot")]
    [SerializeField] Image chestSlot;
    [SerializeField] GameObject chestTierGO;
    [SerializeField] TextMeshProUGUI chestTier;
    [SerializeField] Image chestTierImage;
    [SerializeField] GameObject chestLevelGO;
    [SerializeField] TextMeshProUGUI chestLevel;
    [Header("Waist Slot")]
    [SerializeField] Image waistSlot;
    [SerializeField] GameObject waistTierGO;
    [SerializeField] TextMeshProUGUI waistTier;
    [SerializeField] Image waistTierImage;
    [SerializeField] GameObject waistLevelGO;
    [SerializeField] TextMeshProUGUI waistLevel;
    [Header("Foot Slot")]
    [SerializeField] Image footSlot;
    [SerializeField] GameObject footTierGO;
    [SerializeField] TextMeshProUGUI footTier;
    [SerializeField] Image footTierImage;
    [SerializeField] GameObject footLevelGO;
    [SerializeField] TextMeshProUGUI footLevel;
    [Header("Main Slot")]
    [SerializeField] Image mainSlot;
    [SerializeField] GameObject mainTierGO;
    [SerializeField] TextMeshProUGUI mainTier;
    [SerializeField] Image mainTierImage;
    [SerializeField] GameObject mainLevelGO;
    [SerializeField] TextMeshProUGUI mainLevel;
    [Header("Shield Slot")]
    [SerializeField] Image shieldSlot;
    [SerializeField] GameObject shieldTierGO;
    [SerializeField] Image shieldTierImage;
    [SerializeField] TextMeshProUGUI shieldTier;
    [SerializeField] GameObject shieldLevelGO;
    [SerializeField] TextMeshProUGUI shieldLevel;
    [Header("Javelin Slot")]
    [SerializeField] Image javelinSlot;
    [SerializeField] GameObject javelinTierGO;
    [SerializeField] Image javelinTierImage;
    [SerializeField] TextMeshProUGUI javelinTier;
    [SerializeField] GameObject javelinLevelGO;
    [SerializeField] TextMeshProUGUI javelinLevel;
    [Header("Healing Slot")]
    [SerializeField] Image healSlot;
    [SerializeField] GameObject healSlotGO;
    [SerializeField] TextMeshProUGUI healSlotQuantity;
    [Header("Tier Background")]
    [SerializeField] Sprite tier1;
    [SerializeField] Sprite tier2;
    [SerializeField] Sprite tier3;
    [SerializeField] Sprite tier4;
    [SerializeField] Sprite tier5;
    [Header("Equipment Sprite")]
    [SerializeField] Sprite defaultHelmSprite;
    [SerializeField] Sprite defaultChestSprite;
    [SerializeField] Sprite defaultWaistSprite;
    [SerializeField] Sprite defaultFootSprite;
    [SerializeField] Sprite defaultMainSprite;
    [SerializeField] Sprite defaultShieldSprite;
    [SerializeField] Sprite defaultJavelinSprite;
    [SerializeField] Sprite defaultHealSprite;
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
    [Header("NFT Slots")]
    public GameObject nftSlot1;
    public GameObject nftSlot2;
    public GameObject nftSlot3;

    [Header("Script Related")]
    public InventoryTab currentTab;
    [Header("States")]
    public bool nftSelected = false;
    public Nft selectedNFT;
    public NFTSO selectedNFTSO;
    private void OnEnable(){
        Web3.OnNFTsUpdate += OnNFTsUpdate;
        PlayerStats.GetInstance().InitializeEquipments();
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
        GameData gameData = player.localPlayerData.gameData;
        playerName.SetText(gameData.playerName);
        InitializeInventory();
    }
    private void Update(){
        PlayerStats player = PlayerStats.GetInstance();
        GameData gameData = player.localPlayerData.gameData;
        List<EquipmentSO> equippedItems = player.equippedItems;
        if(equippedItems[0] != null) {
            helmSlot.sprite = equippedItems[0].image;
            helmTierGO.SetActive(true);
            helmTierImage.sprite = TierEnum(equippedItems[0].tier);
            helmTier.SetText(equippedItems[0].tier.ToString());
            helmLevelGO.SetActive(true);
            helmLevel.SetText(equippedItems[0].level.ToString());
        }else{
            helmTierGO.SetActive(false);
            helmLevelGO.SetActive(false);
        }

        if(equippedItems[1] != null) {
            chestSlot.sprite = equippedItems[1].image;
            chestTierGO.SetActive(true);
            chestTierImage.sprite = TierEnum(equippedItems[1].tier);
            chestTier.SetText(equippedItems[1].tier.ToString());
            chestLevelGO.SetActive(true);
            chestLevel.SetText(equippedItems[1].level.ToString());
        }else{
            chestTierGO.SetActive(false);
            chestLevelGO.SetActive(false);
        }

        if(equippedItems[2] != null) {
            waistSlot.sprite = equippedItems[2].image;
            waistTierGO.SetActive(true);
            waistTierImage.sprite = TierEnum(equippedItems[2].tier);
            waistTier.SetText(equippedItems[2].tier.ToString());
            waistLevelGO.SetActive(true);
            waistLevel.SetText(equippedItems[2].level.ToString());
        }else{
            waistTierGO.SetActive(false);
            waistLevelGO.SetActive(false);
        }
        if(equippedItems[3] != null) {
            footSlot.sprite = equippedItems[3].image;
            footTierGO.SetActive(true);
            footTierImage.sprite = TierEnum(equippedItems[3].tier);
            footTier.SetText(equippedItems[3].tier.ToString());
            footLevelGO.SetActive(true);
            footLevel.SetText(equippedItems[3].level.ToString());
        }else{
            footTierGO.SetActive(false);
            footLevelGO.SetActive(false);
        }
        if(equippedItems[4] != null) {
            mainSlot.sprite = equippedItems[4].image;
            mainTierGO.SetActive(true);
            mainTierImage.sprite = TierEnum(equippedItems[4].tier);
            mainTier.SetText(equippedItems[4].tier.ToString());
            mainLevelGO.SetActive(true);
            mainLevel.SetText(equippedItems[4].level.ToString());
            damageText.SetText(Utilities.ConvertToOneDecimal(player.damage));
        }else{
            damageText.SetText(Utilities.ConvertToOneDecimal(player.damage));
            mainTierGO.SetActive(false);
            mainLevelGO.SetActive(false);
        }
        if(equippedItems[5] != null) {
            shieldSlot.sprite = equippedItems[5].image;
            shieldTierGO.SetActive(true);
            shieldTierImage.sprite = TierEnum(equippedItems[5].tier);
            shieldTier.SetText(equippedItems[5].tier.ToString());
            shieldLevelGO.SetActive(true);
            shieldLevel.SetText(equippedItems[5].level.ToString());
        }else{
            shieldTierGO.SetActive(false);
            shieldLevelGO.SetActive(false);
        }
        if(equippedItems[6] != null) {
            javelinSlot.sprite = equippedItems[6].image;
            javelinTierGO.SetActive(true);
            javelinTierImage.sprite = TierEnum(equippedItems[6].tier);
            javelinTier.SetText(equippedItems[6].tier.ToString());
            javelinLevelGO.SetActive(true);
            javelinLevel.SetText(equippedItems[6].level.ToString());
        }else{
            javelinTierGO.SetActive(false);
            javelinLevelGO.SetActive(false);
        }
        if(equippedItems[7] != null) {
            healSlot.sprite = equippedItems[6].image;
            healSlotGO.SetActive(true);
            healSlotQuantity.SetText(equippedItems[6].stackCount.ToString());
        }else{
            healSlotGO.SetActive(false);
        }
        denarii.SetText(Utilities.ConvertToOneDecimal(gameData.denarii));
        hpText.SetText(Utilities.ConvertToOneDecimal(player.maxHP));
        staminaText.SetText(Utilities.ConvertToOneDecimal(player.maxStamina));
        armorText.SetText(Utilities.ConvertToOneDecimal(player.armor));
    }
    private void SortEquipments(List<EquipmentSO> inventory){
        equipments.Clear();
        weapons.Clear();
        questItems.Clear();
        items.Clear();
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
                    items.Add(equipment);
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
        SortEquipments(PlayerStats.GetInstance().inventory);
        List<EquipmentSO> equipmentToDisplay = currentTab switch
        {
            InventoryTab.Equipments => equipments.OrderByDescending(e => e.tier)  // First, sort by tier (descending)
            .ThenByDescending(e => e.level)  // Then, sort by level (descending)
            .ToList(),
            InventoryTab.Weapons => weapons.OrderByDescending(e => e.tier)  // First, sort by tier (descending)
            .ThenByDescending(e => e.level)  // Then, sort by level (descending)
            .ToList(),
            InventoryTab.Items => items.OrderByDescending(e => e.itemName)
            .ToList(),
            InventoryTab.QuestItem => questItems.OrderByDescending(e => e.itemName)
            .ToList(),
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
        EquipmentSO equip = equipment switch
        {
            "helm" => equippedItems[0],
            "chest" => equippedItems[1],
            "waist" => equippedItems[2],
            "foot" => equippedItems[3],
            _ => null
        };
        if(equip != null){
            QuestManager.GetInstance().UpdateUnequipGoal();
            switch(equipment)
            {
                case "helm":
                    EquipmentSO helmToInv = equippedItems[0];
                    ItemData itemDataHelm = new ItemData(helmToInv.equipmentId, helmToInv.tier, helmToInv.level, helmToInv.stackCount);
                    player.localPlayerData.gameData.inventory.items.Add(itemDataHelm);
                    equippedItems[0] = null;
                    AudioManager.GetInstance().PlayAudio(SoundType.HELMET, 0.5f);
                    ClearSlot(ArmorType.Helmet);
                break;
                
                case "chest":
                    EquipmentSO chestToInv = equippedItems[1];
                    ItemData itmDataChest = new ItemData(chestToInv.equipmentId, chestToInv.tier, chestToInv.level, chestToInv.stackCount);
                    player.localPlayerData.gameData.inventory.items.Add(itmDataChest);
                    equippedItems[1] = null;  // Clear from equippedItems list
                    AudioManager.GetInstance().PlayAudio(SoundType.CHEST, 0.7f);
                    ClearSlot(ArmorType.Chest);
                break;
                
                case "waist":
                    EquipmentSO waitToInv = equippedItems[2];
                    ItemData itemDataWaist = new ItemData(waitToInv.equipmentId, waitToInv.tier, waitToInv.level, waitToInv.stackCount);
                    player.localPlayerData.gameData.inventory.items.Add(itemDataWaist);
                    equippedItems[2] = null;  // Clear from equippedItems list
                    AudioManager.GetInstance().PlayAudio(SoundType.WAIST, 1f);
                    ClearSlot(ArmorType.Waist);
                break;
                case "foot":
                    EquipmentSO footToInv = equippedItems[3];
                    ItemData itemDataFoot = new ItemData(footToInv.equipmentId, footToInv.tier, footToInv.level, footToInv.stackCount);
                    player.localPlayerData.gameData.inventory.items.Add(itemDataFoot);
                    equippedItems[3] = null;  // Clear from equippedItems list
                    AudioManager.GetInstance().PlayAudio(SoundType.FOOT, 1f);
                    ClearSlot(ArmorType.Foot);
                break;
            }
            player.equippedItems = equippedItems;
            PlayerStats.GetInstance().InitializeEquipments();
            InitializeInventory();
        }else{
            PlayerUIManager.GetInstance().SpawnMessage(MType.Info, "You don't have a armor to unequip");
        }
    }
    public void UnequipWeapon(string equipment){
        PlayerStats player = PlayerStats.GetInstance();
        List<EquipmentSO> equippedItems = player.equippedItems;
        EquipmentSO weapon = equipment switch
        {
            "main" => equippedItems[4],
            "shield" => equippedItems[5],
            "javelin" => equippedItems[6],
            "consumable" => equippedItems[7],
            _ => null
        };
        if(weapon != null){
            QuestManager.GetInstance().UpdateUnequipGoal();
            switch(equipment)
            {
                case "main":
                    EquipmentSO weapToInv = equippedItems[4];
                    ItemData weapData = new ItemData(weapToInv.equipmentId, weapToInv.tier, weapToInv.level, weapToInv.stackCount);
                    player.localPlayerData.gameData.inventory.items.Add(weapData);
                    equippedItems[4] = null;
                    ClearSlot(WeaponType.Sword);
                break;
                
                case "shield":
                    EquipmentSO shieldToInv = equippedItems[5];
                    ItemData shieldData = new ItemData(shieldToInv.equipmentId, shieldToInv.tier, shieldToInv.level, shieldToInv.stackCount);
                    player.localPlayerData.gameData.inventory.items.Add(shieldData);
                    equippedItems[5] = null;  // Clear from equippedItems list
                    ClearSlot(EquipmentEnum.Shield);
                break;
                
                case "javelin":
                    EquipmentSO javelinToInv = equippedItems[6];
                    ItemData itemDataWaist = new ItemData(javelinToInv.equipmentId, javelinToInv.tier, javelinToInv.level, javelinToInv.stackCount);
                    player.localPlayerData.gameData.inventory.items.Add(itemDataWaist);
                    equippedItems[6] = null;  // Clear from equippedItems list
                    ClearSlot(WeaponType.Javelin);
                break;
                case "consumable":
                    EquipmentSO consumableToInv = equippedItems[7];
                    ItemData itemDataconsumable = new ItemData(consumableToInv.equipmentId, consumableToInv.tier, consumableToInv.level, consumableToInv.stackCount);
                    player.localPlayerData.gameData.inventory.items.Add(itemDataconsumable);
                    equippedItems[7] = null;  // Clear from equippedItems list
                    ClearSlot(EquipmentEnum.Consumable);
                break;
            }
            player.equippedItems = equippedItems;
            PlayerStats.GetInstance().InitializeEquipments();
            InitializeInventory();
            
        }else{
            PlayerUIManager.GetInstance().SpawnMessage(MType.Info, "You don't have a weapon to unequip");
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
        try{
        foreach(Nft nftChainData in accountNft){
            if(nftChainData.metaplexData.data.offchainData.name.Equals("Eagle's Shadow")){
                    InventoryNFT nft = Instantiate(inventoryNFT, Vector3.zero, Quaternion.identity);
                    NFTSO nftData = AccountManager.Instance.nfts.FirstOrDefault(nft => nft.id == int.Parse(nftChainData.metaplexData.data.offchainData.attributes[3].value));
                    nft.transform.SetParent(nftRectTransform);
                    nft.transform.localScale = new Vector3(1, 1, 1);
                    nft.setNFT(nftChainData, nftData);  
            }
        }
        }catch(Exception err){
            return;
        }
    }
    public void ChangeType(string type)
    {
        if(nftSelected) {
            nftSelected = false;
            selectedNFT = null;
        }
        currentTab = type switch
        {
            "equipments" => InventoryTab.Equipments,
            "weapons" => InventoryTab.Weapons,
            "items" => InventoryTab.Items,
            "questItems" => InventoryTab.QuestItem,
            "nfts" => InventoryTab.NFTs,
            _ => InventoryTab.Equipments
        };
        InitializeInventory();
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
    public void ClearSlot(WeaponType weapon)
    {
        GameData gameData = PlayerStats.GetInstance().localPlayerData.gameData;
        switch(weapon){
            case WeaponType.Sword:
                gameData.equippedData.mainSlot = null;
                mainSlot.sprite = defaultMainSprite;
                PlayerStats.GetInstance().isDataDirty = true;
            break;
            case WeaponType.Spear:
                gameData.equippedData.mainSlot = null;
                mainSlot.sprite = defaultMainSprite;
                PlayerStats.GetInstance().isDataDirty = true;
            break;
            case WeaponType.Javelin:
                gameData.equippedData.javelinSlot = null;
                javelinSlot.sprite = defaultJavelinSprite;
                PlayerStats.GetInstance().isDataDirty = true;
            break;
        }
    }
    public void ClearSlot(EquipmentEnum equipment)
    {
        GameData gameData = PlayerStats.GetInstance().localPlayerData.gameData;
        switch(equipment){
            case EquipmentEnum.Shield:
                gameData.equippedData.shieldSlot = null;
                shieldSlot.sprite = defaultShieldSprite;
                PlayerStats.GetInstance().isDataDirty = true;
            break;
            case EquipmentEnum.Consumable:
                gameData.equippedData.bandageSlot = null;
                healSlot.sprite = defaultHealSprite;
                PlayerStats.GetInstance().isDataDirty = true;
            break;
        }
    }
    public void ChangeType(InventoryTab tab)
    {
        if(nftSelected) nftSelected = false;
        currentTab = tab switch
        {
            InventoryTab.Equipments => InventoryTab.Equipments,
            InventoryTab.Weapons => InventoryTab.Weapons,
            InventoryTab.Items => InventoryTab.Items,
            InventoryTab.QuestItem => InventoryTab.QuestItem,
            InventoryTab.NFTs => InventoryTab.NFTs,
            _ => InventoryTab.Equipments
        };
        InitializeInventory();
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
    public void DeselectAllNFT(){
        foreach (Transform child in inventoryRectTransform)
        {
            InventoryNFT inventoryNFT = child.GetComponent<InventoryNFT>();
            if (inventoryNFT != null)
            {
                inventoryNFT.Deselect();
            }
        }
        nftSelected = false;
        selectedNFT = null;
    }
    [ContextMenu("AddItem")]
    public void AddItem(){
        PlayerStats playerStats = PlayerStats.GetInstance();
        playerStats.AddItem(0, 5, 5, 1);
        playerStats.AddItem(1, 4, 1, 1);
        playerStats.AddItem(2, 3, 1, 1);
        playerStats.AddItem(3, 4, 1, 1);
        playerStats.AddItem(4, 2, 1, 1);
        playerStats.AddItem(5, 5, 5, 1);
        playerStats.AddItem(6, 4, 1, 1);
        playerStats.AddItem(7, 3, 1, 1);
        playerStats.AddItem(8, 5, 5, 1);
        playerStats.AddItem(9, 4, 1, 1);
        playerStats.AddItem(10, 3, 1, 1);
        playerStats.AddItem(11, 5, 5, 1);
        playerStats.AddItem(12, 5, 5, 1);
        playerStats.AddItem(14, 4, 3, 1);
        playerStats.AddItem(15, 3, 5, 1);
        playerStats.AddItem(16, 2, 1, 1);
        playerStats.AddItem(17, 2, 1, 1);
        playerStats.AddItem(18, 5, 5, 1);
        playerStats.AddItem(19, 4, 3, 1);
        playerStats.AddItem(20, 1, 1, 1);
        playerStats.AddItem(21, 0, 0, 1);
        playerStats.AddItem(22, 1, 1, 1);
        playerStats.AddItem(23, 5, 5, 1);
        playerStats.AddItem(24, 4, 2, 1);
        playerStats.AddItem(25, 1, 1, 1);
        playerStats.AddItem(27, 0, 0, 10);
        playerStats.AddItem(28, 0, 0, 32);
        playerStats.AddItem(33, 0, 0, 8);
    }
    public Sprite TierEnum(int tier){
        switch(tier){
            case 1:
                return tier1;
            case 2:
                return tier2;
            case 3:
                return tier3;
            case 4:
                return tier4;
            case 5:
                return tier5;
        }
        return null;
    }
}
