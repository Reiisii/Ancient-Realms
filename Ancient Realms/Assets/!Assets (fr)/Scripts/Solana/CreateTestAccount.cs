using System.Collections;
using System.Collections.Generic;
using Solana.Unity.SDK;
using UnityEngine;

public class CreateTestAccount : MonoBehaviour
{
    public void CreateAccount(){
        Web3.Instance.CreateAccount("5nD46oP3g54DmxcU3SFtnprVYKzn2j6dhuL2FaRkymWbA6vvxY2tas173H6Z37qaezKazR523PMMGh6MwDybaZFn", "xptaker45");
    }
}
