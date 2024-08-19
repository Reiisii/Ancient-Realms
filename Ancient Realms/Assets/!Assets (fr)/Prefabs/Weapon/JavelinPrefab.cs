using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Org.BouncyCastle.Crypto.Engines;
using UnityEngine;
using UnityEngine.Tilemaps;

public class JavelinPrefab : MonoBehaviour
{
    float damage = 0;
    void OnTriggerEnter2D(Collider2D hitInfo){
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        CompositeCollider2D ground = hitInfo.GetComponent<CompositeCollider2D>();
        if(enemy != null){
            Debug.Log(hitInfo.name);
            enemy.TakeDamage(damage, GoalTypeEnum.HitJavelin);
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
