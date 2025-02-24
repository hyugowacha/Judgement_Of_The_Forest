using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;



public partial class PlayerController : MonoBehaviour
{
    Animator playerAnimator;
    Vector3 moveDir;
    float turnSpeed = 10.0f;
    float moveSpeed = 4.0f;
    float maxWatingTime = 6.0f;
    Rigidbody rigid;
    Vector2 dir;
    AnimatorStateInfo animationInfo;
    bool attackOn;

    public PlayerState nowPlayerState;
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

    public void WeaponOff()
    {
        playerWeapon.SetActive(false);
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
    }

    void OnAttack()
    {
        attackOn = true;
    }

    private void Update()
    {
        playerCurrentState.UpdateState(this);
    }

    private void FixedUpdate()
    {
        playerCurrentState.FixedUpdateState(this);
        Debug.Log(playerCurrentState.ToString());
    }
}
