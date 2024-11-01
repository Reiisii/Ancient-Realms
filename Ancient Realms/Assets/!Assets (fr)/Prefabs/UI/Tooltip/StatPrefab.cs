using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatPrefab : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Sprite hpSprite;
    [SerializeField] Sprite armorSprite;
    [SerializeField] Sprite damageSprite;
    [SerializeField] Sprite staminaSprite;
    [SerializeField] Sprite speedSprite;
    [SerializeField] Sprite rangeSprite;

    public void SetHP(string hp){
        icon.sprite = hpSprite;
        text.SetText($"HP: +{hp}");
    }
    public void SetArmor(string armor){
        icon.sprite = armorSprite;
        text.SetText($"Armor: +{armor}");
    }
    public void SetDamage(string damage){
        icon.sprite = damageSprite;
        text.SetText($"Damage: +{damage}");
    }
    public void SetStamina(string stamina){
        icon.sprite = staminaSprite;
        text.SetText($"Stamina: +{stamina}");
    }
    public void SetSpeed(string speed){
        icon.sprite = speedSprite;
        text.SetText($"Speed: +{speed}");
    }
    public void SetRange(string range){
        icon.sprite = rangeSprite;
        text.SetText($"Range: +{range}");
    }
}
