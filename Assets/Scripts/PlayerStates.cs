using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IPlayerState
{
    void EnterState(PlayerController player);
    void UpdateState(PlayerController player);
    void FixedUpdateState(PlayerController player);
    void CheckNowState(PlayerController player);
}

public class IdleState : IPlayerState
{
    float waitTime;
    float maxWaitTime = 10.0f;

    public void EnterState(PlayerController player)
    {
        player.WeaponOff();
        player.Rigid.velocity = new Vector3(player.Rigid.velocity.x, 0, player.Rigid.velocity.z);
        player.playerAnimator.applyRootMotion = false;
        player.playerAnimator.SetFloat("isRun", player.Dir.magnitude);

    }

    public void UpdateState(PlayerController player)
    {
        if (player.MoveDir != Vector3.zero && player.AnimationInfo.IsName("Land") == false)
        {
            player.ChangeState(new RunningState());
            return;
        }

        if (player.AttackOn == true)
        {
            player.ChangeState(new ComboAttack1State());
            return;
        }

        if (player.Rigid.velocity.y <= -1)
        {
            player.ChangeState(new FallingState());
        }

        if (player.JumpOn == true)
        {
            player.ChangeState(new JumpingState());
            player.JumpOn = false;
            return;
        }

        if (player.ESkillOn == true)
        {
            player.ChangeState(new ESkillState());
            return;
        }

        if (player.QSkillOn == true)
        {
            player.ChangeState(new QSkillState());
            player.QSkillOn = false;
            return;
        }

    }

    public void CheckNowState(PlayerController player)
    {
        player.StateName = "IDLE";
    }

    public void FixedUpdateState(PlayerController player)
    {
        //Debug.Log(waitTime);

        if (player.AnimationInfo.IsName("Waiting") == false) 
        {
            waitTime += Time.deltaTime;
        }

        else
        {
            waitTime = 0;
        }

        if (waitTime > maxWaitTime)
        {
            player.playerAnimator.SetTrigger("onWait");
        }
    }
}

public class RunningState : IPlayerState
{
    Vector3 desiredMoveDir;
    public void CheckNowState(PlayerController player)
    {
        player.StateName = "RUNNING";
    }

    public void EnterState(PlayerController player)
    {

        player.playerAnimator.runtimeAnimatorController = player.moveAnimator;
        player.WeaponOff();
        player.playerAnimator.applyRootMotion = false;
    }

    public void FixedUpdateState(PlayerController player)
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0f;

        desiredMoveDir = cameraForward * player.MoveDir.z + cameraRight * player.MoveDir.x;
        desiredMoveDir.Normalize();

        if(desiredMoveDir.magnitude > 0f)
        {
            Quaternion PlayerTurn = Quaternion.LookRotation(desiredMoveDir);
            player.Rigid.MoveRotation(Quaternion.RotateTowards(player.Rigid.rotation, 
                PlayerTurn, player.TurnSpeed));
        }

        player.Rigid.MovePosition(player.Rigid.position + desiredMoveDir 
            * Time.deltaTime * player.MoveSpeed);
        
        
        player.playerAnimator.SetFloat("isRun", player.Dir.magnitude);
    }

    public void UpdateState(PlayerController player)
    {
        if (player.MoveDir == Vector3.zero && player.StateName != "FALLING")
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
            player.playerAnimator.SetBool("isDash", true);
            player.Rigid.MovePosition(player.Rigid.position + desiredMoveDir * Time.deltaTime * player.MoveSpeed * 1.5f);
        }

        if (player.DashOn == false || player.MoveDir == Vector3.zero)
        {
            player.playerAnimator.SetBool("isDash", false);
        }


        if (player.JumpOn == true)
        {
            player.ChangeState(new JumpingState());
            player.JumpOn = false;
            return;
        }
        if (player.Rigid.velocity.y <= -6)
        {
            player.ChangeState(new FallingState());
        }

        if (player.ESkillOn == true)
        {
            player.ChangeState(new ESkillState());
            return;
        }

        if (player.QSkillOn == true)
        {
            player.ChangeState(new QSkillState());
            player.QSkillOn = false;
            return;
        }
    }
}

