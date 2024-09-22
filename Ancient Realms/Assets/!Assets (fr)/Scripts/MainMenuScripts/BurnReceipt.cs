using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnReceipt : MonoBehaviour
{
    [SerializeField] public string receiptURL;

    public void openURL(){
        Application.OpenURL(receiptURL);
    }
}
