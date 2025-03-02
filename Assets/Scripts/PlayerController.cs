using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;


public partial class PlayerController : MonoBehaviour
{
    Animator playerAnimator;
    Vector3 moveDir;
    string stateName;
    float turnSpeed = 30.0f;
    float moveSpeed = 4.0f;
    float maxWatingTime = 5.0f;
    float jumpPower = 3.0f;
    Rigidbody rigid;
    Vector2 dir;
    AnimatorStateInfo animationInfo;

    bool attackOn;
    bool dashOn;
    bool jumpOn;
    bool isJumping;
    bool canJump = true;
    bool eSkillOn;
    bool canStateChange;
    bool areaOn;

    public GameObject playerWeapon;
    public AnimatorController attackAnimator;
    public AnimatorController moveAnimator;
    public AnimatorController eSkillAnimator;
    public Collider attackArea;
    public ParticleSystem eSkillEffect;
    public ParticleSystem eSkillChargeEffect;
    public GameObject SlashPoint;
    public GameObject eSkillProjectile;
    public GameObject eChargeProjectile;

    IPlayerState playerCurrentState;

    PlayerInput playerInput;
    InputActionMap mainActionMap;
    InputAction moveAction;
    InputAction attackAction;
    InputAction dashAction;
    InputAction jumpAction;

    InputAction eSkillAction;

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
        eSkillAction = mainActionMap.FindAction("NormalSkill");

        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;

        attackAction.started += OnAttack;

        dashAction.performed += OnDash;
        dashAction.canceled += OnDash;

        jumpAction.started += OnJump;

        eSkillAction.performed += OnNormalSkill;
        eSkillAction.canceled += OnNormalSkill;

        ChangeState(new IdleState());
    }

    public void WeaponOff()
    {
        playerWeapon.SetActive(false);
    }

    public void CanStateChangeValChanger()
    {
        if (canStateChange == false)
        {
            canStateChange = true;
        }
    }

    public void ChangeToIdle()
    {
        ChangeState(new IdleState());
    }

    public void ParticleOff()
    {
        eSkillEffect.gameObject.SetActive(false);
        eSkillChargeEffect.gameObject.SetActive(false);
    }

    public void ChangeState(IPlayerState newState)
    {
        playerCurrentState = newState;
        playerCurrentState.EnterState(this);
        playerCurrentState.CheckNowState(this);
    }

    void OnMove(InputAction.CallbackContext ctx)
    {
        dir = ctx.ReadValue<Vector2>();
        moveDir = new Vector3(dir.x, 0, dir.y);
    }

    void OnAttack(InputAction.CallbackContext ctx)
    {
        if (stateName != "JUMPING" || stateName != "FALLING")
        {
            attackOn = true;
        }
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
        if (stateName != "ATTACKING" && CanJump == true)
        {
            jumpOn = true;
        }
    }

    void OnNormalSkill(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            eSkillOn = true;
        }

        if (ctx.canceled)
        {
            eSkillOn = false;
        }
    }

    public void InstantiateECharge()
    {
        Instantiate(eChargeProjectile, SlashPoint.transform.position, transform.rotation);
    }

    public void InstantiateENormal()
    {
        Instantiate(eSkillProjectile, SlashPoint.transform.position, transform.rotation);
    }

    private void Update()
    {
        Debug.Log(stateName);
        //Debug.Log(playerCurrentState.ToString());
        //Debug.Log(canJump);
        Debug.Log(eSkillOn);
        playerCurrentState.UpdateState(this);

    }

    private void FixedUpdate()
    {
        playerCurrentState.FixedUpdateState(this);
        //Debug.Log(rigid.velocity.y);
    }

}
