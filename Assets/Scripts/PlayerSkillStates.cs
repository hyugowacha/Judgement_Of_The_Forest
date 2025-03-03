using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cinemachine;

public class ESkillState : IPlayerState
{
    float chargeTime;
    float maxChargeTime = 3.0f;
    bool canAttack;
    bool isEndSkill;
    bool chargeOn;
    bool isShot;

    public void EnterState(PlayerController player)
    {
        player.WeaponOff();
        player.CanStateChange = false;
        canAttack = true;
        chargeTime = 0;
        player.playerWeapon.SetActive(true);
        player.PlayerAnimator.applyRootMotion = false;
        player.PlayerAnimator.runtimeAnimatorController = player.eSkillAnimator;
        player.eSkillEffect.gameObject.SetActive(true);
        chargeOn = true;
        isShot = true;
    }

    public void UpdateState(PlayerController player)
    {
        //Debug.Log(chargeTime);
        //Debug.Log(player.CanStateChange);
        chargeTime += Time.deltaTime;

        player.AnimationInfo = player.PlayerAnimator.GetCurrentAnimatorStateInfo(0);

        if (!player.AnimationInfo.IsName("NormalAttack") && chargeTime > maxChargeTime)
        {
            player.eSkillEffect.gameObject.SetActive(false);
            if (chargeOn == true)
            {
                player.eSkillChargeEffect.gameObject.SetActive(true);
                chargeOn = false;
            }
        }

        if (player.ESkillOn == false)
        {
            if (chargeTime > maxChargeTime || chargeTime > 6.0f)
            {
                if (canAttack == true)
                {
                    player.PlayerAnimator.SetTrigger("OnCharge");
                }
            }

            else if (chargeTime < maxChargeTime)
            {
                if (canAttack == true)
                {
                    player.PlayerAnimator.SetTrigger("OnNormal");
                }
            }
        }

        if (player.CanStateChange == true)
        {
            if (player.AttackOn == true)
            {
                player.ChangeState(new ComboAttack1State());
                player.AttackOn = false;
                return;
            }

            if (player.MoveDir != Vector3.zero)
            {
                player.ChangeState(new RunningState());
                return;
            }

            if (player.DashOn == true)
            {
                player.PlayerAnimator.SetBool("isDash", true);
                player.Rigid.MovePosition(player.Rigid.position + player.MoveDir * Time.deltaTime * player.MoveSpeed * 1.5f);
            }

            if (player.DashOn == false || player.MoveDir == Vector3.zero)
            {
                player.PlayerAnimator.SetBool("isDash", false);
            }
        }
    }

    public void FixedUpdateState(PlayerController player)
    {

    }

    public void CheckNowState(PlayerController player)
    {
        player.StateName = "Eskill";
    }

}

public class QSkillState : IPlayerState
{
    CinemachineTrackedDolly dolly;

    public void EnterState(PlayerController player)
    {
        player.WeaponOff();
        player.PlayerAnimator.runtimeAnimatorController = player.qSkillAnimator;
        dolly = player.QCutScenecam.GetComponentInChildren<CinemachineTrackedDolly>();
        player.StartCoroutine(changeCamera(player));
        player.StartCoroutine(FireballInstantiate(player));
    }

    public IEnumerator changeCamera(PlayerController player)
    {
        player.QCutScenecam.Priority = 12;
        yield return new WaitForSeconds(2.0f);
        dolly.m_AutoDolly.m_Enabled = true;
        yield return new WaitForSeconds(3.0f);
        dolly.m_AutoDolly.m_Enabled = false;
        player.QCutScenecam.Priority = 9;
        yield return new WaitForSeconds(0.1f);
        dolly.m_PathPosition = 0;
        player.ChangeState(new IdleState());
        yield return 0;
    }

    public IEnumerator FireballInstantiate(PlayerController player)
    {
        GameObject[] fireballs = new GameObject[5];
        GameObject fireball;
        for (int i = 0; i < 5; i++)
        {
            fireball = player.InstantiateFireball(i);
            fireballs[i] = fireball;
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < 5; i++)
        {
            fireballs[i].GetComponent<Rigidbody>().AddForce(player.FireballPoints[i].transform.forward*8, ForceMode.Impulse);
        }

        yield return null;
    }


    public void FixedUpdateState(PlayerController player)
    {

    }

    public void UpdateState(PlayerController player)
    {

    }

    public void CheckNowState(PlayerController player)
    {

    }
}