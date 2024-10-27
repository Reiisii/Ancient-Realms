using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeDisplay : MonoBehaviour
{
    // Update is called once per frame
    [SerializeField] TextMeshProUGUI clock;
    void Update()
    {
        PlayerUIManager playerUI = PlayerUIManager.GetInstance();
        int hour = playerUI.time.hours;
        int minutes = playerUI.time.mins;
        clock.SetText($"{hour:D2}:{minutes:D2}");
    }
}
