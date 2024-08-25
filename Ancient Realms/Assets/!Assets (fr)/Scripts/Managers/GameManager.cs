using System.Collections;
using System.Collections.Generic;
using Solana.Unity.SDK;
using System.Runtime.InteropServices;
using Unisave.Facets;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;
    [DllImport("__Internal")]
    public static extern void Warn(string test);
    private void Awake(){
        if (Instance == null)
        {
            // If not, set this as the instance and make it persistent
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Found more than one Game Manager in the scene");
            Destroy(gameObject);
        }
    }
    public static GameManager GetInstance(){
        return Instance;
    }
    public void TriggerApplicationQuit(){
        if(!AccountManager.Instance.EntityId.Equals("")){
            FacetClient.CallFacet((DatabaseService facet) => facet.ForgotSession(Web3.Account.PublicKey));
        }
        Debug.Log("User has exited the page.");
    }
}
