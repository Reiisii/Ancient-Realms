using UnityEngine;

public class KeyPressHandler : MonoBehaviour
{
    [SerializeField] public GameObject keyPressA;
    [SerializeField] public GameObject keyPressD;
    [SerializeField] public GameObject keyA;
    [SerializeField] public GameObject keyD;
    [SerializeField] public GameObject keyPressShift;
    [SerializeField] public GameObject keyShift;
    [SerializeField] public GameObject keyPressE;
    [SerializeField] public GameObject keyE;
    [SerializeField] public GameObject keyPressSpacebar;
    [SerializeField] public GameObject keySpacebar;
    [SerializeField] public GameObject keyPressR;
    [SerializeField] public GameObject keyR;
    [SerializeField] public GameObject keyPressW;
    [SerializeField] public GameObject keyW;
    [SerializeField] public GameObject keyPressS;
    [SerializeField] public GameObject keyS;
    [SerializeField] public GameObject key1;
    [SerializeField] public GameObject key1Press;
    [SerializeField] public GameObject key2;
    [SerializeField] public GameObject key2Press;
    [SerializeField] public GameObject key3;
    [SerializeField] public GameObject key3Press;
    [SerializeField] public GameObject key4;
    [SerializeField] public GameObject key4Press;
    [SerializeField] public GameObject key5;
    [SerializeField] public GameObject key5Press;
    [SerializeField] public GameObject CombatGO;
    [SerializeField] public GameObject CommanderGO;
    void Update()
    {
        if(PlayerStats.GetInstance().isCombatMode){
            CombatGO.SetActive(true);
        }else{
            CombatGO.SetActive(false);
        }
        if(MissionManager.GetInstance().inMission && Contubernium.Instance != null && PlayerStats.GetInstance().isCombatMode){
            CommanderGO.SetActive(true);
        }
        // Handle "A" key press
        if (Input.GetKeyDown(KeyCode.A))
        {
            keyA.SetActive(false);
            keyPressA.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            keyA.SetActive(true);
            keyPressA.SetActive(false);
        }

        // Handle "D" key press
        if (Input.GetKeyDown(KeyCode.D))
        {
            keyD.SetActive(false);
            keyPressD.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            keyD.SetActive(true);
            keyPressD.SetActive(false);
        }
        // Handle "Shift" key press
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            keyShift.SetActive(false);
            keyPressShift.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            keyShift.SetActive(true);
            keyPressShift.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            keyE.SetActive(false);
            keyPressE.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            keyE.SetActive(true);
            keyPressE.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            keySpacebar.SetActive(false);
            keyPressSpacebar.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            keySpacebar.SetActive(true);
            keyPressSpacebar.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            keyR.SetActive(false);
            keyPressR.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            keyR.SetActive(true);
            keyPressR.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            keyW.SetActive(false);
            keyPressW.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            keyW.SetActive(true);
            keyPressW.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            keyS.SetActive(false);
            keyPressS.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            keyS.SetActive(true);
            keyPressS.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            key1.SetActive(false);
            key1Press.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            key1.SetActive(true);
            key1Press.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            key2.SetActive(false);
            key2Press.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            key2.SetActive(true);
            key2Press.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            key3.SetActive(false);
            key3Press.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            key3.SetActive(true);
            key3Press.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            key4.SetActive(false);
            key4Press.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            key4.SetActive(true);
            key4Press.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            key5.SetActive(false);
            key5Press.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            key5.SetActive(true);
            key5Press.SetActive(false);
        }
    }
}
