using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartedTrigger : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;

    public void ShowAchievement(QuestSO quest){
        title.SetText(quest.questTitle);
    } 
}
