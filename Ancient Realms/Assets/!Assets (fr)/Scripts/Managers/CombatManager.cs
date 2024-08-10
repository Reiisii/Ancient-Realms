using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;
    public bool canReceiveInput;
    public bool inputReceived;
    private void Awake()
    {
        if(Instance != null){
            Debug.LogWarning("Found more than one Player Controller in the scene");
        }
        Instance = this;
    }
    
    public static CombatManager GetInstance(){
        return Instance;
    }

    public void Attack(InputAction.CallbackContext context){
        if(context.performed){
            if(PlayerStats.GetInstance().isCombatMode && canReceiveInput && PlayerController.GetInstance().IsMoving == false && PlayerController.GetInstance().IsRunning == false){
                inputReceived = true;
                canReceiveInput = false;
            }else{
                return;
            }
        }
    }
    public void InputManger(){
        if(!canReceiveInput){
            canReceiveInput = true;
        }else{
            canReceiveInput = false;
        }
    }
}
