using System;
using System.Collections;
using System.Collections.Generic;
using ESDatabase.Classes;
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

    private void OnEnable(){
        LoadPlayerData(PlayerStats.GetInstance());
    }

    private void LoadPlayerData(PlayerStats player){
        GameData gameData = player.localPlayerData.gameData;
        List<EquipmentSO> equippedItems = player.equippedItems;
        playerName.SetText(gameData.playerName);
        helmSlot.sprite = equippedItems[0].image;
        chestSlot.sprite = equippedItems[1].image;
        waistSlot.sprite = equippedItems[2].image;
        footSlot.sprite = equippedItems[3].image;
        mainSlot.sprite = equippedItems[4].image;
        shieldSlot.sprite = equippedItems[5].image;
        javelinSlot.sprite = equippedItems[6].image;
        hpText.SetText(Utilities.FormatNumber((int) player.maxHP).ToString());
        staminaText.SetText(Utilities.FormatNumber((int) player.maxStamina).ToString());
        armorText.SetText(Utilities.FormatNumber((int) player.armor).ToString());
        damageText.SetText(Utilities.FormatNumber((int) equippedItems[4].baseDamage).ToString());
    }
}
