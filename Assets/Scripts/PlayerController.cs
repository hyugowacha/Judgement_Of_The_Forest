using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    Vector3 moveDir;
    float TurnSpeed = 15.0f;
    float moveSpeed = 4.0f;
    Rigidbody rigid;

    public GameObject PlayerWeapon;
    public AnimatorController AttackAnimator;
    public AnimatorController MoveAnimator;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (moveDir != Vector3.zero)
        {
            Quaternion PlayerTurn = Quaternion.LookRotation(moveDir);
            rigid.MoveRotation(Quaternion.RotateTowards(rigid.rotation, PlayerTurn, TurnSpeed));
            rigid.MovePosition(rigid.position + moveDir * Time.deltaTime * moveSpeed);

        }
    }


    void OnMove(InputValue val)
    {
        PlayerWeapon.SetActive(false);
        animator.runtimeAnimatorController = MoveAnimator;
        Vector2 dir = val.Get<Vector2>();
        moveDir = new Vector3(dir.x, 0, dir.y);

        animator.SetFloat("isRun", dir.magnitude);
    }

    void OnAttack()
    {
        Debug.Log("АјАн");
        animator.runtimeAnimatorController = AttackAnimator;
        PlayerWeapon.SetActive(true);
        animator.applyRootMotion = true;
    }
}
