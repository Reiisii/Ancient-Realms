using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsManager : MonoBehaviour
{
    private static LightsManager Instance;
    public GameObject interiorLights;
    public GameObject exteriorLights;
    private void Awake(){
        if (Instance == null)
        {
            // If not, set this as the instance and make it persistent
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Found more than one Location Settings Manager in the scene");
            Destroy(gameObject);
        }
    }
    public static LightsManager GetInstance(){
        return Instance;
    }
}
