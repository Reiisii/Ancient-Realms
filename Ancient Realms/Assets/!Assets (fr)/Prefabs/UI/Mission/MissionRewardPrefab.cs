using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissionRewardPrefab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI task;
    public void SetText(string text){
        task.SetText(text);
    }
}
