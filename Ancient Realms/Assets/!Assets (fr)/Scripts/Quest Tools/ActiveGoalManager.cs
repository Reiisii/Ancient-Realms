using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActiveManager : MonoBehaviour
{
    [SerializeField] public GameObject gObject;
    public string questIDRequirement;
    public int[] showAtGoal;
    public bool permanentRequirement = false;
    public string[] questRequirementToPermanent;
    private void Awake(){

    }
    private void Start(){
        QuestSO quest = PlayerStats.GetInstance().activeQuests.Find(quest => quest.questID == questIDRequirement);
        List<QuestSO> questCompleted = PlayerStats.GetInstance().completedQuests;
        if(quest != null && showAtGoal.Contains(quest.currentGoal)){
            gObject.SetActive(true);
        }else if(permanentRequirement && Utilities.CheckRequirements(questRequirementToPermanent)){
            gObject.SetActive(true);
        }else{
            gObject.SetActive(false);
        }
    }
    private void Update(){
        QuestSO quest = PlayerStats.GetInstance().activeQuests.Find(quest => quest.questID == questIDRequirement);
        List<QuestSO> questCompleted = PlayerStats.GetInstance().completedQuests;
        if(quest != null && showAtGoal.Contains(quest.currentGoal)){
            gObject.SetActive(true);
        }else if(permanentRequirement && Utilities.CheckRequirements(questRequirementToPermanent)){
            gObject.SetActive(true);
        }else{
            gObject.SetActive(false);
        }
    }
    
}
