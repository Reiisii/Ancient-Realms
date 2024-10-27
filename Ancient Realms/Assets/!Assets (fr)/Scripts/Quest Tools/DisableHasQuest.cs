using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DisableHasQuest : MonoBehaviour
{
    // Update is called once per frame
    [SerializeField] private string requirementQuest;
    [SerializeField] private string disableRequirementQuest;
    void Update()
    {
        QuestSO questActive = PlayerStats.GetInstance().activeQuests.Find(quest => quest.questID == disableRequirementQuest);
        QuestSO questCompleted = PlayerStats.GetInstance().completedQuests.Find(quest => quest.questID == disableRequirementQuest);
        QuestSO requirementNotCompleted = PlayerStats.GetInstance().completedQuests.Find(quest => quest.questID == requirementQuest);
        if(questActive != null || questCompleted != null || requirementNotCompleted == null){
            gameObject.GetComponent<Button>().interactable = false;
        }else{
            gameObject.GetComponent<Button>().interactable = true;
        }
    }
}
