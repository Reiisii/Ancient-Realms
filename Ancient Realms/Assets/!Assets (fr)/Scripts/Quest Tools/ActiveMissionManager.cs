using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActiveMissionManager : MonoBehaviour
{
    [SerializeField] public GameObject gObject;
    public string missionIDRequirement;
    public int[] showAtGoal;
    public bool permanentRequirement = false;
    private void Start(){
        if(MissionManager.GetInstance().inMission && showAtGoal.Contains(MissionManager.GetInstance().mission.currentGoal)){
            gObject.SetActive(true);
        }else{
            gObject.SetActive(false);
        }
    }
    private void Update(){
        if(MissionManager.GetInstance().inMission && showAtGoal.Contains(MissionManager.GetInstance().mission.currentGoal)){
            gObject.SetActive(true);
        }else{
            gObject.SetActive(false);
        }
    }
}
