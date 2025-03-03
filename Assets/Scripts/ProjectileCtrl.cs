using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileCtrl : MonoBehaviour
{
    public int ENormalDamage = 50;
    public int EChargeDamage = 100;
    public int FireballDamage = 50;
    float maxTime = 4.0f;
    float nowTime = 0;

    private void Start()
    {
        
    }

    private void Update()
    {
        nowTime += Time.deltaTime;
        if(nowTime > maxTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            EnemyController enemy = other.GetComponent<EnemyController>();

            if(enemy != null)
            {
                if(gameObject.tag == "ENormal")
                {
                    enemy.EnterDamage(ENormalDamage);
                    Destroy(gameObject);
                }
                if (gameObject.tag == "ECharge")
                {
                    enemy.EnterDamage(EChargeDamage);
                    Destroy(gameObject);
                }
                if(gameObject.tag == "Fireball")
                {
                    enemy.EnterDamage(FireballDamage);
                    Destroy(gameObject);
                }
            }
        }
    }
}
