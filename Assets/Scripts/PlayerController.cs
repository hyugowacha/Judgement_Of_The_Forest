using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;



public partial class PlayerController : MonoBehaviour
{
    Animator playerAnimator;
    Vector3 moveDir;
    float turnSpeed = 20.0f;
    float moveSpeed = 4.0f;
    float maxWatingTime = 6.0f;
    float jumpPower = 5.0f;
    Rigidbody rigid;
    Vector2 dir;
    AnimatorStateInfo animationInfo;
    bool attackOn;
    bool dashOn;
    bool jumpOn;
    bool isJumping;

    public GameObject playerWeapon;
    public AnimatorController attackAnimator;
    public AnimatorController moveAnimator;
    public Collider playerFeetCollider;

    IPlayerState playerCurrentState;

    PlayerInput playerInput;
    InputActionMap mainActionMap;
    InputAction moveAction;
    InputAction attackAction;
    InputAction dashAction;
    InputAction jumpAction;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        animationInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);

        playerInput = GetComponent<PlayerInput>();
        mainActionMap = playerInput.actions.FindActionMap("PlayerActions");


        moveAction = mainActionMap.FindAction("Move");
        attackAction = mainActionMap.FindAction("Attack");
        dashAction = mainActionMap.FindAction("Dash");
        jumpAction = mainActionMap.FindAction("Jump");

        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;

        attackAction.started += OnAttack;

        dashAction.performed += OnDash;
        dashAction.canceled += OnDash;

        jumpAction.performed += OnJump;

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

    void OnMove(InputAction.CallbackContext ctx)
    {
        dir = ctx.ReadValue<Vector2>();
        moveDir = new Vector3(dir.x, 0, dir.y);
    }

    void OnAttack(InputAction.CallbackContext ctx)
    {
        attackOn = true;
    }

    void OnDash(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            dashOn = true;
        }

        else if (ctx.canceled)
        {
            dashOn = false;
        }
    }

    void OnJump(InputAction.CallbackContext ctx)
    {
        jumpOn = true;
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
