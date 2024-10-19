using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CommanderSystem : MonoBehaviour
{
    public void ToggleFollow(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(Contubernium.Instance != null && Contubernium.Instance.allies.Count < 1){
                PlayerUIManager.GetInstance().SpawnMessage(MType.Info, "Your Centurio is wiped out");
            }else if(Contubernium.Instance != null){
                Contubernium.Instance.ToggleFollow();
            }else{
                PlayerUIManager.GetInstance().SpawnMessage(MType.Warning, "This is not a battlefield");
            }
        }
    }
    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(Contubernium.Instance != null && Contubernium.Instance.allies.Count < 1){
                PlayerUIManager.GetInstance().SpawnMessage(MType.Info, "Your Centurio is wiped out");
            }else if(Contubernium.Instance != null){
                if(Contubernium.Instance.isAttacking) {
                    PlayerUIManager.GetInstance().SpawnMessage(MType.Info, "Centurio is currently attacking");
                    return;
                }
                Contubernium.Instance.OrderAttack();
            }else{
                PlayerUIManager.GetInstance().SpawnMessage(MType.Warning, "This is not a battlefield");
            }
        }
    }
    public void MarchFormation(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(Contubernium.Instance != null && Contubernium.Instance.allies.Count < 1){
                PlayerUIManager.GetInstance().SpawnMessage(MType.Info, "Your Centurio is wiped out");
            }else if(Contubernium.Instance != null){
                if(Contubernium.Instance.isMarch) {
                    PlayerUIManager.GetInstance().SpawnMessage(MType.Info, "Centurio is already in march formation");
                    return;
                }
                Contubernium.Instance.MarchFormation();
                
            }else{
                PlayerUIManager.GetInstance().SpawnMessage(MType.Warning, "This is not a battlefield");
            }
        }
    }
    public void CombatFormation(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(Contubernium.Instance != null && Contubernium.Instance.allies.Count < 1){
                PlayerUIManager.GetInstance().SpawnMessage(MType.Info, "Your Centurio is wiped out");
            }else if(Contubernium.Instance != null){
                if(Contubernium.Instance.isBattle) {
                    PlayerUIManager.GetInstance().SpawnMessage(MType.Info, "Centurio is already in battle formation");
                    return;
                }
                Contubernium.Instance.BattleFormation();
                
            }else{
                PlayerUIManager.GetInstance().SpawnMessage(MType.Warning, "This is not a battlefield");
            }
        }
    }
    public void ToggleCombatMode(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(Contubernium.Instance != null && Contubernium.Instance.allies.Count < 1){
                PlayerUIManager.GetInstance().SpawnMessage(MType.Info, "Your Centurio is wiped out");
            }else if(Contubernium.Instance != null){
                Contubernium.Instance.ToggleCombatMode();
            }else{
                PlayerUIManager.GetInstance().SpawnMessage(MType.Warning, "This is not a battlefield");
            }
        }
    }
}
