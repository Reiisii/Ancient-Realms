using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ESDatabase.Classes;
using ESDatabase.Entities;
using UnityEngine;
using UnityEngine.UI;

public class EnableQuestManager : MonoBehaviour
{
    public string questIDRequirement;
    private void Start(){
        PlayerData playerData = AccountManager.Instance.playerData;
        QuestData questData = playerData.gameData.quests.Find(quest => quest.questID == questIDRequirement);
        if(questData == null){
            gameObject.GetComponent<Button>().interactable = false;
        }else if(questData != null && questData.completed){
            gameObject.GetComponent<Button>().interactable = true;
        }else if(questData != null && !questData.completed){
            gameObject.GetComponent<Button>().interactable = false;
        }
    }
    private void Update(){
        PlayerData playerData = AccountManager.Instance.playerData;
        QuestData questData = playerData.gameData.quests.Find(quest => quest.questID == questIDRequirement);
        if(questData == null){
            gameObject.GetComponent<Button>().interactable = false;
        }else if(questData != null && questData.completed){
            gameObject.GetComponent<Button>().interactable = true;
        }else if(questData != null && !questData.completed){
            gameObject.GetComponent<Button>().interactable = false;
        }
    }
}
