using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationTracker : MonoBehaviour
{
    [SerializeField] GameObject interiorGrid;
    [SerializeField] GameObject exteriorGrid;
    void Update()
    {
        if(interiorGrid == null) return;
        if(PlayerController.GetInstance().isInterior){
            interiorGrid.SetActive(true);
            exteriorGrid.SetActive(false);
        }else{
            interiorGrid.SetActive(false);
            exteriorGrid.SetActive(true);
        }
    }
}
