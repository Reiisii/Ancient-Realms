using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestPointer : MonoBehaviour
{
    [Header("Pointer Settings")]
    private Camera worldCamera;  // Camera from the world scene
    private RectTransform uiCanvas;  // The root canvas of your UI scene
    [SerializeField] GameObject pointer;
    [SerializeField] Image icon;
    public float yOffset = 1.5f;  // The base Y-offset for the quest target
    public float bobbingAmplitude = 0.05f;  // How much the pointer bobs up and down
    public float bobbingSpeed = 10f;  // The speed of the bobbing effect
    [Header("Pointer Image")]
    [SerializeField] Sprite questMarker;
    [SerializeField] Sprite targetMarker;
    [Header("Quest")]
    public QuestSO quest;
    public GameObject npcParent;
    public Vector3 npcPosition;
    public bool isNPCFound = false;
    public float screenEdgeBuffer = 50f;  // Distance from the screen edges to clamp pointer

    private void Update()
    {
        List<QuestSO> activeQuest = PlayerStats.GetInstance().activeQuests;
        List<QuestSO> completedQuest = PlayerStats.GetInstance().completedQuests;

        if(activeQuest.FirstOrDefault(q=> q.questID == quest.questID) == null && completedQuest.FirstOrDefault(q => q.questID == quest.questID) != null){
            Destroy(gameObject);
            return;
        }
        FindNPC();
        UpdatePointerPosition();
    }

    void UpdatePointerPosition()
    {
        if(PlayerController.GetInstance().cm == null) return;
        worldCamera = PlayerController.GetInstance().cm.GetComponent<Camera>();
        GoalTypeEnum goal = quest.goals[quest.currentGoal].goalType;
        if(goal == GoalTypeEnum.Talk){
            icon.sprite = questMarker;
        }else if (goal == GoalTypeEnum.HitAny || goal == GoalTypeEnum.HitJavelin || goal == GoalTypeEnum.HitMelee || goal == GoalTypeEnum.HitRange){
            icon.sprite = targetMarker;
        }

        if(goal == GoalTypeEnum.Talk || goal == GoalTypeEnum.HitAny || goal == GoalTypeEnum.HitJavelin || goal == GoalTypeEnum.HitMelee || goal == GoalTypeEnum.HitRange && quest.isPinned){
            if(isNPCFound){
                pointer.SetActive(true);
                float bobbingOffset = Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmplitude;

                // Apply Y-offset to the quest target position and include the bobbing effect
                Vector3 targetPositionWithOffset = npcPosition + new Vector3(0, yOffset + bobbingOffset, 0);

                // Convert the world position to screen space
                Vector3 screenPosition = PlayerController.GetInstance().cm.GetComponent<Camera>().WorldToScreenPoint(targetPositionWithOffset);

                // Check if the target is behind the camera
                if (screenPosition.z < 0)
                {
                    // Flip the position to the front if it's behind
                    screenPosition *= -1;
                }

                // Convert the screen position to UI space (Canvas Space)
                Vector2 pointerPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(uiCanvas, screenPosition, null, out pointerPos);

                // Clamp the pointer to the screen edges
                pointerPos.x = Mathf.Clamp(pointerPos.x, -uiCanvas.rect.width / 2 + screenEdgeBuffer, uiCanvas.rect.width / 2 - screenEdgeBuffer);
                pointerPos.y = Mathf.Clamp(pointerPos.y, -uiCanvas.rect.height / 2 + screenEdgeBuffer, uiCanvas.rect.height / 2 - screenEdgeBuffer);

                // Set the pointer's position in UI space
                gameObject.GetComponent<RectTransform>().anchoredPosition = pointerPos;
            }
        }else{
            pointer.SetActive(false);
        }
    }
    void FindNPC()
    {
        npcParent = GameObject.Find("NPCS");
        if(npcParent == null) return;
        // Get all child objects with DialogueTrigger component under the NPCs parent
        DialogueTrigger[] allNPCs = npcParent.GetComponentsInChildren<DialogueTrigger>();
        Enemy[] enemyNPC = npcParent.GetComponentsInChildren<Enemy>();
        if(quest.currentGoal <= quest.goals.Count) {
            if(quest.goals[quest.currentGoal].goalType == GoalTypeEnum.Talk){
                string npcID = quest.characters[quest.goals[quest.currentGoal].characterIndex];
                // Loop through each NPC and check if the npcID matches the target ID
                foreach (DialogueTrigger npc in allNPCs)
                {
                    
                    if (npc.npcData.id == npcID)
                    {
                        npcPosition = npc.gameObject.transform.position;
                        isNPCFound = true;
                        break;
                    }
                }
            }else if(quest.goals[quest.currentGoal].goalType == GoalTypeEnum.HitMelee){
            string npcID = quest.goals[quest.currentGoal].targetCharacters[0];
                // Loop through each NPC and check if the npcID matches the target ID
                foreach (Enemy npc in enemyNPC)
                {
                    
                    if (npc.id == npcID)
                    {
                        npcPosition = npc.gameObject.transform.position;
                        isNPCFound = true;
                        break;
                    }
                }
        }else if(quest.goals[quest.currentGoal].goalType == GoalTypeEnum.HitAny){
            string npcID = quest.goals[quest.currentGoal].targetCharacters[0];
                // Loop through each NPC and check if the npcID matches the target ID
                foreach (Enemy npc in enemyNPC)
                {
                    
                    if (npc.id == npcID)
                    {
                        npcPosition = npc.gameObject.transform.position;
                        isNPCFound = true;
                        break;
                    }
                }
        }else if(quest.goals[quest.currentGoal].goalType == GoalTypeEnum.HitJavelin){
            string npcID = quest.goals[quest.currentGoal].targetCharacters[0];
                // Loop through each NPC and check if the npcID matches the target ID
                foreach (Enemy npc in enemyNPC)
                {
                    
                    if (npc.id == npcID)
                    {
                        npcPosition = npc.gameObject.transform.position;
                        isNPCFound = true;
                        break;
                    }
                }
        }else if(quest.goals[quest.currentGoal].goalType == GoalTypeEnum.Kill){
            string npcID = quest.goals[quest.currentGoal].targetCharacters[0];
                // Loop through each NPC and check if the npcID matches the target ID
                foreach (Enemy npc in enemyNPC)
                {
                    
                    if (npc.id == npcID)
                    {
                        npcPosition = npc.gameObject.transform.position;
                        isNPCFound = true;
                        break;
                    }
                }
        }
        }
    }
    public void SetData(QuestSO questSO){
        quest = questSO;
        uiCanvas = PlayerUIManager.GetInstance().canvas.GetComponent<RectTransform>();
    }
}
