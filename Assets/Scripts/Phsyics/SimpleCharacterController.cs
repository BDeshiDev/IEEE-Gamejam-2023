using System;
using System.Collections;
using System.Collections.Generic;
using BDeshi.Input;
using BDeshi.Utility;
using BDeshi.Utility.Extensions;
using Core.Input;
using Core.Misc;
using FSM.GameState;
using UnityEngine;
using UnityEngine.Serialization;

public class SimpleCharacterController : MonoBehaviour
{
    private CharacterController cc;
    public Vector3 input;
    [SerializeField] private Vector3 inputVel;
    [SerializeField] Vector3 moveVel = Vector3.zero;
    
    [SerializeField] private float sprintSpeedMultiplier = 1.6f;
    [FormerlySerializedAs("moveSpeed")] 
    [SerializeField] private float baseMoveSpeed = 6;
    [SerializeField] private float curMoveSpeed = 6;
    [SerializeField] private Vector3 curAcc;
    [SerializeField] private float velSmoothTime = .2f;
    [SerializeField] private float maxAcc = 50;

    [SerializeField] private float rotationSpeed = 60;
    
    [SerializeField] private bool IsGrounded;

    [SerializeField] private float gravity = -15.15f;
    [SerializeField] private float maxJumpHeight = 5;
    [SerializeField] private float minJumpHeight = 2;
    [SerializeField] private float timeToJumpApex = .6f;
    private float maxJumpVelocity;
    private float minJumpVelocity;
    [SerializeField] Vector3 maxVelocityInEachAxis = Vector3.one * 40;
    public float jumpGracePeriod = .2f;

    
    [SerializeField] bool isJumping = false;
    public int jumpLimit = 1;
    [SerializeField] int remainingJumps;
    public bool GravityEnabled = true;

    public float maxKnockBack = 50;
    public float minKnockBack = .025f;
    public Vector3 knockBackBuildup;
    /// <summary>
    /// Sampled at t = 0 at knockBackBuildup = 0(not that it will go lower than minKnockBack)
    /// Sampled at t = 1 at knockBackBuildup = maxKnockBack;
    /// </summary>
    public AnimationCurve knockBackDecayCurve = AnimationCurve.Linear(0, 0, 1, 1);
    public AnimationCurve knockBackControlRecoveryCurve = AnimationCurve.Linear(0, 0, 1, 1);
    public float maxKnockBackDecay = 5;
    public Vector3 knockbackDecayVel = Vector3.zero;
    /// <summary>
    /// Time it takes to go from maxKnockBack to 0
    /// </summary>
    public float knockBackSmoothTime = .6f;
    
    [FormerlySerializedAs("boostAmount")] public Vector3 boostBuildUp;
    public Vector3 maxBoostMagnitudePerAxis =Vector3.one* 20; 
    public float boostDecay = -2f;
    public float minBoostThreshold = .15f;
    public FiniteTimer boostGravityImmunityTimer = new FiniteTimer(0, .6f);
    
