using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Create Quest", menuName = "SO/Quest")]
public class QuestSO : ScriptableObject
{

    public string questID;
    public string questTitle;
    public string questDescription;
    public TextAsset dialogue;
    public List<string> characters;
    public List<string> requirements;
    public ChapterEnum chapter;
    public NPCData npcGiver;
    public bool isMain;
    public bool isActive;
    public bool isCompleted;
    public bool isChained;
    public bool isRewarded;
    public int currentGoal;
    public string currentKnot;
    public List<Goal> goals;
    public List<Reward> rewards;
}
