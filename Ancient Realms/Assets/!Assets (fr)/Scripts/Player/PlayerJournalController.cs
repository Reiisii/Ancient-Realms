using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJournalController : MonoBehaviour
{
    // Start is called before the first frame update
    public void QuestCycleRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            JournalManager.GetInstance().CycleQuestRight();
        }
    }
    public void QuestCycleLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            JournalManager.GetInstance().CycleQuestLeft();
        }
    }
    public void QuestDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PlayerUIManager.GetInstance().ToggleMintingUI();
        }
    }
    public void QuestUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PlayerUIManager.GetInstance().ToggleMintingUI();
        }
    }
}
