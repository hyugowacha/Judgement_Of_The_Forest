using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;



public partial class PlayerController : MonoBehaviour
{
    Animator playerAnimator;
    Vector3 moveDir;
    float turnSpeed = 5.0f;
    float moveSpeed = 4.0f;
    float maxWatingTime = 10.0f;
    Rigidbody rigid;
    Vector2 dir;
    AnimatorStateInfo animationInfo;

    public GameObject playerWeapon;
    public AnimatorController attackAnimator;
    public AnimatorController moveAnimator;

    IPlayerState playerCurrentState;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        animationInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);

        ChangeState(new IdleState());
    }

    public void ChangeState(IPlayerState newState)
    {
        playerCurrentState = newState;
        playerCurrentState.EnterState(this);
    }

    void OnMove(InputValue val)
    {
        dir = val.Get<Vector2>();
        moveDir = new Vector3(dir.x, 0, dir.y);

        if (moveDir != Vector3.zero)
        {
            ChangeState(new RunningState());
        }

        else
        {
            ChangeState(new IdleState());
        }
    }

    void OnAttack()
    {
        Debug.Log("АјАн");
        

        ChangeState(new AttackState());

        
    }

    private void Update()
    {
        playerCurrentState.UpdateState(this);
        Debug.Log(playerCurrentState);
    }
}