public class FallingState : IPlayerState
{
    bool isGrounded = false;
    Coroutine isGroundedCor;

    public void EnterState(PlayerController player)
    {
        player.playerAnimator.SetBool("isFalling", true);
        isGroundedCor = player.StartCoroutine(CheckisGrounded(player));
    }

    public void UpdateState(PlayerController player)
    {
        if (isGrounded == true)
        {
            if (isGroundedCor != null)
            {
                isGroundedCor = null;
            }
            player.ChangeState(new IdleState());
        }

    }

    public void FixedUpdateState(PlayerController player)
    {

    }

    public IEnumerator CheckisGrounded(PlayerController player)
    {
        RaycastHit hit;
        Debug.DrawRay(new Vector3(player.transform.position.x, player.transform.position.y +
            0.9f, player.transform.position.z), Vector3.down, Color.red, 0);

        //yield return new WaitForSeconds(0.2f);

        while (true)
        {
            bool groundDetected = Physics.Raycast(new Vector3(player.transform.position.x, player.transform.position.y +
                0.9f, player.transform.position.z), Vector3.down, out hit, 1.2f, LayerMask.GetMask("Ground"));

            if (groundDetected == true && hit.collider != null)
            {
                isGrounded = true;
                player.playerAnimator.SetBool("isFalling", false);
                yield return new WaitForSeconds(0.2f);
                player.CanJump = true;
                yield return new WaitForSeconds(0.2f);
                yield break;
            }

            else
            {
                isGrounded = false;
            }

            yield return null;
        }
    }

    public void CheckNowState(PlayerController player)
    {
        player.StateName = "FALLING";
    }
}

public class JumpingState : IPlayerState
{
    Coroutine changeToFallingcor;

    public void EnterState(PlayerController player)
    {
        if (player.CanJump == true) //점프가 가능한 상태라면
        {
            player.CanStateChange = false;
            player.playerAnimator.SetTrigger("isJump");//점프하면서 수행할 로직
            player.CanJump = false;
            player.Rigid.velocity = Vector3.zero;

            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0f;
            cameraForward.Normalize();

            Vector3 cameraRight = Camera.main.transform.right;
            cameraRight.y = 0f;

            Vector3 desiredMoveDir = cameraForward * player.MoveDir.z + cameraRight * player.MoveDir.x;
            desiredMoveDir.Normalize();

            Vector3 jumpDirection = (desiredMoveDir * player.MoveSpeed) + (Vector3.up * player.JumpPower);

            player.Rigid.AddForce(jumpDirection, ForceMode.Impulse);
            changeToFallingcor = player.StartCoroutine(ChangeToFalling(player));
        }
    }

    public IEnumerator ChangeToFalling(PlayerController player)
    {
        yield return new WaitForSeconds(0.3f);
        player.ChangeState(new FallingState());
    }

    public void UpdateState(PlayerController player)
    {
    }

    public void CheckNowState(PlayerController player)
    {
        player.StateName = "JUMPING";
    }

    public void FixedUpdateState(PlayerController player)
    {
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
        player.AnimationInfo = player.playerAnimator.GetCurrentAnimatorStateInfo(0);
        player.AttackOn = false;
    }

    public void UpdateState(PlayerController player)
    {
        if (!player.AnimationInfo.IsName("Attack_Idle"))
        {
            player.AnimationInfo = player.playerAnimator.GetCurrentAnimatorStateInfo(0);
        }

        if (player.AttackOn == true && player.AnimationInfo.IsName("Attack_Idle"))
        {
            player.ChangeState(new ComboAttack1State());
        }

        if (player.QSkillOn == true)
        {
            player.ChangeState(new QSkillState());
            player.QSkillOn = false;
            return;
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
                player.playerAnimator.SetTrigger("AttackOff");
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
        if (player.playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("SheathingSword"))
        {
            float animationLength = player.playerAnimator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSecondsRealtime(animationLength);
            player.ChangeState(new IdleState());
            CanWeaponoff = false;
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
