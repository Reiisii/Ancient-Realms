using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] public string itemID;
    [SerializeField] public bool playerInRange = false;
    [SerializeField] public GameObject exclamationPoint;

    private void Update(){
        if(playerInRange){
            exclamationPoint.SetActive(true);
            if(PlayerController.GetInstance().playerActionMap.enabled){
                if(PlayerController.GetInstance().GetInteractPressed()){
                    PickupItem();
                }
            }
        }else{
            exclamationPoint.SetActive(false);
        }
    }
    public void PickupItem(){
        if(MissionManager.GetInstance().inMission){
            MissionSO missionSO = MissionManager.GetInstance().mission;
            MissionGoal missionGoal = missionSO.goals[missionSO.currentGoal];
            if(missionGoal.missionType.Equals(MissionGoalType.Pickup) && missionGoal.itemID == itemID){
                MissionManager.GetInstance().UpdateGoal(MissionGoalType.Pickup);
                PlayerStats.GetInstance().AddItem(int.Parse(itemID), 0, 0, 1);
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
        }else{
            var itemRelevantQuest = PlayerStats.GetInstance().activeQuests
            .Where(q => q.goals.Any(g => g.goalType == GoalTypeEnum.Find 
            && g.requiredItems.Contains(int.Parse(itemID)))).FirstOrDefault();
            if(itemRelevantQuest != null){
                QuestManager.GetInstance().UpdateFindGoal(itemRelevantQuest);
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.CompareTag("Player")){
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider){
        if(collider.gameObject.CompareTag("Player")){
            playerInRange = false;
        }
    }
}
