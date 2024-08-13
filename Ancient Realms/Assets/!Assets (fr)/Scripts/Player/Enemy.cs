using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ink;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stat")]
    public float currentHP = 100f;
    public string id = "dummy-00";
    public float maxHP = 100f;
    public bool isDead = false;
    public bool isDummy = false;
    public int level = 0;
    public float maxStamina = 70f;
    public float stamina = 70f;
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float staminaDepletionRate = 40f;
    public float staminaRegenRate = 10f;
    public float attackRange = 0.5f;
    public float attack = 30;
    [Header("Enemy Controller")]
    public bool invulnerable = false;
    public bool canMove = true;
    public bool isBlocking = false;

    private void Start(){
        CalculateStatsForCurrentLevel();
    }
    public void TakeDamage(float damage){
        List<QuestSO> quest = PlayerStats.GetInstance().activeQuests.ToList();
        foreach(QuestSO q in quest){
        if(q.goals[q.currentGoal].goalType == GoalTypeEnum.Damage && q.goals[q.currentGoal].targetCharacters.Contains(id)){
                QuestManager.GetInstance().UpdateDamageGoal(damage);
            } 
        }
        if(isDummy){
            foreach(QuestSO q in quest){
            if(q.goals[q.currentGoal].goalType == GoalTypeEnum.Hit && q.goals[q.currentGoal].targetCharacters.Contains(id)){
                    QuestManager.GetInstance().UpdateHitGoal();
                }
            }
        }else{
            if(invulnerable) return;
            else{
                currentHP -= damage;
                if(currentHP <= 0){
                    isDead = true;
                    gameObject.SetActive(false);
                    QuestManager.GetInstance().Update();
                    foreach(QuestSO q in quest){
                    if(q.goals[q.currentGoal].goalType == GoalTypeEnum.Kill && q.goals[q.currentGoal].targetCharacters.Contains(id)){
                            QuestManager.GetInstance().UpdateKillGoal();
                        } 
                    }
                    foreach(QuestSO q in quest){
                    if(q.goals[q.currentGoal].goalType == GoalTypeEnum.Hit && q.goals[q.currentGoal].targetCharacters.Contains(id)){
                            QuestManager.GetInstance().UpdateHitGoal();
                        }
                    }
                }
            }

        }
        
    }
    private void CalculateStatsForCurrentLevel()
    {
        // Calculate the stats based on the current level without incrementing it
        maxHP = 100f * Mathf.Pow(1.05f, level - 1); // Assuming initial maxHP is 100
        currentHP = maxHP;
        maxStamina = 70f * Mathf.Pow(1.03f, level - 1); // Assuming initial maxStamina is 70
        attack = 30f * Mathf.Pow(1.04f, level - 1); // Assuming initial attack is 30
        staminaRegenRate = 10f * Mathf.Pow(1.03f, level - 1); // Assuming initial staminaRegenRate is 10
    }
}
