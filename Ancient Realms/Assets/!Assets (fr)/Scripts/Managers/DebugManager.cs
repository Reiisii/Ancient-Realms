using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public void resetQuestsSO(){
        List<QuestSO> quests = Resources.LoadAll<QuestSO>("QuestSO").ToList();
        ClearContent(QuestManager.GetInstance().questPanel);
        PlayerStats.GetInstance().activeQuests.Clear();
        PlayerStats.GetInstance().completedQuests.Clear();
        foreach(QuestSO quest in quests){
            quest.isActive = false;
            quest.isCompleted = false;
            quest.currentKnot = "start";
            quest.currentGoal = 0;
            foreach(Goal goal in quest.goals){
                goal.currentAmount = 0;
            }
        }
    }

    public void AddPlayerGold(int value){
        PlayerStats.GetInstance().AddGold(value);
    }
    public void AddPlayerXP(int value){
        PlayerStats.GetInstance().AddXp(value);
    }
    public void ClearContent(RectTransform cPanel)
    {
        foreach (Transform child in cPanel)
        {
            Destroy(child.gameObject);
        }
    }
}