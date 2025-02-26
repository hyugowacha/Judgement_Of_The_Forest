using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        player.PlayerAnimator.applyRootMotion = false;
        player.PlayerAnimator.runtimeAnimatorController = player.moveAnimator;
        player.PlayerAnimator.SetFloat("isRun", player.Dir.magnitude);
    }

    public void UpdateState(PlayerController player)
    {
        if (player.IsJumping == true)
        {
            return;
        }

        if (player.MoveDir != Vector3.zero)
        {
            player.ChangeState(new RunningState());
            return;
        }

        if (player.AttackOn == true)
        {
            player.ChangeState(new ComboAttack1State());
            return;
        }

        if (player.JumpOn == true)
        {
            player.ChangeState(new JumpingState());
            player.JumpOn = false;
            player.IsJumping = true;
            return;
        }
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
        Quaternion PlayerTurn = Quaternion.LookRotation(player.MoveDir);
        player.Rigid.MoveRotation(Quaternion.RotateTowards(player.Rigid.rotation, PlayerTurn, player.TurnSpeed));
        player.Rigid.MovePosition(player.Rigid.position + player.MoveDir * Time.deltaTime * player.MoveSpeed);

        player.PlayerAnimator.runtimeAnimatorController = player.moveAnimator;
        player.PlayerAnimator.SetFloat("isRun", player.Dir.magnitude);
    }

    public void UpdateState(PlayerController player)
    {
        if (player.IsJumping == true)
        {
            return;
        }

        if (player.MoveDir == Vector3.zero)
        {
            player.ChangeState(new IdleState());
            return;
        }

        if (player.AttackOn == true)
        {
            player.ChangeState(new ComboAttack1State());
            player.AttackOn = false;
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

        if (player.JumpOn == true)
        {
            player.ChangeState(new JumpingState());
            player.JumpOn = false;
            player.IsJumping = true;
            return;
        }

    }
}

public class JumpingState : IPlayerState
{
    bool canJump = true;
    bool isGrounded = true;

    Coroutine isGroundedCor;

    public void EnterState(PlayerController player)
    {
        if (canJump == true)
        {
            Debug.Log("Á¡ÇÁ");
            player.PlayerAnimator.SetTrigger("isJump");
            player.Rigid.AddForce(new Vector3(0, player.JumpPower, 0), ForceMode.Impulse);
            canJump = false;
        }
    }

    public void UpdateState(PlayerController player)
    {
        Debug.Log("¶¥" + isGrounded);      
        isGroundedCor = player.StartCoroutine(CheckisGrounded(player));

        if (isGrounded == true)
        {
            player.PlayerAnimator.SetTrigger("isLand");
            canJump = true;
            player.ChangeState(new IdleState());
            player.IsJumping = false;
        }

        else
        {
            canJump = false;
        }
    }

    public void FixedUpdateState(PlayerController player)
    {

        if (player.Rigid.velocity.y != 0)
        {
            Quaternion PlayerTurn = Quaternion.LookRotation(player.MoveDir * 0.5f);
            player.Rigid.MoveRotation(Quaternion.RotateTowards(player.Rigid.rotation, PlayerTurn, player.TurnSpeed * 0.5f));
        }
    }

    public IEnumerator CheckisGrounded(PlayerController player)
    {
        RaycastHit hit;
        Debug.DrawRay(new Vector3(player.transform.position.x, player.transform.position.y + 0.9f, player.transform.position.z), Vector3.down, Color.red, 0);

        yield return new WaitForSeconds(0.5f);

        Physics.Raycast(new Vector3(player.transform.position.x, player.transform.position.y + 0.9f, player.transform.position.z), Vector3.down, out hit, 0);

        if (hit.collider != null && hit.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        else
        {
            isGrounded = false;
        }
    }



}

public class AttackOnState : IPlayerState
{
    float watingtime;
    Coroutine WeaponOffCoroutine;
    bool CanWeaponoff;
    ComboAttack1State comboAttack1 = new();

    public void EnterState(PlayerController player)
    {
        player.AnimationInfo = player.PlayerAnimator.GetCurrentAnimatorStateInfo(0);
        player.AttackOn = false;
    }

    public void UpdateState(PlayerController player)
    {
        if (!player.AnimationInfo.IsName("Attack_Idle"))
        {
            player.AnimationInfo = player.PlayerAnimator.GetCurrentAnimatorStateInfo(0);
        }

        if (player.AttackOn == true && player.AnimationInfo.IsName("Attack_Idle"))
        {
            player.ChangeState(new ComboAttack1State());
        }

        if (player.AnimationInfo.IsName("Attack_Idle"))
        {
            watingtime += Time.deltaTime;

            if (player.MoveDir != Vector3.zero)
            {
                player.ChangeState(new RunningState());
                return;
            }

            if (watingtime > player.MaxWatingTime)
            {
                player.PlayerAnimator.SetTrigger("AttackOff");
                WeaponOffCoroutine = player.StartCoroutine(EndAttackState(player));
            }

            if (WeaponOffCoroutine != null && CanWeaponoff == false)
            {
                player.StopCoroutine(WeaponOffCoroutine);
                WeaponOffCoroutine = null;
                CanWeaponoff = true;
            }

        }
    }

    IEnumerator EndAttackState(PlayerController player)
    {
        if (player.PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Sheathing Sword"))
        {
            float animationLength = player.PlayerAnimator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSecondsRealtime(animationLength);

            player.ChangeState(new IdleState());
            CanWeaponoff = false;
        }

    }

    public void FixedUpdateState(PlayerController player)
    {
    }

}
