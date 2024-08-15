using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JournalQuestGoal : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI titleText;
    public QuestSO quest;
    public Goal goal;
    void Start(){
        UpdateQuestDisplay();
    }
    void Update(){
        UpdateQuestDisplay();
    }
    public void setQuestSO(QuestSO q, Goal g){
        quest = q;
        goal = g;
    }

    public void UpdateQuestDisplay(){
        if(quest == null) return;
        if(goal.currentAmount >= goal.requiredAmount){
            titleText.SetText("<s>" + goal.goalDescription + "</s>");
        }else{
            Goal questGoal = quest.goals[quest.currentGoal];
            titleText.SetText(questGoal.goalType == GoalTypeEnum.Talk ? questGoal.goalDescription : questGoal.goalDescription + " [" + questGoal.currentAmount + "/" + questGoal.requiredAmount + "]");
        }
    }
}
