using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsLoggedIn : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject connectionMenu;
    // Start is called before the first frame update
    void Start()
    {
        if(AccountManager.Instance.EntityId != "" && AccountManager.Instance.UIDInstance != ""){
            connectionMenu.SetActive(false);
            mainMenu.SetActive(true);
            
        }else{
            connectionMenu.SetActive(true);
            mainMenu.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
