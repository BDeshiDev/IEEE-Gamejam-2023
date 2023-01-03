using System;
using System.Collections;
using System.Collections.Generic;
using BDeshi.Input;
using BDeshi.Utility.Extensions;
using Core.Input;
using UnityEngine;
using UnityEngine.Serialization;

public class SimpleCharacterController : MonoBehaviour
{
    private CharacterController cc;
    public Vector3 input;
    [SerializeField] private Vector3 inputVel;
    [SerializeField] Vector3 moveVel = Vector3.zero;
    [SerializeField] private float moveSpeed = 6;
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
    public float jumpGracePeriod = .2f;
    [SerializeField] bool isJumping = false;
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

    public float groundCheckRadius = .5f;
    [SerializeField] private float groundHeight;
    [SerializeField] private LayerMask groundLayerMask;

    public Animator animator;

    private void OnValidate()
    {
        recalcJumpAndGravity();
    }

    #region Input

    public void calcInput(out Vector3 moveInput)
    {
        moveInput = InputManager.convertVecCamRelative(InputManager.RawMoveInput.toTopDown());

        moveInput.Normalize();
    }

    public void calcVelocity(Vector3 moveInput)
    {
        float yVel = moveVel.y;
        inputVel = moveInput * moveSpeed;
        if (moveInput == Vector3.zero)
        {
            moveVel = Vector3.SmoothDamp(moveVel, inputVel, ref curAcc, velSmoothTime, maxAcc);
        }
        else
        {
            moveVel = Vector3.SmoothDamp(moveVel, inputVel, ref curAcc, velSmoothTime, maxAcc);
        }

        moveVel.y = yVel;
        
        if (IsGrounded)
        {
            moveVel.y = Mathf.Max(moveVel.y, 0);
        }
        

        applyJumpVel();
        applyGravity();
    }



    #endregion
    # region  Jump and gravity

    private Vector3? groundPoint;
    
    public bool checkGrounded()
    {
        Physics.SphereCast(transform.position +groundHeight * Vector3.up, 
            groundCheckRadius, 
            Vector3.down, 
            out var hit, 
            groundHeight -groundCheckRadius,
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
        if (IsGrounded
            && InputManager.jumpButton.isHeld
            // && InputManager.jumpButton.LastPressedTime.withinDuration(jumpGracePeriod)
            && !isJumping)
        {
            handleJumpDown();
        }
        else
        {
            if (!InputManager.jumpButton.isHeld)
            {
                handleJumpUp();
            }
        }
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
    }
    public float testForce = 5;

    void test()
    {
        addKnockBack(testForce * Vector3.forward * -1);
    }
    #endregion

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        groundHeight = cc.height * .5f;
    }

    private void Update()
    {
        IsGrounded = checkGrounded();
        calcInput(out  input);
        calcVelocity(input);
        calcKnockBack(out float knockbackFactor);
        calcMoveAmount(knockbackFactor, out var moveAmount);

        cc.Move(moveAmount);
        updateRotation();
        animate();
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
        animator.SetFloat("Speed", moveVel.magnitude / moveSpeed);
        animator.SetBool("Grounded", IsGrounded);
        animator.SetBool("Jump" , isJumping);
        animator.SetBool("IsMoving" , InputManager.RawMoveInput != Vector2.zero);
        animator.SetBool("FreeFall" , moveVel.y <0 && IsGrounded);
    }

    public void calcMoveAmount(float knockBackFactor, out Vector3 moveAmount)
    {
        var actualMoveVel = moveVel;
        actualMoveVel.y = 0;
        actualMoveVel = actualMoveVel * (1-knockBackControlRecoveryCurve.Evaluate(knockBackFactor)) + knockBackBuildup;
        actualMoveVel.y = moveVel.y;
        moveAmount = actualMoveVel * Time.deltaTime;
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
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(groundPoint.Value, .25f);

        }
        Gizmos.DrawWireSphere(transform.position + (groundCheckRadius) * Vector3.up, groundCheckRadius);
    }
}
