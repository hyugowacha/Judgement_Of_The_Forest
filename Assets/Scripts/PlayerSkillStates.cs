using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
            if(chargeOn == true)
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
