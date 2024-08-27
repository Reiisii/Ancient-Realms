using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActiveQuestManager : MonoBehaviour
{
    [SerializeField] public GameObject gObject;
    public List<string> requiredQuest;
    private List<QuestSO> completedList;
    private void Start(){
        completedList = PlayerStats.GetInstance().completedQuests.ToList();
        if (AreAllRequiredQuestsCompleted())
        {
            gObject.SetActive(true);
        }else{
            gObject.SetActive(false);
        }
    }
    private void Update(){
        completedList = PlayerStats.GetInstance().completedQuests.ToList();
        if (AreAllRequiredQuestsCompleted())
        {
            gObject.SetActive(true);
        }else{
            gObject.SetActive(false);
        }
    }

    private bool AreAllRequiredQuestsCompleted()
    {
        return requiredQuest.All(requiredQuestID => completedList.Any(quest => quest.questID == requiredQuestID));
    }
}
