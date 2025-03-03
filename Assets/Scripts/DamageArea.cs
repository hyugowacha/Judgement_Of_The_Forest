using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        float damage = player.atk;

        if(other.tag == "Enemy")
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            enemy.EnterDamage(player.atk);
        }
    }
}
