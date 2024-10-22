using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIActiveGoal : MonoBehaviour
{
    [SerializeField] public GameObject gObject;
    public string questIDRequirement;
    public int currentGoalRequirement;
    public bool onlyInJournal;
    public bool onlyInBackpack;
    private void Start(){
        QuestSO quest = PlayerStats.GetInstance().activeQuests.Find(quest => quest.questID == questIDRequirement);
        if(quest == null) return;
        if(quest != null && quest.currentGoal == currentGoalRequirement && onlyInJournal || onlyInBackpack){
            gObject.SetActive(true);
        }
        if(quest.currentGoal > currentGoalRequirement){
            gObject.SetActive(false);
        }
    }
    void Update()
    {
        
    }
}
