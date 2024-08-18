using System.Collections;
using System.Collections.Generic;
using Org.BouncyCastle.Crypto.Engines;
using UnityEngine;

public class JavelinPrefab : MonoBehaviour
{
    float damage = 0;
    void OnTriggerEnter2D(Collider2D hitInfo){
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if(enemy != null){
            Debug.Log(hitInfo.name);
            enemy.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
    public void SetDamage(float dmg){
        damage = dmg;
    }
}
