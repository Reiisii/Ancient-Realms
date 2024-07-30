using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(QuestManager))]
public class QuestManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        QuestManager questManager = (QuestManager)target;

        if (questManager.questList != null && questManager.questList.quests != null)
        {
            foreach (var quest in questManager.questList.quests)
            {
                EditorGUILayout.LabelField("Quest ID", quest.questID);
                EditorGUILayout.LabelField("Title", quest.questTitle);
                EditorGUILayout.LabelField("Description", quest.questDescription);
                EditorGUILayout.LabelField("Chapter", quest.chapter.ToString());
                EditorGUILayout.LabelField("Main Quest", quest.isMain.ToString());
                EditorGUILayout.LabelField("Active", quest.isActive.ToString());
                EditorGUILayout.LabelField("Completed", quest.completed.ToString());

                EditorGUILayout.LabelField("Goals:");
                foreach (var goal in quest.goals)
                {
                    EditorGUILayout.LabelField("  Goal ID", goal.goalID);
                    EditorGUILayout.LabelField("  Description", goal.goalDescription);
                    EditorGUILayout.LabelField("  Type", goal.goalType);
                    EditorGUILayout.LabelField("  Required", goal.requiredAmount.ToString());
                    EditorGUILayout.LabelField("  Current", goal.currentAmount.ToString());
                }

                EditorGUILayout.LabelField("Rewards:");
                foreach (var reward in quest.rewards)
                {
                    EditorGUILayout.LabelField("  Type", reward.rewardType);
                    EditorGUILayout.LabelField("  Value", reward.value.ToString());
                }

                EditorGUILayout.Space();
            }
        }
    }
}