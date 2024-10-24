using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Org.BouncyCastle.Crypto.Engines;
using UnityEngine;
using UnityEngine.Tilemaps;

public class JavelinPrefab : MonoBehaviour
{
    [SerializeField] SpriteRenderer image;
    float damage = 0;

    private void Awake(){
        image.sprite = PlayerStats.GetInstance().equippedItems[6].front;
    }
    void OnTriggerEnter2D(Collider2D hitInfo){
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        CompositeCollider2D ground = hitInfo.GetComponent<CompositeCollider2D>();
        if(enemy != null){
            Debug.Log(hitInfo.name);
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
        if(ground != null){
            Destroy(gameObject);
        }
    }
    public void SetDamage(float dmg){
        damage = dmg;
    }
}