    public float groundCheckRadius = .5f;
    [FormerlySerializedAs("groundHeight")] [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundLayerMask;

    public Animator animator;

    private void OnValidate()
    {
        recalcJumpAndGravity();
    }

    #region Input

    public void calcInput(out Vector3 moveInput)
    {
        moveInput = InputManager.convertVecCamRelative(
            SceneVarTracker.Instance.Camera,
            InputManager.RawMoveInput.toTopDown()
            );

        moveInput.Normalize();

        curMoveSpeed = baseMoveSpeed * (InputManager.sprintButton.isHeld ? sprintSpeedMultiplier : 1);
    }

    public void addSpeedBoost(Vector3 boost)
    {
        // moveVel += boost;
        // addKnockBack(boost);
        boostBuildUp += boost;
        clampVector(boostBuildUp, maxBoostMagnitudePerAxis);

        boostGravityImmunityTimer.reset();
        
    }
 
    /// <summary>
    /// We can have boosts in different directions
    /// Ex: boost walls and spikes
    /// They should coexists as long as the axes are diff
    /// </summary>
    void clampVector(Vector3 boostBuildUp, Vector3 maxPerAxis)
    {
        boostBuildUp.x = Mathf.Sign(boostBuildUp.x) * Mathf.Min(Mathf.Abs(boostBuildUp.x), maxPerAxis.x);
        boostBuildUp.y = Mathf.Sign(boostBuildUp.y) * Mathf.Min(Mathf.Abs(boostBuildUp.y), maxPerAxis.y);
        boostBuildUp.z = Mathf.Sign(boostBuildUp.z) * Mathf.Min(Mathf.Abs(boostBuildUp.z), maxPerAxis.z);
    }

    public void calcVelocity(Vector3 moveInput)
    {
        float yVel = moveVel.y;
        inputVel = moveInput * curMoveSpeed;
        
        //turn off acceleration
        // if (moveInput == Vector3.zero)
        // {
        //     moveVel = Vector3.SmoothDamp(moveVel, inputVel, ref curAcc, velSmoothTime, maxAcc);
        // }
        // else
        // {
        //     moveVel = Vector3.SmoothDamp(moveVel, inputVel, ref curAcc, velSmoothTime, maxAcc);
        // }
        moveVel = inputVel;

        moveVel.y = yVel;
        
        if (IsGrounded)
        {
            if (knockBackBuildup.y > 0)
            {
                moveVel.y = Mathf.Min(moveVel.y, knockBackBuildup.y );
            }
            else
            {
                moveVel.y = Mathf.Max(moveVel.y, 0);

            }
        }
        

        applyJumpVel();
        applyGravity();
    }



    #endregion
    # region  Jump and gravity

    private Vector3? groundPoint;

    Vector3 getGroundCheckEndPoint()
    {
        return getGroundCheckStartPoint()+ (groundCheckDistance - groundCheckRadius) * Vector3.down;
    }
    public bool checkGrounded()
    {
        Physics.SphereCast(getGroundCheckStartPoint(), 
            groundCheckRadius, 
            Vector3.down, 
            out var hit, 
            groundCheckDistance - groundCheckRadius,
            groundLayerMask);
        
        if (hit.collider)
        {
            groundPoint = hit.point;
        }
        else
        {
            groundPoint = null;
        }

        return hit.collider != null;
    }

    public void recalcJumpAndGravity()
    {
        gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);
    }

    public void applyJumpVel()
    {
        // the actual jump limit is refreshed whe you are grounded
        // So you can't jump infinitely midair as that defeats any sense of challenge
        if (IsGrounded)
        {
            resetJumpLimit();
        }

        if (!isJumping)
        {
            if (!IsGrounded // can only jump when NOT GROUNDED
                && InputManager.jumpButton.isHeld)
                // && InputManager.jumpButton.LastPressedTime.withinDuration(jumpGracePeriod))
            {
                if (remainingJumps > 0)
                {
                    remainingJumps -= 1;
                    handleJumpDown();   
                }
            }
        }else
        {
            if (!InputManager.jumpButton.isHeld)
            {
                handleJumpUp();
            }
        }
        // if (!IsGrounded // can only jump when NOT GROUNDED
        //     && InputManager.jumpButton.isHeld
        //     // && InputManager.jumpButton.LastPressedTime.withinDuration(jumpGracePeriod)
        //     && !isJumping)
        // {
        //     if (remainingJumps > 0)
        //     {
        //         remainingJumps -= 1;
        //         handleJumpDown();   
        //     }
        // }
        // else
        // {
        //     if (!InputManager.jumpButton.isHeld)
        //     {
        //         handleJumpUp();
        //     }
        // }
    }

    public void resetJumpLimit()
    {
        remainingJumps = jumpLimit;
    }

    public void handleJumpDown()
    {
        moveVel.y = (moveVel.y >= 0 ? moveVel.y : 0) + maxJumpVelocity;
        isJumping = true;
    }
    
    public void handleJumpUp()
    {
        moveVel.y = Mathf.Min(moveVel.y, minJumpVelocity);
        isJumping = false;
    }

    public void applyGravity()
    {
        if(IsGrounded|| !GravityEnabled)
            return;
        moveVel.y +=  gravity * Time.deltaTime;
    }
    
    #endregion
    #region Knockback

    public void calcKnockBack(out float knockbackFactor)
    {
        float knockBackMagnitude = knockBackBuildup.magnitude;
        
        if (knockBackMagnitude < minKnockBack)
        {
            knockBackBuildup = Vector3.zero;
            knockbackFactor = 0;
            knockbackDecayVel = Vector3.zero;
        }
        else
        {
            knockBackBuildup = Vector3.SmoothDamp(knockBackBuildup, Vector3.zero,
                ref knockbackDecayVel,
                knockBackSmoothTime,
                knockBackDecayCurve.Evaluate(knockBackBuildup.magnitude / maxKnockBack) * maxKnockBackDecay);
        }

        knockbackFactor = knockBackBuildup.magnitude / maxKnockBack;
    }

