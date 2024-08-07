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

    void Update()
    {
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
    }
}
