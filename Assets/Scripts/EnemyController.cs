using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float MaxEnemyHp = 10000;
    public float CurEnemyHp = 10000;

    public Image barImage;
    public Image BossHealthbar;

    private void Update()
    {
        CheckHp();
    }

    private void CheckHp()
    {
        if(CurEnemyHp >= MaxEnemyHp)
        {
            BossHealthbar.gameObject.SetActive(false);
        }

        else
        {
            BossHealthbar.gameObject.SetActive(true);
            barImage.fillAmount = CurEnemyHp / MaxEnemyHp;
        }
    }

    public void EnterDamage(float Damage)
    {
        CurEnemyHp -= Damage;

        if (CurEnemyHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        BossHealthbar.gameObject.SetActive(false);
    }
}
