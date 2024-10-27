using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnergyDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI energy;
    void Update()
    {
        energy.SetText(PlayerStats.GetInstance().localPlayerData.gameData.currentEnergy + "/90");
    }
}
