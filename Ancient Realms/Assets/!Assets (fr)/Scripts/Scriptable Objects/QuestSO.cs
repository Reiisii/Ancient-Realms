using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public bool isPinned;
    public bool isChained;
    public bool isRewarded;
    public int currentGoal;
    public string currentKnot;
    public List<Goal> goals;
    public List<Reward> rewards;
    public QuestSO CreateCopy()
    {
        // Create a new instance of QuestSO
        QuestSO newQuest = ScriptableObject.CreateInstance<QuestSO>();

        // Copy all fields from this QuestSO to the new instance
        newQuest.questID = this.questID;
        newQuest.questTitle = this.questTitle;
        newQuest.questDescription = this.questDescription;
        newQuest.dialogue = this.dialogue;
        newQuest.characters = new List<string>(this.characters);
        newQuest.requirements = new List<string>(this.requirements);
        newQuest.chapter = this.chapter;
        newQuest.npcGiver = this.npcGiver; // assuming NPCData is a reference type, otherwise create a deep copy
        newQuest.isMain = this.isMain;
        newQuest.isActive = this.isActive;
        newQuest.isCompleted = this.isCompleted;
        newQuest.isChained = this.isChained;
        newQuest.isRewarded = this.isRewarded;
        newQuest.currentGoal = this.currentGoal;
        newQuest.currentKnot = this.currentKnot;
        newQuest.goals = new List<Goal>();
        foreach (Goal goal in this.goals)
        {
            Goal newGoal = new Goal
            {
                goalID = goal.goalID,
                goalDescription = goal.goalDescription,
                goalType = goal.goalType,
                requiredAmount = goal.requiredAmount,
                currentAmount = goal.currentAmount,
                inkyRedirect = goal.inkyRedirect,
                characterIndex = goal.characterIndex,
                targetCharacters = (string[])goal.targetCharacters.Clone(),
                missionID = goal.missionID,
                questID = goal.questID,
                questItem = new List<int>(goal.questItem),
                requiredItems = new List<int>(goal.requiredItems),
                inkyNoRequirement = goal.inkyNoRequirement
            };
            newQuest.goals.Add(newGoal);
        }
        newQuest.rewards = new List<Reward>(this.rewards);

        return newQuest;
    }
}
