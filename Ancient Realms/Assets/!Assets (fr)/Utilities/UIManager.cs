using System;
using System.Collections;
using System.Collections.Generic;
using Solana.Unity.SDK.Nft;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static void EnableAllButtons(GameObject gameObject)
    {
        // Example: Enable all buttons in the mainMenu GameObject
        Button[] buttons = gameObject.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }
    public static void DisableAllButtons(GameObject gameObject)
    {
        // Example: Enable all buttons in the mainMenu GameObject
        Button[] buttons = gameObject.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
    }
}