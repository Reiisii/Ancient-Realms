using System.Collections;
using System.Collections.Generic;
using Solana.Unity.SDK;
using UnityEngine;

public class CreateTestAccount : MonoBehaviour
{
    [ContextMenu("MainPlayer")]
    public void MainPlayer(){
        Web3.Instance.CreateAccount("5nD46oP3g54DmxcU3SFtnprVYKzn2j6dhuL2FaRkymWbA6vvxY2tas173H6Z37qaezKazR523PMMGh6MwDybaZFn", "xptaker45");
    }
    [ContextMenu("JV")]
    public void JV(){
        Web3.Instance.CreateAccount("57a8H8Kwzv2kdyTYFs6zgpfAWSkqPCVFhhhZnnwo26YZTb2BgQ3zhLSuoA5B7hYXxv7PZZiAxuHAJWiUF5a6FqBt", "Xptaker45");
    }
    [ContextMenu("Marcus")]
    public void Marcus(){
        Web3.Instance.CreateAccount("67fA41CQaht27vgpwv2LLorQeDFRZHvddJfwHi32VgUeZ6bHeavwEhizwWCS8nzwJDzKzo2fyBZqn8RKu7Ei3J5T", "Zetsu101");
    }
    [ContextMenu("Thyrone")]
    public void Thyrone(){
        Web3.Instance.CreateAccount("57a8H8Kwzv2kdyTYFs6zgpfAWSkqPCVFhhhZnnwo26YZTb2BgQ3zhLSuoA5B7hYXxv7PZZiAxuHAJWiUF5a6FqBt", "xptaker45");
    }
    [ContextMenu("Archie")]
    public void Archie(){
        Web3.Instance.CreateAccount("4WhJT6CMu1TMa4mgdoPoBHeDnggD8MrS65rHxS7PpX3idMCfsgt8pfeREYRihbMUWtj3CjQ7moxpu1DypGjJfoA5", "archie23");
    }
}
