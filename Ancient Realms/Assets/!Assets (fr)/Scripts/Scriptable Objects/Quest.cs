using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
[CreateAssetMenu(fileName = "Create Quest", menuName = "SO/Quest")]
public class QuestSO : ScriptableObject
{

    public string questID;
    public string questTitle;
    public string questDescription;
    public ChapterEnum chapter;
    public bool isMain;
    public bool isActive;
    public bool isCompleted;
    public List<Goal> goals;
}
