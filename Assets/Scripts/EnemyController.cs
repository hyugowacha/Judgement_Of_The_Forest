using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    float EnemyHp = 1000;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void EnterDamage(float Damage)
    {
        EnemyHp -= Damage;
    }

    private void Die()
    {
        Debug.Log("die");
    }
}
