using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    void EnterState(PlayerController player);
    void UpdateState(PlayerController player);
    void FixedUpdateState(PlayerController player);
}

public class IdleState : IPlayerState 
{
    public void EnterState(PlayerController player)
    {
        player.PlayerAnimator.SetFloat("isRun", player.Dir.magnitude);
        player.PlayerAnimator.applyRootMotion = false;
    }

    public void UpdateState(PlayerController player)
    {
        
    }

    public void FixedUpdateState(PlayerController player)
    {
        
    }
}

public class RunningState : IPlayerState
{
    public void EnterState(PlayerController player)
    {
        if (player.PlayerWeapon.activeSelf == true)
        {
            player.PlayerWeapon.SetActive(false);
        }
        player.PlayerAnimator.applyRootMotion = false;
    }

    public void FixedUpdateState(PlayerController player)
    {
        
    }

    public void UpdateState(PlayerController player)
    {       
        Quaternion PlayerTurn = Quaternion.LookRotation(player.MoveDir);
        player.Rigid.MoveRotation(Quaternion.RotateTowards(player.Rigid.rotation, PlayerTurn, player.TurnSpeed));
        player.Rigid.MovePosition(player.Rigid.position + player.MoveDir * Time.deltaTime * player.MoveSpeed);

        player.PlayerAnimator.runtimeAnimatorController = player.MoveAnimator;        
        player.PlayerAnimator.SetFloat("isRun", player.Dir.magnitude);

    }

}

public class AttackState : IPlayerState
{
    float watingtime;
    public void EnterState(PlayerController player)
    {
        player.PlayerAnimator.runtimeAnimatorController = player.AttackAnimator;
        player.PlayerWeapon.SetActive(true);
        player.PlayerAnimator.applyRootMotion = true;
    }

    public void FixedUpdateState(PlayerController player)
    {
    }

    public void UpdateState(PlayerController player)
    {
        if (player.AnimationInfo.IsName("Attack_Idle"))
        {
            watingtime += Time.deltaTime;

            Debug.Log(watingtime);

            if (watingtime > player.MaxWatingTime)
            {
                player.PlayerAnimator.SetTrigger("AttackOff");
            }
        }
    }
}
