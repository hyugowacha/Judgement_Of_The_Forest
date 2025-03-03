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
    float turnSpeed = 20.0f;
    float moveSpeed = 4.0f;
    float maxWatingTime = 5.0f;
    float jumpPower = 4.0f;
    Rigidbody rigid;
    Vector2 dir;
    AnimatorStateInfo animationInfo;

    bool attackOn;
    bool dashOn;
    bool jumpOn;
    bool isJumping;
    bool canJump = true;
    bool eSkillOn;
    bool qSkillOn;
    bool canStateChange;
    bool areaOn;

    public GameObject playerWeapon;
    public AnimatorController attackAnimator;
    public AnimatorController moveAnimator;
    public AnimatorController eSkillAnimator;
    public AnimatorController qSkillAnimator;
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
    InputAction qSkillAction;

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
        qSkillAction = mainActionMap.FindAction("BurstSkill");

        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;

        attackAction.started += OnAttack;

        dashAction.performed += OnDash;
        dashAction.canceled += OnDash;

        jumpAction.started += OnJump;

        eSkillAction.performed += OnNormalSkill;
        eSkillAction.canceled += OnNormalSkill;

        qSkillAction.started += OnBurstSkill;

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
    void OnBurstSkill(InputAction.CallbackContext ctx)
    {
        qSkillOn = true;
    }

    public void InstantiateECharge()
    {
        var projectile =  Instantiate(eChargeProjectile, SlashPoint.transform.position, transform.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(SlashPoint.transform.forward*10, ForceMode.Impulse);
    }

    public void InstantiateENormal()
    {
        var projectile = Instantiate(eSkillProjectile, SlashPoint.transform.position, transform.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(SlashPoint.transform.forward*10, ForceMode.Impulse);
    }

    private void Update()
    {
        Debug.Log(eSkillOn);
        playerCurrentState.UpdateState(this);

    }

    private void FixedUpdate()
    {
        playerCurrentState.FixedUpdateState(this);
        //Debug.Log(rigid.velocity.y);
    }

}
