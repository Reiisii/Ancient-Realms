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
    [SerializeField] public GameObject CombatGO;
    void Update()
    {
        if(PlayerStats.GetInstance().isCombatMode){
            CombatGO.SetActive(true);
        }else{
            CombatGO.SetActive(false);
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
    }
}
