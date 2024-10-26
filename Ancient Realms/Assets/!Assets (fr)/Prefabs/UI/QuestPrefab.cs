using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestPrefab : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questTitle;
    [SerializeField] TextMeshProUGUI questTask;
    [SerializeField] TextMeshProUGUI questProgress;
    public QuestSO questSO;
    void Start(){
        UpdateQuestDisplay();
    }
    void Update(){
        UpdateQuestDisplay();
    }
    public void setQuestSO(QuestSO quest){
        questSO = quest;
    }
    public void UpdateQuestDisplay(){
        if(questSO.currentGoal < questSO.goals.Count){
            Goal questGoal = questSO.goals[questSO.currentGoal];
            questTitle.SetText(questSO.questTitle);
            questTask.SetText("- " + questGoal.goalDescription);
            questProgress.SetText(questGoal.goalType == GoalTypeEnum.Talk || questGoal.goalType == GoalTypeEnum.Prompt || questGoal.goalType == GoalTypeEnum.Mission || questGoal.goalType == GoalTypeEnum.Quest || questGoal.goalType == GoalTypeEnum.OpenBackpack || questGoal.goalType == GoalTypeEnum.OpenJournal ? "" : "[" + questGoal.currentAmount + "/" + questGoal.requiredAmount + "]");
        }
    }
}
