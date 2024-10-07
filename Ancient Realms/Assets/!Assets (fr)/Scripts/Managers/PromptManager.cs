using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PromptManager : MonoBehaviour
{
    [SerializeField] TMP_InputField nameField;
    [SerializeField] GameObject nameGameObject;
    [SerializeField] TextMeshProUGUI errorMessage;
    [SerializeField] string activeFor;
    // Update is called once per frame
    private void OnEnable(){
        PlayerController.GetInstance().playerActionMap.Disable();
        PlayerController.GetInstance().promptActionMap.Enable();
    }
    private void Update(){
        if(PlayerController.GetInstance().GetInteractPressed() && PlayerController.GetInstance().promptActionMap.enabled){
                Submit();
        }
    }
    public void Submit(){
        string result = Utilities.ValidateName(nameField.text);

        if (result == "Name cannot be empty!" || result == "Name cannot be longer than 27 characters!")
        {
            PlayerUIManager.GetInstance().SpawnMessage(MType.Error, result);
        }
        else
        {
            Debug.Log("Validated Name: " + result);
            QuestSO quest = PlayerStats.GetInstance().activeQuests.Find(quest => quest.questID == activeFor);
            QuestManager.GetInstance().UpdatePromptGoals(quest);
            PlayerStats.GetInstance().SetName(result);
            nameGameObject.SetActive(false);
            
            PlayerController.GetInstance().playerActionMap.Enable();
            PlayerController.GetInstance().promptActionMap.Disable();
        }

    }
}
