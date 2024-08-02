using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestPrefab : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questTitle;
    [SerializeField] TextMeshProUGUI questTask;
    public QuestSO questSO;
    void Start(){
        UpdateQuestDisplay();
    }
    public void setQuestSO(QuestSO quest){
        questSO = quest;
    }
    public void UpdateQuestDisplay(){
        if(questSO.currentGoal < questSO.goals.Capacity){
            questTitle.SetText(questSO.questTitle);
            questTask.SetText("- " + questSO.goals[questSO.currentGoal].goalDescription);
        }
        
    }
}
