using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissionPanel : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] public RectTransform taskRectTransform;
    [SerializeField] public RectTransform rewardRectTransform;
    [Header("Mission Prefabs")]
    [SerializeField] private MissionTaskPrefab missionPrefab;
    [SerializeField] private MissionRewardPrefab rewardPrefab;
    [SerializeField] private MissionManager missionManager;
    private void Awake(){
        missionManager = MissionManager.GetInstance();
    }
    private void OnEnable(){
        foreach(MissionGoal missionGoal in missionManager.mission.goals){
            MissionTaskPrefab taskPrefab = Instantiate(missionPrefab, Vector3.zero, Quaternion.identity);
            taskPrefab.transform.SetParent(taskRectTransform);
            taskPrefab.transform.localScale = Vector3.one;
            taskPrefab.SetText(missionGoal.taskDescription);
        }
        foreach(Reward reward in missionManager.mission.rewards){
            MissionRewardPrefab taskPrefab = Instantiate(rewardPrefab, Vector3.zero, Quaternion.identity);
            taskPrefab.transform.SetParent(rewardRectTransform);
            taskPrefab.transform.localScale = Vector3.one;
            switch(reward.rewardType){
                case RewardsEnum.Xp:
                    taskPrefab.SetText($"XP: {reward.value}");
                break;
                case RewardsEnum.Gold:
                    taskPrefab.SetText($"Denarii: {reward.value}");
                break;
                case RewardsEnum.Item:
                    EquipmentSO equipmentSO = AccountManager.Instance.equipments.FirstOrDefault(eq => eq.equipmentId == Convert.ToInt32(reward.value));
                    taskPrefab.SetText(equipmentSO.itemName + " " + (reward.tier > 0 ? "T(" + reward.tier +")" +  "L(" + reward.level + ")" + "x" + reward.amount : "x" + reward.amount));
                break;
                case RewardsEnum.Artifact:
                    ArtifactsSO artifactsSO = AccountManager.Instance.achievements.FirstOrDefault(achievement => achievement.id.Equals(reward.value));
                    taskPrefab.SetText($"Artifact: {artifactsSO.artifactName}");
                break;
                case RewardsEnum.Quest:
                    QuestSO questSO = AccountManager.Instance.quests.FirstOrDefault(quest => quest.questID.Equals(reward.value));
                    taskPrefab.SetText($"Artifact: {questSO.questTitle}");
                break;
                case RewardsEnum.Event:
                    EventSO events = AccountManager.Instance.events.FirstOrDefault(ev => ev.eventID.Equals(reward.value));
                    taskPrefab.SetText($"Artifact: {events.eventTitle}");
                break;
            }
        }
    }
    public async void AcceptAndContinue(){
        PlayerUIManager.GetInstance().CloseMissionPanel();
        await PlayerUIManager.GetInstance().ClosePlayerUI();
        MissionManager.GetInstance().RewardPlayer(missionManager.mission);
        PlayerUIManager.GetInstance().LastLocation();
    }
    private void OnDisable(){
        Utilities.ClearContent(taskRectTransform);
        Utilities.ClearContent(rewardRectTransform);
    }
}
