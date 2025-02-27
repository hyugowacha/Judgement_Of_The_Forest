using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAttack1State : IPlayerState
{
    public void EnterState(PlayerController player)
    {
        player.AnimationInfo = player.PlayerAnimator.GetCurrentAnimatorStateInfo(0);

        if (player.AnimationInfo.IsName("Attack_Idle"))
        {
            player.PlayerAnimator.SetTrigger("Attack");
        }

        else
        {
            player.PlayerAnimator.runtimeAnimatorController = player.attackAnimator;
        }

        player.AttackOn = false;
        player.PlayerWeapon.SetActive(true);
        player.PlayerAnimator.applyRootMotion = true;

    }

    public void UpdateState(PlayerController player)
    {
        player.AnimationInfo = player.PlayerAnimator.GetCurrentAnimatorStateInfo(0);

        if (player.AnimationInfo.IsName("Combo1") && player.AnimationInfo.normalizedTime >= 1.0f)
        {
            player.ChangeState(new AttackOnState());
            return;
        }

        if (player.AttackOn == true && player.AnimationInfo.normalizedTime >= 0.5f)
        {
            player.ChangeState(new ComboAttack2State());
            return;
        }

    }

    public void FixedUpdateState(PlayerController player)
    {
    }

    public void CheckNowState(PlayerController player)
    {
        player.StateName = "ATTACKING";
    }
}

public class ComboAttack2State : IPlayerState
{
    bool playerCanTurn;

    public void EnterState(PlayerController player)
    {
        playerCanTurn = true;
        player.AnimationInfo = player.PlayerAnimator.GetCurrentAnimatorStateInfo(0);

        if (player.AnimationInfo.IsName("Combo1"))
        {
            player.PlayerAnimator.SetTrigger("Attack2");
        }
        player.AttackOn = false;
    }

    public void UpdateState(PlayerController player)
    {
        player.AnimationInfo = player.PlayerAnimator.GetCurrentAnimatorStateInfo(0);

        if (player.AnimationInfo.IsName("Combo2") && player.AnimationInfo.normalizedTime >= 1.0f)
        {
            player.ChangeState(new AttackOnState());
            return;
        }

        if (player.AttackOn == true && player.AnimationInfo.normalizedTime >= 0.5f)
        {
            player.ChangeState(new ComboAttack3State());
            return;
        }
    }

    public void FixedUpdateState(PlayerController player)
    {
        if (playerCanTurn == true && player.MoveDir != Vector3.zero)
        {
            Quaternion PlayerTurn = Quaternion.LookRotation(player.MoveDir);
            player.Rigid.MoveRotation(Quaternion.RotateTowards(player.Rigid.rotation, PlayerTurn, player.TurnSpeed * 100));
            playerCanTurn = false;
        }
    }

    public void CheckNowState(PlayerController player)
    {
        player.StateName = "ATTACKING";
    }
}

public class ComboAttack3State : IPlayerState
{
    bool playerCanTurn;

    public void EnterState(PlayerController player)
    {
        playerCanTurn = true;
        player.AnimationInfo = player.PlayerAnimator.GetCurrentAnimatorStateInfo(0);

        if (player.AnimationInfo.IsName("Combo2"))
        {
            player.PlayerAnimator.SetTrigger("Attack3");
        }

        player.AttackOn = false;
    }

    public void UpdateState(PlayerController player)
    {
        player.AnimationInfo = player.PlayerAnimator.GetCurrentAnimatorStateInfo(0);

        if (player.AnimationInfo.IsName("Combo3") && player.AnimationInfo.normalizedTime >= 1.0f)
        {
            player.ChangeState(new AttackOnState());
            return;
        }

        if (player.AttackOn == true && player.AnimationInfo.normalizedTime >= 0.5f)
        {
            player.ChangeState(new ComboAttack4State());
            return;
        }
    }

    public void FixedUpdateState(PlayerController player)
    {
        if (playerCanTurn == true && player.MoveDir != Vector3.zero)
        {
            Quaternion PlayerTurn = Quaternion.LookRotation(player.MoveDir);
            player.Rigid.MoveRotation(Quaternion.RotateTowards(player.Rigid.rotation, PlayerTurn, player.TurnSpeed * 100));
            playerCanTurn = false;
        }
    }

    public void CheckNowState(PlayerController player)
    {
        player.StateName = "ATTACKING";
    }
}

public class ComboAttack4State : IPlayerState
{
    bool playerCanTurn;

    public void EnterState(PlayerController player)
    {
        playerCanTurn = true;
        player.AnimationInfo = player.PlayerAnimator.GetCurrentAnimatorStateInfo(0);

        if (player.AnimationInfo.IsName("Combo3"))
        {
            player.PlayerAnimator.SetTrigger("Attack4");
        }

        player.AttackOn = false;
    }

    public void UpdateState(PlayerController player)
    {
        player.AnimationInfo = player.PlayerAnimator.GetCurrentAnimatorStateInfo(0);

        if (player.AnimationInfo.IsName("Attack_Idle"))
        {
            player.ChangeState(new AttackOnState());
            return;
        }
    }

    public void FixedUpdateState(PlayerController player)
    {
        if (playerCanTurn == true && player.MoveDir != Vector3.zero)
        {
            Quaternion PlayerTurn = Quaternion.LookRotation(player.MoveDir);
            player.Rigid.MoveRotation(Quaternion.RotateTowards(player.Rigid.rotation, PlayerTurn, player.TurnSpeed * 100));
            playerCanTurn = false;
        }
    }

    public void CheckNowState(PlayerController player)
    {
        player.StateName = "ATTACKING";
    }
}
