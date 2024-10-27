using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterSelectBundleQuest : MonoBehaviour
{
    [SerializeField] string[] quests;

    public void AddQuest(){
        foreach(string q in quests){
            QuestManager.GetInstance().StartQuest(q);
        }
    }
}
