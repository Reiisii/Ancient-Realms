using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEquipment : MonoBehaviour
{
    public void ChangeSprite(Sprite sprite){
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
