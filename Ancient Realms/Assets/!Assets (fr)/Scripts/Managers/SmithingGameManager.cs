using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmithingGameManager : MonoBehaviour
{
    [Header("Smithing UI")]
    [SerializeField] GameObject scoreboardUIGO;
    [SerializeField] GameObject smithingGame;
    [SerializeField] GameObject furnaceUIGO;
    [SerializeField] GameObject hammerUIGO;
    [SerializeField] GameObject grindstoneUIGO;
    [SerializeField] GameObject assemblyUIGO;
    [SerializeField] GameObject furnaceGO;
    [SerializeField] GameObject hammerGO;
    [SerializeField] GameObject grindstoneGO;
    [SerializeField] GameObject assemblyGO;
    [Header("Workstation Status")]
    public bool hasMaterials = false;
    public bool furnaceUsed = false;
    public bool hammerUsed = false;
    public bool grindstoneUsed = false;
    public bool assemblyUsed = false;

    [Header("Overall Game Stats")]
    public int totalScore;
    public bool inMiniGame = false;
    public bool inWorkStation = false;
    [Header("Task Score")]
    public int score = 0;
    public int weaponMade = 0;
    private static SmithingGameManager Instance;
    
    private void Awake(){
        if (Instance == null)
        {
            // If not, set this as the instance and make it persistent
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Found more than one Player UI Manager in the scene");
            Destroy(gameObject);
        }
    }
    public static SmithingGameManager GetInstance(){
        return Instance;
    }
    public void StartWorkStation(WorkStation workStation){
        if(workStation == WorkStation.Metal) {
            if(hasMaterials) {
                PlayerUIManager.GetInstance().SpawnMessage(MType.Error, "You already have the materials!");
                return;
            }
            PlayerUIManager.GetInstance().SpawnMessage(MType.Info, "Picked up materials.");
            hasMaterials = true;
            return;
        }
        if(workStation == WorkStation.Submit){
            Submit();
            return;
        }
        switch(workStation){
            case WorkStation.Furnace:
                if(!hasMaterials){
                    PlayerUIManager.GetInstance().SpawnMessage(MType.Error, "Go get some materials first!");
                    return;
                }
                if(furnaceUsed){
                    PlayerUIManager.GetInstance().SpawnMessage(MType.Error, "You already smelt the iron!");
                    return;
                }
                inWorkStation = true;
                furnaceUIGO.SetActive(true);
                furnaceGO.SetActive(true);
                scoreboardUIGO.SetActive(true);
            break;
            case WorkStation.Hammering:
                if(!hasMaterials){
                    PlayerUIManager.GetInstance().SpawnMessage(MType.Error, "Go get some materials first!");
                    return;
                }
                if(!furnaceUsed){
                    PlayerUIManager.GetInstance().SpawnMessage(MType.Error, "Smelt the iron first!");
                    return;
                }
                if(hammerUsed){
                    PlayerUIManager.GetInstance().SpawnMessage(MType.Error, "You already Hammered the weapon!");
                    return;
                }
                inWorkStation = true;
                hammerUIGO.SetActive(true);
                hammerGO.SetActive(true);
                scoreboardUIGO.SetActive(true);
            break;
            case WorkStation.Grindstone:
                if(!hasMaterials){
                    PlayerUIManager.GetInstance().SpawnMessage(MType.Error, "Go get some materials first!");
                    return;
                }
                if(!furnaceUsed){
                    PlayerUIManager.GetInstance().SpawnMessage(MType.Error, "Smelt the iron first!");
                    return;
                }
                if(!hammerUsed){
                    PlayerUIManager.GetInstance().SpawnMessage(MType.Error, "You already hammered weapon!");
                    return;
                }
                if(grindstoneUsed){
                    PlayerUIManager.GetInstance().SpawnMessage(MType.Error, "You already sharpened the weapon!");
                    return;
                }
                inWorkStation = true;
                grindstoneUIGO.SetActive(true);
                grindstoneGO.SetActive(true);
            break;
            case WorkStation.Assembly:
                if(!hasMaterials){
                    PlayerUIManager.GetInstance().SpawnMessage(MType.Error, "Go get some materials first!");
                    return;
                }
                if(!furnaceUsed){
                    PlayerUIManager.GetInstance().SpawnMessage(MType.Error, "Smelt the iron first!");
                    return;
                }
                if(!hammerUsed){
                    PlayerUIManager.GetInstance().SpawnMessage(MType.Error, "Hammer the weapon first!");
                    return;
                }
                if(!grindstoneUsed){
                    PlayerUIManager.GetInstance().SpawnMessage(MType.Error, "Sharpen the weapon first!");
                    return;
                }
                if(assemblyUsed){
                    PlayerUIManager.GetInstance().SpawnMessage(MType.Error, "You already assembled the weapon!");
                    return;
                }
                inWorkStation = true;
                assemblyUIGO.SetActive(true);
                assemblyGO.SetActive(true);
            break;
        }
        PlayerController.GetInstance().virtualCamera.gameObject.SetActive(false);
        PlayerController.GetInstance().cm.SetActive(false);
        PlayerController.GetInstance().playerActionMap.Disable();
        inWorkStation = true;
        smithingGame.SetActive(true);
    }
    public void EndWorkStation(WorkStation workStation){
        PlayerController.GetInstance().virtualCamera.gameObject.SetActive(true);
        PlayerController.GetInstance().cm.SetActive(true);
        PlayerController.GetInstance().playerActionMap.Enable();
        smithingGame.SetActive(false);
        inWorkStation = false;
        switch(workStation){
            case WorkStation.Furnace:
                inWorkStation = false;
                furnaceUIGO.SetActive(false);
                furnaceGO.SetActive(false);
            break;
            case WorkStation.Hammering:
                inWorkStation = false;
                hammerUIGO.SetActive(false);
                hammerGO.SetActive(false);
            break;
            case WorkStation.Grindstone:
                inWorkStation = false;
                grindstoneUIGO.SetActive(false);
                grindstoneGO.SetActive(false);
            break;
            case WorkStation.Assembly:
                inWorkStation = false;
                assemblyUIGO.SetActive(false);
                assemblyGO.SetActive(false);
            break;
        }
    }
    public void Submit(){
        if(hasMaterials && furnaceUsed && hammerUsed && grindstoneUsed && assemblyUsed){
            int tempScore = score;
            hasMaterials = false;
            furnaceUsed = false;
            hammerUsed = false;
            grindstoneUsed = false;
            assemblyUsed = false;
            totalScore += tempScore;
            PlayerUIManager.GetInstance().SpawnMessage(MType.Success, "Weapon delivered! You received " + tempScore +" denarii.");
            score = 0;
            weaponMade++;
        }else{
            if(!hasMaterials){
                PlayerUIManager.GetInstance().SpawnMessage(MType.Error, "Go get some materials first!");
                return;
            }
            if(!furnaceUsed){
                PlayerUIManager.GetInstance().SpawnMessage(MType.Error, "Smelt the iron first!");
                return;
            }
            if(!hammerUsed){
                PlayerUIManager.GetInstance().SpawnMessage(MType.Error, "Hammer the weapon first!");
                return;
            }
            if(!grindstoneUsed){
                PlayerUIManager.GetInstance().SpawnMessage(MType.Error, "Sharpen the weapon first!");
                return;
            }
            if(!assemblyUsed){
                PlayerUIManager.GetInstance().SpawnMessage(MType.Error, "Assemble the weapon first!");
                return;
            }
        }
    }
    public void ClearSmithingGame(){
            inMiniGame = false;
            hasMaterials = false;
            furnaceUsed = false;
            hammerUsed = false;
            grindstoneUsed = false;
            assemblyUsed = false;
            totalScore = 0;
            score = 0;
            weaponMade = 0;
    }
    public async void StartGame(){
        inMiniGame = true;
        await PlayerUIManager.GetInstance().ClosePlayerUI();
    }
    public async void EndGame(){
        ClearSmithingGame();
        PlayerStats.GetInstance().localPlayerData.gameData.denarii += totalScore;
        PlayerStats.GetInstance().isDataDirty = true;
        await PlayerUIManager.GetInstance().OpenPlayerUI();
    }
}