    public void addKnockBack(Vector3 knockBack)
    {
        knockBackBuildup = Vector3.ClampMagnitude(knockBackBuildup + knockBack, maxKnockBack);
        Debug.Log("knockBackBuildup = " + knockBackBuildup);
    }
    public float testForce = 5;
    public float groundCheckOffset = -.5f;


    Vector3 getGroundCheckStartPoint()
    {
        return transform.position + groundCheckOffset * Vector3.up;
    }
    [ContextMenu("test knockback")]
    void test()
    {
        addKnockBack(testForce * Vector3.up);
    }
    #endregion

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        
        groundCheckDistance = cc.height * .5f;
        
        resetJumpLimit();
    }

    private void Update()
    {
        if (GameStateManager.Instance.IsPaused)
        {
            return;
        }
        IsGrounded = checkGrounded();
        calcInput(out input);
        calcVelocity(input);
        calcKnockBack(out float knockbackFactor);
        clampVector(moveVel, maxVelocityInEachAxis);

        calcMoveAmount(knockbackFactor, out var moveAmount);

        applyMovement(moveAmount);
        // updateRotation();
        // animate();
    }

    void applyMovement(Vector3 moveAmount)
    {
        cc.Move(moveAmount);
    }

    private void updateRotation()
    {
        var lookDir = moveVel;
        lookDir.y = 0;
        Quaternion toRotation = Quaternion.LookRotation(lookDir, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
    }

    private void animate()
    {
        // animator.SetFloat("Speed", moveVel.magnitude / baseMoveSpeed);
        // animator.SetBool("Grounded", IsGrounded);
        // animator.SetBool("Jump" , isJumping);
        // animator.SetBool("IsMoving" , InputManager.RawMoveInput != Vector2.zero);
        // animator.SetBool("FreeFall" , moveVel.y <0 && IsGrounded);
    }


    public void calcMoveAmount(float knockBackFactor, out Vector3 moveAmount)
    {
        var actualMoveVel = moveVel;
        actualMoveVel.y = 0;
        actualMoveVel = actualMoveVel * (1-knockBackControlRecoveryCurve.Evaluate(knockBackFactor)) + knockBackBuildup;
        actualMoveVel.y = moveVel.y;

        actualMoveVel += boostBuildUp;
        updateBoost();


        moveAmount = actualMoveVel * Time.deltaTime;

    }

    private void updateBoost()
    {
        
        if (boostBuildUp.magnitude > minBoostThreshold)
        {
            float dotWithInputVel = Vector3.Dot(input, boostBuildUp);

            
            boostBuildUp -= boostBuildUp.normalized * (boostDecay * (dotWithInputVel < 0? 4:1) * Time.deltaTime);
        }
        else
        {
            boostBuildUp = Vector3.zero;
        }
        boostGravityImmunityTimer.safeUpdateTimer(Time.deltaTime);
        if (boostGravityImmunityTimer.isComplete)
        {
            if (IsGrounded)
            {
                boostBuildUp.y = 0;
            }
        }
    }

    public void teleportTo(Vector3 pos)
    {
        cc.enabled = false;
        transform.position = pos;
        cc.enabled = true;
    }

    public void stop()
    {
        moveVel = Vector3.zero;
        knockBackBuildup = Vector3.zero;
    }
    

    public void lookAlong(Vector3 dir)
    {
        transform.lookAlongTopDown(dir);
    }

    public void setRotation(Quaternion r)
    {
        transform.rotation = r;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundPoint.HasValue)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(groundPoint.Value, .25f);
        }
        else
        {
            Gizmos.color = Color.red;
        }

        var startPoint = getGroundCheckStartPoint();
        var endPoint = getGroundCheckEndPoint();
        Gizmos.DrawWireSphere(startPoint, groundCheckRadius);
        Gizmos.DrawWireSphere(endPoint, groundCheckRadius);
        Gizmos.DrawSphere(startPoint, .125f );
        Gizmos.DrawSphere(endPoint, .125f );

        Gizmos.DrawLine(startPoint, endPoint);
        
    }
}
