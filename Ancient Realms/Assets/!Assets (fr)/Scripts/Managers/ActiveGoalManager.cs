using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveManager : MonoBehaviour
{
    [SerializeField] public GameObject gObject;
    public string questIDRequirement;
    public int currentGoalRequirement;
    private QuestSO quest;
    private void Start(){
        QuestSO quest = PlayerStats.GetInstance().activeQuests.Find(quest => quest.questID == questIDRequirement);
        if(quest == null) return;
        if(quest != null && quest.currentGoal == currentGoalRequirement){
            gObject.SetActive(true);
        }
        if(quest.currentGoal > currentGoalRequirement){
            gObject.SetActive(false);
        }
    }
    private void Update(){
        QuestSO quest = PlayerStats.GetInstance().activeQuests.Find(quest => quest.questID == questIDRequirement);
        if(quest == null) return;
        if(quest != null && quest.currentGoal == currentGoalRequirement){
            gObject.SetActive(true);
        }
        if(quest.currentGoal > currentGoalRequirement){
            gObject.SetActive(false);
        }
    }
}
